
using BranchERP.Domain.Entities.Enums;
using BranchERP.Domain.Enums;

namespace BranchERP.Domain.Entities
{
    public class BankTransferRequest : BaseEntity
    {
        // رقم الطلب
        public string RequestNumber { get; set; } = string.Empty;

        // تاريخ تقديم الطلب
        public DateTime RequestDate { get; set; }

        // الفرع
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        // بيانات الفاتورة
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal InvoiceAmount { get; set; }

        // نوع التحويل
        public TransferType TransferType { get; set; }

        // المبلغ المطلوب تحويله
        public decimal TransferAmount { get; set; }

        // بيانات العميل
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobile { get; set; } = string.Empty;

        // بيانات البنك
        public string BankName { get; set; } = string.Empty;
        public string Iban { get; set; } = string.Empty;

        // الحالة
        public TransferRequestStatus Status { get; set; }

        // تاريخ تنفيذ التحويل
        public DateTime? TransferDate { get; set; }

        // مقدم الطلب
        public string CreatedBy { get; set; } = string.Empty;

        // منفذ التحويل
        public string? ProcessedBy { get; set; }

        // توقيع مقدم الطلب
        public string ApplicantSignature { get; set; } = string.Empty;

        // ملاحظات
        public string? Notes { get; set; }
    }
}