using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Suppliers.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Suppliers.Queries;

public class SupplierQueryHandlers :
    IRequestHandler<GetSuppliersQuery, List<SupplierListDto>>,
    IRequestHandler<GetSupplierByIdQuery, SupplierDto?>
{
    private readonly ISupplierService _service;

    public SupplierQueryHandlers(ISupplierService service)
    {
        _service = service;
    }

    public async Task<List<SupplierListDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(cancellationToken);
    }

    public async Task<SupplierDto?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id, cancellationToken);
    }
}
