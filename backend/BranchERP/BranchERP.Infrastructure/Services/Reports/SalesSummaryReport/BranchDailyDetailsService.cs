using AutoMapper;
using BranchERP.Application.DTOs.Reports.SalesSummaryReport;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.Reports.SalesSummaryReports;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.Reports.SalesSummaryReport
{
    public class BranchDailyDetailsService : IBranchDailyDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchDailyDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BranchDailyDetailReportResponse> GetBranchDailyDetailsAsync(
            int branchId, DateTime fromDate, DateTime toDate)
        {
            var salesRepo = _unitOfWork.Repository<BranchSalesDaily>();
            var returnsRepo = _unitOfWork.Repository<BranchDailyReturn>();

            // فلترة التاريخ
            var from = fromDate.Date;
            var to = toDate.Date.AddDays(1).AddTicks(-1);

            // ================================
            // 🔥 جلب بيانات المبيعات اليومية
            // ================================
            var salesList = await salesRepo.Query()
                .Where(s => s.BranchId == branchId &&
                            s.SalesDate >= from &&
                            s.SalesDate <= to)
                .OrderBy(s => s.SalesDate)
                .ToListAsync();

            // ================================
            // 🔥 جلب المرتجعات اليومية
            // ================================
            var returnsList = await returnsRepo.Query()
                .Where(r => r.BranchId == branchId &&
                            r.ReturnDate >= from &&
                            r.ReturnDate <= to)
                .ToListAsync();

            var response = new BranchDailyDetailReportResponse();

            // ================================
            // 🔥 بناء الصفوف اليومية
            // ================================
            foreach (var day in salesList)
            {
                var dayReturns = returnsList
                    .Where(r => r.ReturnDate.Date == day.SalesDate.Date)
                    .Sum(r => r.ReturnAmount);

                var dto = new BranchDailyDetailDto
                {
                    SalesDate = day.SalesDate,
                    TotalSales = day.GrandTotal ?? 0m,
                    TotalReturns = dayReturns,
                    NetSales = (day.GrandTotal ?? 0m) - dayReturns,
                    InvoiceCount = day.TotalInvoicesCount ?? 0,
                    QuantityCount = day.TotalQuantities ?? 0,
                    AvgInvoice = (day.TotalInvoicesCount ?? 0) == 0
                        ? 0
                        : ((day.GrandTotal ?? 0m) - dayReturns) / (day.TotalInvoicesCount ?? 0),
                    AvgPieces = (day.TotalInvoicesCount ?? 0) == 0
                        ? 0
                        : (decimal)(day.TotalQuantities ?? 0) / (day.TotalInvoicesCount ?? 0)
                };

                response.Items.Add(dto);
            }

            // ================================
            // 🔥 حساب الإجماليات
            // ================================
            response.TotalSales = response.Items.Sum(x => x.TotalSales);
            response.TotalReturns = response.Items.Sum(x => x.TotalReturns);
            response.NetSales = response.Items.Sum(x => x.NetSales);
            response.InvoiceCount = response.Items.Sum(x => x.InvoiceCount);
            response.QuantityCount = response.Items.Sum(x => x.QuantityCount);

            response.AvgInvoice = response.InvoiceCount == 0
                ? 0
                : response.NetSales / response.InvoiceCount;

            response.AvgPieces = response.InvoiceCount == 0
                ? 0
                : (decimal)response.QuantityCount / response.InvoiceCount;

            return response;
        }
    }
}
