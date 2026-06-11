using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<string> GenerateAccountNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"CUS-{year}-";
        var last = await DbSet
            .Where(c => c.AccountNumber.StartsWith(prefix))
            .OrderByDescending(c => c.AccountNumber)
            .Select(c => c.AccountNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var seq = 1;
        if (last is not null && int.TryParse(last.Split('-')[^1], out var lastSeq))
            seq = lastSeq + 1;

        return $"{prefix}{seq:D6}";
    }

    public Task<Customer?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        return FirstOrDefaultAsync(c => c.AccountNumber == accountNumber, cancellationToken);
    }
}
