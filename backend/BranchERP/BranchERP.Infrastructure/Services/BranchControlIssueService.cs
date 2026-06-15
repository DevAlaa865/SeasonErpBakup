using AutoMapper;
using AutoMapper.QueryableExtensions;
using BranchERP.Application.DTOs.BranchControlIssues;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class BranchControlIssueService : IBranchControlIssueService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BranchControlIssueService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddIssuesAsync(List<CreateBranchControlIssueDto> issues, string userName)
    {
        foreach (var dto in issues)
        {
            bool exists = await _context.BranchControlIssues
                .AnyAsync(x => x.SalesDailyId == dto.SalesDailyId);

            if (exists)
                continue;

            var entity = _mapper.Map<BranchControlIssue>(dto);

            entity.SentAt = DateTime.Now;
            entity.SentByUser = dto.SentByUser ?? userName;

            // الحالة الافتراضية
            entity.ResolutionType = ResolutionType.UnderReview;
            entity.Status = BranchControlIssueStatus.Pending;

            // ⭐ تحديد نوع المبلغ (عجز / زيادة)
            entity.DifferenceDirection = dto.DifferenceDirection;
            //if (entity.DifferenceAmount < 0)
            //    entity.DifferenceDirection = DifferenceDirection.Shortage;
            //else if (entity.DifferenceAmount > 0)
            //    entity.DifferenceDirection = DifferenceDirection.Increase;
            //else
            //    entity.DifferenceDirection = DifferenceDirection.Shortage; // أو أي Default تحبه

            await _context.BranchControlIssues.AddAsync(entity);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<BranchControlIssueListDto>> GetAllIssuesAsync(BranchControlIssueFilterDto filter)
    {
        var query = _context.BranchControlIssues
            .Include(x => x.Branch)
            .AsQueryable();

        if (filter.BranchId.HasValue)
            query = query.Where(x => x.BranchId == filter.BranchId.Value);

        if (filter.FromDate.HasValue)
            query = query.Where(x => x.SalesDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(x => x.SalesDate <= filter.ToDate.Value);

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);

        if (filter.ResolutionType.HasValue)
            query = query.Where(x => x.ResolutionType == filter.ResolutionType.Value);

        // ⭐ فلتر نوع المبلغ (عجز / زيادة)
        if (filter.DifferenceDirection.HasValue)
            query = query.Where(x => x.DifferenceDirection == filter.DifferenceDirection.Value);

        return await query
            .ProjectTo<BranchControlIssueListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    // ============================
    // 2) Get Issue By Id
    // ============================
    public async Task<BranchControlIssueListDto?> GetIssueByIdAsync(int id)
    {
        var entity = await _context.BranchControlIssues
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            return null;

        return _mapper.Map<BranchControlIssueListDto>(entity);
    }

    // ============================
    // 3) Update Issue (Status + Notes + ResolutionType)
    // ============================
    public async Task<bool> UpdateIssueAsync(UpdateBranchControlIssueDto dto)
    {
        var entity = await _context.BranchControlIssues
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (entity == null)
            return false;

        entity.Status = dto.Status;
        entity.ResolutionType = dto.ResolutionType;
        entity.ControlNotes = dto.ControlNotes;

        if (dto.Status == BranchControlIssueStatus.Resolved)
            entity.ResolvedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<List<ManagerBranchControlIssueListDto>> GetManagerReportAsync(ManagerBranchControlIssueFilterDto filter)
    {
        var query = _context.BranchControlIssues
            .Include(x => x.Branch)
            .AsQueryable();

        if (filter.FromDate.HasValue)
            query = query.Where(x => x.SalesDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(x => x.SalesDate <= filter.ToDate.Value);

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);

        // المدير يشوف فقط الحالات اللي تم حلها أو تحت المراجعة
        query = query.Where(x => x.Status == BranchControlIssueStatus.Resolved
                              || x.Status == BranchControlIssueStatus.InProgress);

        return await query
            .ProjectTo<ManagerBranchControlIssueListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> ManagerApproveAsync(ManagerApproveDto dto)
    {
        var issue = await _context.BranchControlIssues.FindAsync(dto.Id);

        if (issue == null)
            return false;

        issue.IsManagerApproved = dto.IsManagerApproved;
        issue.ManagerSignature = dto.ManagerSignature;
        issue.ManagerNotes = dto.ManagerNotes;
        issue.ManagerApprovedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<AccountantBranchControlIssueListDto>> GetAccountantReportAsync(AccountantBranchControlIssueFilterDto filter)
    {
        var query = _context.BranchControlIssues
            .Include(x => x.Branch)
            .AsQueryable();

        // لأن FromDate و ToDate مش Nullable
        query = query.Where(x => x.SalesDate >= filter.FromDate);
        query = query.Where(x => x.SalesDate <= filter.ToDate);

        if (filter.IsManagerApproved.HasValue)
            query = query.Where(x => x.IsManagerApproved == filter.IsManagerApproved.Value);

        return await query
            .OrderBy(x => x.SalesDate)
            .ThenBy(x => x.Branch.BranchNumber)
            .ProjectTo<AccountantBranchControlIssueListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<AccountantBranchControlIssueDetailsDto?> GetAccountantDetailsAsync(int id)
    {
        var entity = await _context.BranchControlIssues
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<AccountantBranchControlIssueDetailsDto>(entity);
    }

}
