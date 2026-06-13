using System;
using System.Collections.Generic;
using MediatR;
using VikashERP.Application.Features.Suppliers.DTOs;

namespace VikashERP.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery() : IRequest<List<SupplierListDto>>;

public record GetSupplierByIdQuery(Guid Id) : IRequest<SupplierDto?>;
