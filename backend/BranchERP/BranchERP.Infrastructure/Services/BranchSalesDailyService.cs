using AutoMapper;
using BranchERP.Application.DTOs.BranchSalesDaily;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.DTOs.Reports;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Domain.Enums;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class BranchSalesDailyService : IBranchSalesDailyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public BranchSalesDailyService(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<BranchSalesDailyDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<BranchSalesDaily>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(d => d.Branch)
                    .Include(d => d.Supervisor)
                    .Include(d => d.ShortageDetails)
                        .ThenInclude(s => s.ShortageType)
            );

            if (entity == null)
                return ApiResponse<BranchSalesDailyDto>.Fail("Record not found");

            var dto = _mapper.Map<BranchSalesDailyDto>(entity);
            return ApiResponse<BranchSalesDailyDto>.Ok(dto);
        }

        public async Task<ApiResponse<IReadOnlyList<BranchSalesDailyDto>>> GetByBranchAndDateAsync(int branchId, DateTime date)
        {
            var repo = _unitOfWork.Repository<BranchSalesDaily>();

            var items = await repo.GetAllAsync(
                filter: x => x.BranchId == branchId && x.SalesDate.Date == date.Date,
                include: q => q
                    .Include(d => d.Branch)
                    .Include(d => d.Supervisor)
                    .Include(d => d.ShortageDetails)
                        .ThenInclude(s => s.ShortageType)
            );

            var data = _mapper.Map<IReadOnlyList<BranchSalesDailyDto>>(items);
            return ApiResponse<IReadOnlyList<BranchSalesDailyDto>>.Ok(data);
        }

        public async Task<ApiResponse<BranchSalesDailyDto>> CreateAsync(BranchSalesDailyCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<BranchSalesDaily>();
            // 🔥 1) تحقق: هل فيه يومية لنفس الفرع ونفس التاريخ؟
            var exists = await _context.BranchSalesDailies
                .AnyAsync(d => d.BranchId == model.BranchId
                            && d.SalesDate.Date == model.SalesDate.Date);

            if (exists)
            {
                return ApiResponse<BranchSalesDailyDto>.Fail(
                    "تم تسجيل يومية لهذا الفرع في هذا التاريخ بالفعل."
                );
            }
            var entity = _mapper.Map<BranchSalesDaily>(model);

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var dto = await GetByIdAsync(entity.Id);
            return dto;
        }
        public async Task<bool> ExistsAsync(int branchId, DateTime date)
        {
            return await _context.BranchSalesDailies
                .AnyAsync(d => d.BranchId == branchId && d.SalesDate.Date == date.Date);
        }
        public async Task<ApiResponse<BranchSalesDailyDto>> UpdateAsync(int id, BranchSalesDailyCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<BranchSalesDaily>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q.Include(d => d.ShortageDetails)
            );

            if (entity == null)
                return ApiResponse<BranchSalesDailyDto>.Fail("Record not found");

            // تحديث الحقول الأساسية
            entity.BranchId = model.BranchId;
            entity.SupervisorId = model.SupervisorId;
            entity.SalesDate = model.SalesDate;
            entity.NoSalesToday = model.NoSalesToday;
            entity.AttachmentPath = model.AttachmentPath;
            entity.GrandTotal = model.GrandTotal;
            entity.TotalSales = model.TotalSales;
            entity.CashAmount = model.CashAmount;
            entity.NetworkAmount = model.NetworkAmount;
            entity.CreditAmount = model.CreditAmount;
            entity.Difference = model.Difference;

            entity.IsBalanced = model.IsBalanced;
            entity.HasShortage = model.HasShortage;

            entity.SupervisorNotes = model.SupervisorNotes;
            entity.AccountingNotes = model.AccountingNotes;
            entity.AuditNotes = model.AuditNotes;
            entity.FinanceNotes = model.FinanceNotes;
            entity.DataEntryUserName = model.DataEntryUserName;
            entity.ReturnsDeptNotes = model.ReturnsDeptNotes;
            entity.DiscountsDeptNotes = model.DiscountsDeptNotes;

            entity.TotalInvoicesCount = model.TotalInvoicesCount;
            entity.TotalQuantities = model.TotalQuantities;

            // إعادة بناء تفاصيل العجز
            entity.ShortageDetails.Clear();
            foreach (var d in model.ShortageDetails)
            {
                var detail = _mapper.Map<BranchSalesShortageDetail>(d);
                entity.ShortageDetails.Add(detail);
            }

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            var dto = await GetByIdAsync(entity.Id);
            return dto;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<BranchSalesDaily>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.Fail("Record not found");

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true, "Record deleted successfully");
        }

        public async Task<ApiResponse<IReadOnlyList<BranchDailySummaryRowDto>>> GetSummaryReportAsync(BranchDailySummaryFilterDto filter)
        {
            var query = _context.BranchSalesDailies
                .Include(d => d.Branch)
                .Include(d => d.ShortageDetails)
                    .ThenInclude(s => s.ShortageType)
                .AsQueryable();

            // الفلاتر
            query = query.Where(d =>
                d.SalesDate.Date >= filter.FromDate.Date &&
                d.SalesDate.Date <= filter.ToDate.Date
            );

            if (filter.CityIds != null && filter.CityIds.Any())
                query = query.Where(d => filter.CityIds.Contains(d.Branch.CityId));


            if (filter.ActivityTypeId.HasValue)
                query = query.Where(d => d.Branch.ActivityTypeId == filter.ActivityTypeId);

            if (filter.BranchType != "All")
            {
                var type = Enum.Parse<BranchType>(filter.BranchType);
                query = query.Where(d => d.Branch.BranchType == type);
            }

            if (filter.OnlyWithShortage)
                query = query.Where(d => d.Difference < 0);

            var data = await query
                .GroupBy(d => new { d.BranchId, d.Branch.BranchName, d.Branch.BranchNumber })
                .Select(g => new BranchDailySummaryRowDto
                {
                    BranchId = g.Key.BranchId,
                   BranchNumber = g.Key.BranchNumber,
                    BranchName = g.Key.BranchName,

                    CashAmount = g.Sum(x => x.CashAmount ?? 0),
                    NetworkAmount = g.Sum(x => x.NetworkAmount ?? 0),
                    CreditAmount = g.Sum(x => x.CreditAmount ?? 0),

                    TotalSales = g.Sum(x => x.TotalSales ?? 0),
                    GrandTotal = g.Sum(x => x.GrandTotal ?? 0),
                    Difference = g.Sum(x => x.Difference ?? 0),

                    TotalShortageAmount = g
                        .SelectMany(x => x.ShortageDetails)
                        .Sum(s => (decimal?)s.Amount ?? 0),

                    // هنملأ Shortages بعدين في الذاكرة
                    Shortages = new List<BranchShortageSummaryDto>()
                })
                .OrderBy(x => x.BranchName)
                .ToListAsync();

            // 🔹 نجيب كل تفاصيل العجز مرة واحدة
            var allShortages = await query
                .SelectMany(d => d.ShortageDetails)
                .Select(s => new
                {
                    s.BranchSalesDaily.BranchId,
                    s.ShortageTypeId,
                    ShortageTypeName = s.ShortageType.ShortageName,
                    s.Amount
                })
                .ToListAsync();

            // 🔹 نملأ Shortages لكل فرع
            foreach (var row in data)
            {
                var branchShortages = allShortages
                    .Where(s => s.BranchId == row.BranchId)
                    .GroupBy(s => new { s.ShortageTypeId, s.ShortageTypeName })
                    .Select(g => new BranchShortageSummaryDto
                    {
                        ShortageTypeId = g.Key.ShortageTypeId,
                        ShortageTypeName = g.Key.ShortageTypeName,
                        Amount = g.Sum(x => (decimal?)x.Amount ?? 0)
                    })
                    .ToList();

                row.Shortages = branchShortages;
            }
            ////// دة كود جديد عشان نجيب باقى الفروع الى ملهاش يوميه او مدخلوش اليوميه
            // 🔥 نجيب الفروع حسب نفس الفلاتر
            var branchesQuery = _context.Branches.AsQueryable();

            if (filter.CityIds != null && filter.CityIds.Any())
                branchesQuery = branchesQuery.Where(b => filter.CityIds.Contains(b.CityId));

            if (filter.ActivityTypeId.HasValue)
                branchesQuery = branchesQuery.Where(b => b.ActivityTypeId == filter.ActivityTypeId);

            if (filter.BranchType != "All")
            {
                var type = Enum.Parse<BranchType>(filter.BranchType);
                branchesQuery = branchesQuery.Where(b => b.BranchType == type);
            }

            var allBranches = await branchesQuery
                .Select(b => new { b.Id, b.BranchName, b.BranchNumber })
                .ToListAsync();

            // 🔥 IDs الفروع اللي ظهرت في التقرير
            var existingBranchIds = data.Select(x => x.BranchId).ToHashSet();

            // 🔥 نجيب الفروع اللي ماعندهاش يوميات
            var missingBranches = allBranches
                .Where(b => !existingBranchIds.Contains(b.Id))
                .ToList();

            // 🔥 نضيفها للتقرير بقيم صفر
            foreach (var b in missingBranches)
            {
                data.Add(new BranchDailySummaryRowDto
                {
                    BranchId = b.Id,
                    BranchNumber = b.BranchNumber,
                    BranchName = b.BranchName,

                    CashAmount = 0,
                    NetworkAmount = 0,
                    CreditAmount = 0,
                    TotalSales = 0,
                    GrandTotal = 0,
                    Difference = 0,
                    TotalShortageAmount = 0,

                    Shortages = new List<BranchShortageSummaryDto>()
                });
            }

            // 🔥 إعادة ترتيب النتيجة
            data = data.OrderBy(x => x.BranchName).ToList();
            ////////
           
            return ApiResponse<IReadOnlyList<BranchDailySummaryRowDto>>.Ok(data);
        }

        /// <summary>
        /// /تقرير لادارة المرتجعات والخصومات
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<ReturnsDiscountsManagementRowDto>> GetReturnsDiscountsManagementAsync(ReturnsDiscountsManagementFilterDto filter)
        {
            var query = _context.BranchSalesShortageDetails
                .Where(s => s.BranchSalesDaily.SalesDate >= filter.FromDate &&
                            s.BranchSalesDaily.SalesDate <= filter.ToDate);

            // 🔥 Multi City Filter
            if (filter.CityIds != null && filter.CityIds.Any())
            {
                query = query.Where(s => filter.CityIds.Contains(s.BranchSalesDaily.Branch.CityId));
            }

            // 🔥 Multi Branch Filter
            if (filter.BranchIds != null && filter.BranchIds.Any())
            {
                query = query.Where(s => filter.BranchIds.Contains(s.BranchSalesDaily.BranchId));
            }

            // 🔥 Shortage Type Filter
            query = query.Where(s =>
                !filter.ShortageTypeId.HasValue ||
                s.ShortageTypeId == filter.ShortageTypeId.Value
            );

            // 🔥 Status Filter (كما هو بدون تغيير)
            if (filter.Status == ReturnsDiscountsApprovalStatus.Approved)
            {
                query = query.Where(s =>
                    (
                        (s.ShortageTypeId == 3 || s.ShortageTypeId == 6) &&
                        s.IsReturnApproved == true
                    )
                    ||
                    (
                        (s.ShortageTypeId != 3 && s.ShortageTypeId != 6) &&
                        s.IsDiscountApproved == true
                    )
                );
            }
            else if (filter.Status == ReturnsDiscountsApprovalStatus.NotApproved)
            {
                query = query.Where(s =>
                    (
                        (s.ShortageTypeId == 3 || s.ShortageTypeId == 6) &&
                        (!s.IsReturnApproved.HasValue || s.IsReturnApproved == false)
                    )
                    ||
                    (
                        (s.ShortageTypeId != 3 && s.ShortageTypeId != 6) &&
                        (!s.IsDiscountApproved.HasValue || s.IsDiscountApproved == false)
                    )
                );
            }

            var result = await query
                .Select(s => new ReturnsDiscountsManagementRowDto
                {
                    JournalDate = s.BranchSalesDaily.SalesDate,
                    BranchId = s.BranchSalesDaily.BranchId,
                    BranchName = s.BranchSalesDaily.Branch.BranchName,

                    ShortageTypeId = s.ShortageTypeId,
                    ShortageTypeName = s.ShortageType.ShortageName,

                    Amount = s.Amount ?? 0
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<BranchDailySummaryRowDto>> GetBranchDailySummaryAsync(BranchDailySummaryFilterDto filter)
        {
            var query = _context.BranchSalesDailies
                .Include(d => d.Branch)
                .Include(d => d.ShortageDetails)
                    .ThenInclude(s => s.ShortageType)
                .AsQueryable();

            // الفلاتر
            query = query.Where(d =>
                d.SalesDate.Date >= filter.FromDate.Date &&
                d.SalesDate.Date <= filter.ToDate.Date
            );
            if (filter.CityIds != null && filter.CityIds.Any())
                query = query.Where(d => filter.CityIds.Contains(d.Branch.CityId));


            if (filter.ActivityTypeId.HasValue)
                query = query.Where(d => d.Branch.ActivityTypeId == filter.ActivityTypeId);

            if (filter.BranchType != "All")
            {
                var type = Enum.Parse<BranchType>(filter.BranchType);
                query = query.Where(d => d.Branch.BranchType == type);
            }

            if (filter.OnlyWithShortage)
                query = query.Where(d => d.Difference < 0);

            var data = await query
                .GroupBy(d => new { d.BranchId, d.Branch.BranchName })
                .Select(g => new BranchDailySummaryRowDto
                {
                    BranchId = g.Key.BranchId,
                    BranchName = g.Key.BranchName,

                    CashAmount = g.Sum(x => x.CashAmount ?? 0),
                    NetworkAmount = g.Sum(x => x.NetworkAmount ?? 0),
                    CreditAmount = g.Sum(x => x.CreditAmount ?? 0),

                    TotalSales = g.Sum(x => x.TotalSales ?? 0),
                    GrandTotal = g.Sum(x => x.GrandTotal ?? 0),
                    Difference = g.Sum(x => x.Difference ?? 0),

                    TotalShortageAmount = g
                        .SelectMany(x => x.ShortageDetails)
                        .Sum(s => (decimal?)s.Amount ?? 0),

                    Shortages = new List<BranchShortageSummaryDto>()
                })
                .OrderBy(x => x.BranchName)
                .ToListAsync();

            // نجيب كل تفاصيل العجز مرة واحدة
            var allShortages = await query
                .SelectMany(d => d.ShortageDetails)
                .Select(s => new
                {
                    s.BranchSalesDaily.BranchId,
                    s.ShortageTypeId,
                    ShortageTypeName = s.ShortageType.ShortageName,
                    s.Amount
                })
                .ToListAsync();

            // نملأ Shortages لكل فرع
            foreach (var row in data)
            {
                var branchShortages = allShortages
                    .Where(s => s.BranchId == row.BranchId)
                    .GroupBy(s => new { s.ShortageTypeId, s.ShortageTypeName })
                    .Select(g => new BranchShortageSummaryDto
                    {
                        ShortageTypeId = g.Key.ShortageTypeId,
                        ShortageTypeName = g.Key.ShortageTypeName,
                        Amount = g.Sum(x => (decimal?)x.Amount ?? 0)
                    })
                    .ToList();

                row.Shortages = branchShortages;
            }

            return data;
        }

        public async Task<ApiResponse<bool>> UpdateShortagesApprovalsAsync(
        List<ShortageApprovalUpdateDto> items)
        {
            if (items == null || !items.Any())
                return ApiResponse<bool>.Fail("لا توجد بيانات للتحديث");

            // نجيب الـ Ids اللي جايه من الفرونت
            var ids = items.Select(x => x.Id).ToList();

            // نجيب كل التفاصيل من الداتا بيز مرة واحدة
            var shortages = await _context.BranchSalesShortageDetails
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            if (!shortages.Any())
                return ApiResponse<bool>.Fail("لم يتم العثور على أي سجلات مطابقة");

            // نعمل ماب من DTO للـ Entity
            foreach (var shortage in shortages)
            {
                var dto = items.First(x => x.Id == shortage.Id);

                // لو نوع العجز مرتجعات (ShortageTypeId == 3 مثلاً)
                // أو ممكن تسيبها مفتوحة وتخلي الفرونت يتحكم
                if (dto.IsReturnApproved.HasValue)
                    shortage.IsReturnApproved = dto.IsReturnApproved;

                if (dto.IsDiscountApproved.HasValue)
                    shortage.IsDiscountApproved = dto.IsDiscountApproved;

                if (!string.IsNullOrWhiteSpace(dto.ReturnNotes))
                    shortage.ReturnNotes = dto.ReturnNotes;

                if (!string.IsNullOrWhiteSpace(dto.DiscountNotes))
                    shortage.DiscountNotes = dto.DiscountNotes;
            }

            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true, "تم تحديث الاعتمادات بنجاح");
        }

        public async Task<List<BranchNetworkShortageReportRowDto>> GetBranchNetworkShortagesAsync(BranchNetworkShortageFilterDto filter)
        {
            var query = _context.BranchSalesDailies
                .Include(d => d.Branch)
                .Include(d => d.ShortageDetails)
                    .ThenInclude(s => s.ShortageType)
                .AsQueryable();

            // فلترة التاريخ
            query = query.Where(d =>
                d.SalesDate.Date >= filter.FromDate.Date &&
                d.SalesDate.Date <= filter.ToDate.Date
            );

            // فلترة المدينة
            if (filter.CityId.HasValue)
                query = query.Where(d => d.Branch.CityId == filter.CityId.Value);

            // نجيب كل الفروع في المدينة
            var branches = await _context.Branches
                .Where(b => !filter.CityId.HasValue || b.CityId == filter.CityId.Value)
                .ToListAsync();

            // نجيب كل أنواع العجز ماعدا مرتجعات واستبدال
            var shortageTypes = await _context.ShortageTypes
                .Where(t => t.ShortageName != "مرتجعات" && t.ShortageName != "استبدال")
                .ToListAsync();

            var result = new List<BranchNetworkShortageReportRowDto>();

            foreach (var branch in branches)
            {
                var daily = await query
                    .Where(d => d.BranchId == branch.Id)
                    .ToListAsync();

                var allShortages = daily
                    .SelectMany(d => d.ShortageDetails)
                    .Where(s => s.ShortageType.ShortageName != "مرتجعات" &&
                                s.ShortageType.ShortageName != "استبدال")
                    .ToList();

                var row = new BranchNetworkShortageReportRowDto
                {
                    BranchId = branch.Id,
                    BranchName = branch.BranchName,
                    NetworkAmount = daily.Sum(d => d.NetworkAmount ?? 0),
                    Shortages = shortageTypes.Select(t => new BranchShortageSummaryDto
                    {
                        ShortageTypeId = t.Id,
                        ShortageTypeName = t.ShortageName,
                        Amount = allShortages
                            .Where(s => s.ShortageTypeId == t.Id)
                            .Sum(s => s.Amount ?? 0)
                    }).ToList()
                };

                result.Add(row);
            }

            return result;
        }


        public async Task<List<BranchSalesDailyListRowDto>> SearchAsync(BranchSalesDailySearchFilterDto filter)
        {
            var query = _context.BranchSalesDailies
                .Include(d => d.Branch)
                .AsQueryable();

            // فلترة التاريخ
            query = query.Where(d =>
                d.SalesDate.Date >= filter.FromDate.Date &&
                d.SalesDate.Date <= filter.ToDate.Date
            );

            // فلترة بالفرع ID
            if (filter.BranchId.HasValue)
                query = query.Where(d => d.BranchId == filter.BranchId.Value);

            // فلترة برقم الفرع
            if (filter.BranchNumber.HasValue)
                query = query.Where(d => d.Branch.BranchNumber == filter.BranchNumber.Value);

            var data = await query
                .OrderByDescending(d => d.SalesDate)
                .Select(d => new BranchSalesDailyListRowDto
                {
                    Id = d.Id,
                    BranchId = d.BranchId,
                    BranchName = d.Branch.BranchName,
                    SalesDate = d.SalesDate,

                    CashAmount = d.CashAmount,
                    NetworkAmount = d.NetworkAmount,
                    CreditAmount = d.CreditAmount,

                    TotalSales = d.TotalSales,
                    GrandTotal = d.GrandTotal,
                    Difference = d.Difference
                })
                .ToListAsync();

            return data;
        }


    }
}
