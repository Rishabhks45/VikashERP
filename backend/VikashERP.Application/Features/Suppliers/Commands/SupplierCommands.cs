using System;
using MediatR;
using VikashERP.Application.Features.Suppliers.DTOs;

namespace VikashERP.Application.Features.Suppliers.Commands;

public record CreateSupplierCommand(CreateSupplierDto SupplierDto) : IRequest<SupplierDto?>;

public record UpdateSupplierCommand(Guid Id, UpdateSupplierDto SupplierDto) : IRequest<SupplierDto?>;

public record DeleteSupplierCommand(Guid Id) : IRequest<bool>;
