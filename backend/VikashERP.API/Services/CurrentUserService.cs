using System;
using Microsoft.AspNetCore.Http;
using VikashERP.Application.Interfaces;
using VikashERP.SharedKernel.Extensions;

namespace VikashERP.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.GetAuthenticatedUserId();
        }
    }
}
