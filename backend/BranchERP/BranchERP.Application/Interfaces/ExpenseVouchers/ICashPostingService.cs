using BranchERP.Application.DTOs.ExpenseVouchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface ICashPostingService
    {
        Task<CashPostingResultDto> PostDailyCashForUserAsync(string userId, DateTime date);

        // 🔥 الترحيل اليدوي
        Task<ManualPostingResultDto> ManualPostAsync(ManualPostingRequestDto dto);


        // 🔥 شاشة التاريخ
        Task<List<PostingHistoryDto>> GetPostingHistoryAsync(DateTime date);

        Task<PostingDetailsDto?> GetPostingDetailsAsync(int id);
    }
}
