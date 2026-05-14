using AutoMapper;
using BranchERP.Application.DTOs.BranchDailyTarget;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Enums;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class BranchDailyTargetService : IBranchDailyTargetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchDailyTargetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<BranchDailyTargetHeaderDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(h => h.Branch)
                    .Include(h => h.Details)
                        .ThenInclude(d => d.Employee)
            );

            if (entity == null)
                return ApiResponse<BranchDailyTargetHeaderDto>.Fail("Target not found");

            var dto = _mapper.Map<BranchDailyTargetHeaderDto>(entity);
            return ApiResponse<BranchDailyTargetHeaderDto>.Ok(dto);
        }

        public async Task<ApiResponse<IReadOnlyList<BranchDailyTargetHeaderDto>>> GetByBranchAndDateAsync(
            int branchId,
            DateTime date)
        {
            var repo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            var items = await repo.GetAllAsync(
                filter: x => x.BranchId == branchId && x.TargetDate.Date == date.Date,
                include: q => q
                    .Include(h => h.Branch)
                    .Include(h => h.Details)
                        .ThenInclude(d => d.Employee)
            );

            var data = _mapper.Map<IReadOnlyList<BranchDailyTargetHeaderDto>>(items);
            return ApiResponse<IReadOnlyList<BranchDailyTargetHeaderDto>>.Ok(data);
        }

        public async Task<ApiResponse<BranchDailyTargetHeaderDto>> CreateAsync(BranchDailyTargetHeaderCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            var entity = _mapper.Map<BranchDailyTargetHeader>(model);

            entity.Details.Clear();
            foreach (var d in model.Details)
                entity.Details.Add(_mapper.Map<BranchDailyTargetDetail>(d));

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<ApiResponse<BranchDailyTargetHeaderDto>> UpdateAsync(int id, BranchDailyTargetHeaderCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q.Include(h => h.Details)
            );

            if (entity == null)
                return ApiResponse<BranchDailyTargetHeaderDto>.Fail("Target not found");

            entity.BranchId = model.BranchId;
            entity.TargetDate = model.TargetDate;
            entity.TotalBranchTarget = model.TotalBranchTarget;
            entity.TotalAchieved = model.TotalAchieved;

            entity.Details.Clear();
            foreach (var d in model.Details)
                entity.Details.Add(_mapper.Map<BranchDailyTargetDetail>(d));

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.Fail("Target not found");

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true, "Target deleted successfully");
        }

        public async Task<ApiResponse<int>> ImportFromExcelAsync(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream);

            var headerSheet = workbook.Worksheet("Header");
            var detailsSheet = workbook.Worksheet("Details");

            // ================================
            // 🔥 0) مسح كل البيانات القديمة قبل إدخال الجديد
            // ================================
            var headerRepo = _unitOfWork.Repository<BranchDailyTargetHeader>();
            var detailRepo = _unitOfWork.Repository<BranchDailyTargetDetail>();

            await detailRepo.DeleteAllAsync();
            await headerRepo.DeleteAllAsync();
            await _unitOfWork.CompleteAsync();

            // ================================
            // 1) قراءة كل صفوف الهيدر (أكتر من فرع)
            // ================================
            var headerRows = new List<(int BranchNumber, DateTime TargetDate, decimal TotalTarget, decimal TotalAchieved)>();

            int hRow = 2;
            while (!headerSheet.Row(hRow).IsEmpty())
            {
                headerRows.Add((
                    BranchNumber: headerSheet.Cell(hRow, 1).GetValue<int>(),
                    TargetDate: headerSheet.Cell(hRow, 2).GetValue<DateTime>(),
                    TotalTarget: headerSheet.Cell(hRow, 3).GetValue<decimal>(),
                    TotalAchieved: headerSheet.Cell(hRow, 4).GetValue<decimal>()
                ));

                hRow++;
            }

            // ================================
            // 2) قراءة كل صفوف التفاصيل (أكتر من موظف)
            // ================================
            var detailRows = new List<(int BranchNumber, string EmployeeCode, int Shift, decimal Target, decimal Achieved)>();

            int dRow = 2;
            while (!detailsSheet.Row(dRow).IsEmpty())
            {
                detailRows.Add((
                    BranchNumber: detailsSheet.Cell(dRow, 1).GetValue<int>(),
                    EmployeeCode: detailsSheet.Cell(dRow, 2).GetValue<string>(),
                    Shift: detailsSheet.Cell(dRow, 3).GetValue<int>(),
                    Target: detailsSheet.Cell(dRow, 4).GetValue<decimal>(),
                    Achieved: detailsSheet.Cell(dRow, 5).GetValue<decimal>()
                ));

                dRow++;
            }

            // ================================
            // 3) نبدأ نعمل Create لكل فرع
            // ================================
            var branchRepo = _unitOfWork.Repository<Branch>();
            var employeeRepo = _unitOfWork.Repository<Employee>();

            int createdCount = 0;

            foreach (var header in headerRows)
            {
                // جلب الفرع
                var branch = await branchRepo.GetAsync(b => b.BranchNumber == header.BranchNumber);

                if (branch == null)
                    return ApiResponse<int>.Fail($"BranchNumber {header.BranchNumber} غير موجود في قاعدة البيانات");

                int branchId = branch.Id;

                // إنشاء هيدر جديد (لأننا مسحنا القديم كله)
                var headerEntity = new BranchDailyTargetHeader
                {
                    BranchId = branchId,
                    TargetDate = header.TargetDate,
                    TotalBranchTarget = header.TotalTarget,
                    TotalAchieved = header.TotalAchieved,
                    Details = new List<BranchDailyTargetDetail>()
                };

                await headerRepo.AddAsync(headerEntity);

                // ================================
                // 4) ربط الموظفين بالفرع الصحيح
                // ================================
                var branchEmployees = detailRows
                    .Where(d => d.BranchNumber == header.BranchNumber)
                    .ToList();

                foreach (var d in branchEmployees)
                {
                    var employee = await employeeRepo.GetAsync(e => e.EmployeeCode == d.EmployeeCode);

                    if (employee == null)
                        throw new Exception($"EmployeeCode {d.EmployeeCode} غير موجود في قاعدة البيانات");

                    if (!Enum.IsDefined(typeof(WorkShift), d.Shift))
                        throw new Exception($"Shift value {d.Shift} غير صحيح. يجب أن يكون بين 1 و 6.");

                    headerEntity.Details.Add(new BranchDailyTargetDetail
                    {
                        EmployeeId = employee.Id,
                        Shift = (WorkShift)d.Shift,
                        EmployeeTarget = d.Target,
                        EmployeeAchieved = d.Achieved
                    });
                }

                createdCount++;
            }

            // ================================
            // 5) حفظ كل شيء
            // ================================
            await _unitOfWork.CompleteAsync();

            return ApiResponse<int>.Ok(createdCount, "تم استيراد التارجت بنجاح");
        }


        public async Task<ApiResponse<IReadOnlyList<EmployeeDailyTargetReportDto>>> GetEmployeeReportAsync(
           int? cityId,
           int? branchId,
           DateTime date)
        {
            var headerRepo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            var query = headerRepo.Query()
                .Include(h => h.Branch)
                    .ThenInclude(b => b.City)
                .Include(h => h.Details)
                    .ThenInclude(d => d.Employee)
                .Where(h => h.TargetDate.Date == date.Date);

            if (cityId.HasValue)
                query = query.Where(h => h.Branch.CityId == cityId.Value);

            if (branchId.HasValue)
                query = query.Where(h => h.BranchId == branchId.Value);

            var headers = await query.ToListAsync();

            var result = new List<EmployeeDailyTargetReportDto>();

            foreach (var h in headers)
            {
                foreach (var d in h.Details)
                {
                    var percentage = d.EmployeeTarget > 0
                        ? Math.Round((d.EmployeeAchieved ?? 0) / (d.EmployeeTarget ?? 1) * 100, 2)
                        : 0;

                    // 🔥 هنا بالظبط تحط الكود اللي سألت عنه
                    result.Add(new EmployeeDailyTargetReportDto
                    {
                        HeaderId = h.Id,
                        DetailId = d.Id,

                        EmployeeId = d.EmployeeId,
                        EmployeeName = d.Employee?.FullName ?? "",

                        BranchId = h.BranchId,
                        BranchName = h.Branch?.BranchName ?? "",

                        CityId = h.Branch?.CityId ?? 0,
                        CityName = h.Branch?.City?.CityName ?? "",

                        TargetDate = h.TargetDate,

                        EmployeeTarget = d.EmployeeTarget,
                        EmployeeAchieved = d.EmployeeAchieved,
                        AchievementPercentage = percentage
                    });
                }
            }

            return ApiResponse<IReadOnlyList<EmployeeDailyTargetReportDto>>.Ok(result);
        }




        public async Task<BranchTargetPeriodReportDto> GetBranchTargetPeriodReportAsync(
         int branchId,
         DateTime fromDate,
         DateTime toDate)
        {
            var headerRepo = _unitOfWork.Repository<BranchDailyTargetHeader>();

            // نجيب الصفوف خلال الفترة
            var rowsQuery = headerRepo.Query()
                .Where(h =>
                    h.BranchId == branchId &&
                    h.TargetDate >= fromDate &&
                    h.TargetDate <= toDate)
                .OrderBy(h => h.TargetDate);

            var rows = await rowsQuery
             .Select(h => new BranchTargetPeriodRowDto
             {
                 TargetDate = h.TargetDate,
                 TotalTarget = h.TotalBranchTarget ?? 0,
                 TotalAchieved = h.TotalAchieved ?? 0,
                 AchievementPercentage = (h.TotalBranchTarget ?? 0) == 0
                     ? 0
                     : Math.Round(((h.TotalAchieved ?? 0) / (h.TotalBranchTarget ?? 0)) * 100, 2)
             })
             .ToListAsync();


            // نجيب بيانات الفرع مرة واحدة فقط
            var branchInfo = await headerRepo.Query()
                .Where(h => h.BranchId == branchId)
                .Select(h => new
                {
                    BranchName = h.Branch.BranchName,
                    CityName = h.Branch.City.CityName,
                    SupervisorName = h.Branch.Supervisor!.FullName
                })
                .FirstOrDefaultAsync();

            return new BranchTargetPeriodReportDto
            {
                CityName = branchInfo?.CityName!,
                BranchName = branchInfo?.BranchName!,
                SupervisorName = branchInfo?.SupervisorName!,
                Rows = rows
            };
        }


    }
}
