using AutoMapper;
using BranchERP.Application.DTOs.BankTransferRequests;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services
{
    internal class BankTransferRequestService : IBankTransferRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankTransferRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    
        public async Task<ApiResponse<BankTransferRequestDto>> CreateAsync(
         CreateBankTransferRequestDto dto)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var entity = _mapper.Map<BankTransferRequest>(dto);

            entity.RequestDate = DateTime.Now;

            entity.Status = TransferRequestStatus.Pending;

            entity.RequestNumber =
                $"BT-{DateTime.Now:yyyyMMddHHmmss}";

            await repo.AddAsync(entity);

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<BankTransferRequestDto>(entity);

            return ApiResponse<BankTransferRequestDto>.Ok(result);
        }


        public async Task<ApiResponse<BankTransferRequestDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q.Include(x => x.Branch)
            );

            if (entity == null)
                return ApiResponse<BankTransferRequestDto>.Fail("الطلب غير موجود");

            var result = _mapper.Map<BankTransferRequestDto>(entity);

            return ApiResponse<BankTransferRequestDto>.Ok(result);
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

            if (entity.Status == TransferRequestStatus.Completed)
            {
                entity.TransferDate = DateTime.Now;
                entity.ProcessedBy = processedBy;
            }

            repo.Update(entity);

            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true);
        }

        public async Task<ApiResponse<IReadOnlyList<BankTransferRequestDto>>> SearchAsync(
           BankTransferRequestFilterDto filter)
        {
            var repo = _unitOfWork.Repository<BankTransferRequest>();

            var data = await repo.GetAllAsync(
                filter: x =>

                    (string.IsNullOrEmpty(filter.RequestNumber)
                        || x.RequestNumber.Contains(filter.RequestNumber))

                    &&

                    (!filter.BranchId.HasValue
                        || x.BranchId == filter.BranchId)

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
                        || (int)x.Status == filter.Status)

                    &&

                    (!filter.FromRequestDate.HasValue
                        || x.RequestDate >= filter.FromRequestDate)

                    &&

                    (!filter.ToRequestDate.HasValue
                        || x.RequestDate <= filter.ToRequestDate)

                    &&

                    (!filter.FromTransferDate.HasValue
                        || x.TransferDate >= filter.FromTransferDate)

                    &&

                    (!filter.ToTransferDate.HasValue
                        || x.TransferDate <= filter.ToTransferDate),

                include: q => q.Include(x => x.Branch)
            );

            var result =
                _mapper.Map<IReadOnlyList<BankTransferRequestDto>>(data);

            return ApiResponse<IReadOnlyList<BankTransferRequestDto>>
                .Ok(result);
        }

    }
}
