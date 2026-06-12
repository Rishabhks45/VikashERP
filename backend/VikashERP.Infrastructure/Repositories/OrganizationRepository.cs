using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public Task<Organization?> GetAsync(CancellationToken cancellationToken = default) =>
        _context.Organizations.AsNoTracking().OrderBy(o => o.Id).FirstOrDefaultAsync(cancellationToken);

    public async Task<Organization> GetOrCreateDefaultAsync(CancellationToken cancellationToken = default)
    {
        var existing = await _context.Organizations.OrderBy(o => o.Id).FirstOrDefaultAsync(cancellationToken);
        if (existing is not null)
            return existing;

        // Empty shell only — all business details come from DB migrations or /admin/system.
        var organization = new Organization
        {
            Id = 1,
            LegalName = "Organization",
            DisplayName = "Organization",
            Country = "India",
            DefaultCurrency = "INR",
            DefaultWeightUnit = "KG",
            TimeZone = "Asia/Kolkata",
            DateFormat = "dd-MM-yyyy",
            EnableLowStockAlerts = true,
            EnablePaymentReminders = true,
            EnableTradeConfirmations = true,
            IsActive = true
        };

        _context.Organizations.Add(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return organization;
    }

    public async Task UpdateAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        organization.UpdatedAt = DateTime.UtcNow;
        _context.Organizations.Update(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
