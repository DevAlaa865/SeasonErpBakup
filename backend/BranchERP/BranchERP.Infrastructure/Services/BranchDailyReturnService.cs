using AutoMapper;
using BranchERP.Application.DTOs.BranchDailyReturnDto;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Application.Services
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
        // 1) IMPORT EXCEL
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

            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (DateTime.TryParse(row[0]?.ToString(), out var date))
                    datesInFile.Add(date.Date);
            }

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

                if (!int.TryParse(row[4]?.ToString(), out var typeInt))
                    continue;

                var notes = row[5]?.ToString();

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
        // 2) GET RETURNS (FINAL FIXED VERSION)
        // ============================================================
        public async Task<List<BranchDailyReturnDto>> GetReturnsAsync(
            DateTime? fromDate,
            DateTime? toDate,
            List<int>? cityIds,
            List<int>? branchIds,
            int? returnType
        )
        {
            var query = _context.BranchDailyReturns
                .Include(r => r.Branch)
                .AsQueryable();

            // Date filters
            if (fromDate.HasValue)
                query = query.Where(r => r.ReturnDate.Date >= fromDate.Value.Date);

            if (toDate.HasValue)
                query = query.Where(r => r.ReturnDate.Date <= toDate.Value.Date);

            // Multi city filter
            if (cityIds != null && cityIds.Any())
                query = query.Where(r => cityIds.Contains(r.Branch.CityId));

            // Multi branch filter
            if (branchIds != null && branchIds.Any())
                query = query.Where(r => branchIds.Contains(r.BranchId));

            // Return type
            if (returnType.HasValue && returnType.Value > 0)
                query = query.Where(r => (int)r.ReturnType == returnType.Value);

            var data = await query
                .OrderByDescending(r => r.ReturnDate)
                .ThenBy(r => r.Branch.BranchNumber)
                .ToListAsync();

            return _mapper.Map<List<BranchDailyReturnDto>>(data);
        }

        // ============================================================
        // 3) GET BY ID
        // ============================================================
        public async Task<BranchDailyReturnDto?> GetByIdAsync(int id)
        {
            var entity = await _context.BranchDailyReturns
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(r => r.Id == id);

            return entity == null ? null : _mapper.Map<BranchDailyReturnDto>(entity);
        }

        // ============================================================
        // 4) UPDATE
        // ============================================================
        public async Task<bool> UpdateAsync(int id, BranchDailyReturnUpdateDto dto, string userName)
        {
            var entity = await _context.BranchDailyReturns
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
                return false;

            if (dto.ReturnType < 1 || dto.ReturnType > 7)
                throw new Exception("Invalid return type");

            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchNumber == dto.BranchNumber);

            if (branch == null)
                throw new Exception("Branch not found");

            entity.BranchId = branch.Id;
            entity.ReturnDate = dto.ReturnDate;
            entity.ReturnAmount = dto.ReturnAmount;
            entity.ReturnType = (BranchReturnType)dto.ReturnType;
            entity.Notes = dto.Notes;

            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userName;

            await _context.SaveChangesAsync();
            return true;
        }

        // ============================================================
        // 5) EXPORT
        // ============================================================
        public async Task<byte[]> ExportToExcelAsync(
            DateTime? fromDate,
            DateTime? toDate,
            List<int>? cityIds,
            List<int>? branchIds,
            int? returnType
        )
        {
            var data = await GetReturnsAsync(
                fromDate,
                toDate,
                cityIds,
                branchIds,
                returnType
            );

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("DailyReturns");

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
                    5 => "تحويل بنكى",
                    6 => "ادخال خطأ",
                    7 => "اخرى",
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

        // ============================================================
        // 6) CHART
        // ============================================================
        public async Task<List<BranchDailyReturnChartDto>> GetChartDataAsync(
            DateTime? fromDate,
            DateTime? toDate,
            List<int>? cityIds,
            List<int>? branchIds,
            int? returnType
        )
        {
            var data = await GetReturnsAsync(
                fromDate,
                toDate,
                cityIds,
                branchIds,
                returnType
            );

            return data
                .GroupBy(x => x.BranchName)
                .Select(g => new BranchDailyReturnChartDto
                {
                    BranchName = g.Key,
                    Cash = g.Where(x => x.ReturnType == 1).Sum(x => x.ReturnAmount),
                    Replacement = g.Where(x => x.ReturnType == 2).Sum(x => x.ReturnAmount),
                    Tabby = g.Where(x => x.ReturnType == 3).Sum(x => x.ReturnAmount),
                    Tamara = g.Where(x => x.ReturnType == 4).Sum(x => x.ReturnAmount)
                })
                .ToList();
        }
    }
}