using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
public class CashBoxTransaction : BaseEntity
{
    public int CashBoxId { get; set; }
    public CashBox CashBox { get; set; }

    public decimal Amount { get; set; }

    public TransactionDirection Direction { get; set; }  // IN / OUT

    public TransactionType TransactionType { get; set; }  // 🔥 دي اللي ناقصاك

    public DateTime TransactionDate { get; set; }     // 🔥 ده هيبقى SalesDate
    public DateTime CreatedAt { get; set; }           // 🔥 ده تاريخ الترحيل الفعلي
    public string? Description { get; set; }
    public string? ReferenceNumber { get; set; }

    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public int? PettyHolderId { get; set; }
    public PettyHolder? PettyHolder { get; set; }

    public int? ExpenseVoucherId { get; set; }
    public ExpenseVoucher? ExpenseVoucher { get; set; }
}}
