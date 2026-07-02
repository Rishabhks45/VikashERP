using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface ITimezoneRepository
{
    Task<List<Timezone>> GetActiveTimezonesAsync(CancellationToken cancellationToken);
}
