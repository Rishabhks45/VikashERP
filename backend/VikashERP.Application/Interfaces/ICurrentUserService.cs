using System;

namespace VikashERP.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
