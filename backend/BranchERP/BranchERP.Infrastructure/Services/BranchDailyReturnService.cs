using Application.Interfaces;
using AutoMapper;
using BranchERP.Application.DTOs.BranchDailyReturnDto;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Infrastructure.Services
{
    public class BranchDailyReturnService : IBranchDailyReturnService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BranchDailyReturnService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ============================================================
        // 1) Import Excel
        // ============================================================
        public async Task ImportFromExcelAsync(Stream fileStream, string fileName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var reader = ExcelReaderFactory.CreateReader(fileStream);
            var result = reader.AsDataSet();
            var table = result.Tables[0];

            var branches = await _context.Branches
                .Select(b => new { b.Id, b.BranchNumber })
                .ToListAsync();

            var datesInFile = new HashSet<DateTime>();

            // جمع التواريخ
            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (DateTime.TryParse(row[0]?.ToString(), out var date))
                    datesInFile.Add(date.Date);
            }

            // حذف القديم
            var oldReturns = await _context.BranchDailyReturns
                .Where(r => datesInFile.Contains(r.ReturnDate.Date))
                .ToListAsync();

            _context.BranchDailyReturns.RemoveRange(oldReturns);

            var newReturns = new List<BranchDailyReturn>();

            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];

                if (!DateTime.TryParse(row[0]?.ToString(), out var returnDate))
                    continue;

                if (!int.TryParse(row[1]?.ToString(), out var branchNumber))
                    continue;

                if (!decimal.TryParse(row[2]?.ToString(), out var amount))
                    continue;

                if (!int.TryParse(row[3]?.ToString(), out var typeInt))
                    continue;

                var notes = row[4]?.ToString();

                var branch = branches.FirstOrDefault(b => b.BranchNumber == branchNumber);
                if (branch == null)
                    continue;

                newReturns.Add(new BranchDailyReturn
                {
                    ReturnDate = returnDate.Date,
                    BranchId = branch.Id,
                    ReturnAmount = amount,
                    ReturnType = (BranchReturnType)typeInt,
                    Notes = notes,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.BranchDailyReturns.AddRangeAsync(newReturns);
            await _context.SaveChangesAsync();
        }

        // ============================================================
        // 2) Get Returns with Filters
        // ============================================================
        public async Task<List<BranchDailyReturnDto>> GetReturnsAsync(
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            int? branchNumber,
            int? cityId,
            int? returnType
        )
        {
            var query = _context.BranchDailyReturns
                .Include(r => r.Branch)
                .AsQueryable();

            // التاريخ من
            if (fromDate.HasValue)
                query = query.Where(r => r.ReturnDate.Date >= fromDate.Value.Date);

            // التاريخ إلى
            if (toDate.HasValue)
                query = query.Where(r => r.ReturnDate.Date <= toDate.Value.Date);

            // الفلترة بالفرع ID
            if (branchId.HasValue)
                query = query.Where(r => r.BranchId == branchId.Value);

            // الفلترة برقم الفرع
            if (branchNumber.HasValue)
                query = query.Where(r => r.Branch.BranchNumber == branchNumber.Value);

            // 🔥 الفلترة بالمدينة CityId
            if (cityId.HasValue)
                query = query.Where(r => r.Branch.CityId == cityId.Value);

            // الفلترة بنوع المرتجع
            if (returnType.HasValue && returnType.Value > 0)
                query = query.Where(r => (int)r.ReturnType == returnType.Value);

            var data = await query
                .OrderByDescending(r => r.ReturnDate)
                .ThenBy(r => r.Branch.BranchNumber)
                .ToListAsync();

            return _mapper.Map<List<BranchDailyReturnDto>>(data);
        }

        // ============================================================
        // 3) Get By ID
        // ============================================================
        public async Task<BranchDailyReturnDto?> GetByIdAsync(int id)
        {
            var entity = await _context.BranchDailyReturns
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
                return null;

            return _mapper.Map<BranchDailyReturnDto>(entity);
        }

        // ============================================================
        // 4) Update
        // ============================================================
        public async Task<bool> UpdateAsync(int id, BranchDailyReturnUpdateDto dto, string userName)
        {
            var entity = await _context.BranchDailyReturns
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
                return false;

            if (dto.ReturnType != 1 && dto.ReturnType != 2)
                throw new Exception("نوع المرتجع يجب أن يكون 1 (كاش) أو 2 (استبدال)");

            // تحديث رقم الفرع
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchNumber == dto.BranchNumber);

            if (branch == null)
                throw new Exception("رقم الفرع غير موجود");

            entity.BranchId = branch.Id;

            // تحديث التاريخ
            entity.ReturnDate = dto.ReturnDate;

            // تحديث باقي الحقول
            entity.ReturnAmount = dto.ReturnAmount;
            entity.ReturnType = (BranchReturnType)dto.ReturnType;
            entity.Notes = dto.Notes;

            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userName;

            await _context.SaveChangesAsync();
            return true;
        }

          public async Task<byte[]> ExportToExcelAsync(
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            int? branchNumber,
            int? cityId,
            int? returnType
        )
        {
            var data = await GetReturnsAsync(
                fromDate,
                toDate,
                branchId,
                branchNumber,
                cityId,
                returnType
            );

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("DailyReturns");

            // العناوين
            ws.Cell(1, 1).Value = "التاريخ";
            ws.Cell(1, 2).Value = "رقم الفرع";
            ws.Cell(1, 3).Value = "اسم الفرع";
            ws.Cell(1, 4).Value = "المبلغ";
            ws.Cell(1, 5).Value = "النوع";
            ws.Cell(1, 6).Value = "ملاحظات";

            int row = 2;

            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = item.ReturnDate.ToString("yyyy-MM-dd");
                ws.Cell(row, 2).Value = item.BranchNumber;
                ws.Cell(row, 3).Value = item.BranchName;
                ws.Cell(row, 4).Value = item.ReturnAmount;
                ws.Cell(row, 5).Value = item.ReturnType switch
                {
                    1 => "كاش",
                    2 => "استبدال",
                    3 => "تابى",
                    4 => "تمارا",
                    _ => "-"
                };

                ws.Cell(row, 6).Value = item.Notes;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

          public async Task<List<BranchDailyReturnChartDto>> GetChartDataAsync(
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            int? branchNumber,
            int? cityId,
            int? returnType
)
        {
            var data = await GetReturnsAsync(
                fromDate,
                toDate,
                branchId,
                branchNumber,
                cityId,
                returnType
            );

            var grouped = data
                .GroupBy(r => r.BranchName)
                .Select(g => new BranchDailyReturnChartDto
                {
                    BranchName = g.Key,
                    Cash = g.Where(x => x.ReturnType == 1).Sum(x => x.ReturnAmount),
                    Replacement = g.Where(x => x.ReturnType == 2).Sum(x => x.ReturnAmount),
                    Tabby = g.Where(x => x.ReturnType == 3).Sum(x => x.ReturnAmount),
                    Tamara = g.Where(x => x.ReturnType == 4).Sum(x => x.ReturnAmount)
                })
                .ToList();

            return grouped;
        }


    }
}
