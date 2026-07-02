using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class TimezoneRepository : ITimezoneRepository
{
    private readonly ApplicationDbContext _context;

    public TimezoneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Timezone>> GetActiveTimezonesAsync(CancellationToken cancellationToken)
    {
        return await _context.Timezones
            .AsNoTracking()
            .Where(t => t.IsActive && !t.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
