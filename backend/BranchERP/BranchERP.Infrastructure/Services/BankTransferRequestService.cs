using AutoMapper;
using BranchERP.Application.DTOs.BankTransferRequests;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Domain.Enums;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class BankTransferRequestService : IBankTransferRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankTransferRequestService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private async Task<string> GenerateRequestNumberAsync()
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var count = (await repo.GetAllAsync()).Count + 1;

            return $"BT-{count:D6}";
        }

        public async Task<ApiResponse<BankTransferRequestDto>> CreateAsync(
            CreateBankTransferRequestDto dto,
            string createdBy)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var entity = _mapper.Map<BankTransferRequest>(dto);

            entity.RequestNumber = await GenerateRequestNumberAsync();

            entity.RequestDate = DateTime.Now;

            entity.Status = TransferRequestStatus.Pending;

            entity.CreatedBy = createdBy;

            await repo.AddAsync(entity);

            await _unitOfWork.CompleteAsync();

            entity = await repo.GetAsync(
                x => x.Id == entity.Id,
                include: q => q.Include(x => x.Branch));

            var result = _mapper.Map<BankTransferRequestDto>(entity);

            return ApiResponse<BankTransferRequestDto>.Ok(
                result,
                "تم إنشاء الطلب بنجاح");
        }

        public async Task<ApiResponse<BankTransferRequestDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q.Include(x => x.Branch));

            if (entity == null)
                return ApiResponse<BankTransferRequestDto>.Fail("الطلب غير موجود");

            var result = _mapper.Map<BankTransferRequestDto>(entity);

            return ApiResponse<BankTransferRequestDto>.Ok(result);
        }

        public async Task<ApiResponse<IReadOnlyList<BankTransferRequestDto>>> GetPendingAsync()
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var data = await repo.GetAllAsync(
                x => x.Status == TransferRequestStatus.Pending,
                include: q => q.Include(x => x.Branch));

            var result =
                _mapper.Map<IReadOnlyList<BankTransferRequestDto>>(data);

            return ApiResponse<IReadOnlyList<BankTransferRequestDto>>
                .Ok(result);
        }

        public async Task<ApiResponse<IReadOnlyList<BankTransferRequestDto>>> SearchAsync(
            BankTransferRequestFilterDto filter)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var data = await repo.GetAllAsync(
                x =>

                (string.IsNullOrEmpty(filter.RequestNumber)
                    || x.RequestNumber.Contains(filter.RequestNumber))

                &&

                (!filter.BranchId.HasValue
                    || x.BranchId == filter.BranchId.Value)

                &&

                (string.IsNullOrEmpty(filter.InvoiceNumber)
                    || x.InvoiceNumber.Contains(filter.InvoiceNumber))

                &&

                (string.IsNullOrEmpty(filter.CustomerName)
                    || x.CustomerName.Contains(filter.CustomerName))

                &&

                (string.IsNullOrEmpty(filter.CustomerMobile)
                    || x.CustomerMobile.Contains(filter.CustomerMobile))

                &&

                (string.IsNullOrEmpty(filter.Iban)
                    || x.Iban.Contains(filter.Iban))

                &&

                (!filter.Status.HasValue
                    || (int)x.Status == filter.Status.Value)

                &&

                (!filter.FromRequestDate.HasValue
                    || x.RequestDate >= filter.FromRequestDate.Value)

                &&

                (!filter.ToRequestDate.HasValue
                    || x.RequestDate <= filter.ToRequestDate.Value)

                &&

                (!filter.FromTransferDate.HasValue
                    || x.TransferDate >= filter.FromTransferDate.Value)

                &&

                (!filter.ToTransferDate.HasValue
                    || x.TransferDate <= filter.ToTransferDate.Value),

                include: q => q.Include(x => x.Branch));

            var result =
                _mapper.Map<IReadOnlyList<BankTransferRequestDto>>(data);

            return ApiResponse<IReadOnlyList<BankTransferRequestDto>>
                .Ok(result);
        }

        public async Task<ApiResponse<bool>> UpdateStatusAsync(
            UpdateTransferStatusDto dto,
            string processedBy)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var entity = await repo.GetByIdAsync(dto.RequestId);

            if (entity == null)
                return ApiResponse<bool>.Fail("الطلب غير موجود");

            entity.Status = (TransferRequestStatus)dto.Status;

            entity.TransferReferenceNumber = dto.TransferReferenceNumber;

            if ((TransferRequestStatus)dto.Status ==
                TransferRequestStatus.Completed)
            {
                if (!entity.TransferDate.HasValue)
                {
                    entity.TransferDate = DateTime.Now;
                    entity.ProcessedBy = processedBy;
                }
            }

            repo.Update(entity);

            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(
                true,
                "تم تحديث الحالة بنجاح");
        }

        public async Task UpdateAttachmentAsync(UpdateAttachmentDto dto)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var request = await repo.GetByIdAsync(dto.RequestId);

            if (request == null)
                throw new Exception("Request not found");

            request.AttachmentPath = dto.AttachmentPath;

            repo.Update(request);

            await _unitOfWork.CompleteAsync();
        }


    }
}