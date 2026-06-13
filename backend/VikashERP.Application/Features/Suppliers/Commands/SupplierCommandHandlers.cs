using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Suppliers.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Suppliers.Commands;

public class SupplierCommandHandlers : 
    IRequestHandler<CreateSupplierCommand, SupplierDto?>,
    IRequestHandler<UpdateSupplierCommand, SupplierDto?>,
    IRequestHandler<DeleteSupplierCommand, bool>
{
    private readonly ISupplierService _service;

    public SupplierCommandHandlers(ISupplierService service)
    {
        _service = service;
    }

    public async Task<SupplierDto?> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        return await _service.CreateAsync(request.SupplierDto, cancellationToken);
    }

    public async Task<SupplierDto?> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(request.Id, request.SupplierDto, cancellationToken);
    }

    public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        return await _service.DeleteAsync(request.Id, cancellationToken);
    }
}
