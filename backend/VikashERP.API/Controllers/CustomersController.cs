using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Features.Customers.DTOs;
using VikashERP.Application.Features.Customers.Validators;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Enums;
using VikashERP.SharedKernel.Settings;
using VikashERP.SharedKernel.Extensions;

namespace VikashERP.API.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly Microsoft.Extensions.Options.IOptions<EncryptionSettings> _encryptionOptions;
    private readonly ISharedRepository _sharedRepository;
    private readonly CreateCustomerDtoValidator _createValidator;
    private readonly UpdateCustomerDtoValidator _updateValidator;
    private readonly UpdateCustomerShopDtoValidator _shopUpdateValidator;

    public CustomersController(
        ApplicationDbContext context,
        ICustomerRepository customerRepository,
        IEncryptionService encryptionService,
        Microsoft.Extensions.Options.IOptions<EncryptionSettings> encryptionOptions,
        ISharedRepository sharedRepository,
        CreateCustomerDtoValidator createValidator,
        UpdateCustomerDtoValidator updateValidator,
        UpdateCustomerShopDtoValidator shopUpdateValidator)
    {
        _context = context;
        _customerRepository = customerRepository;
        _encryptionService = encryptionService;
        _encryptionOptions = encryptionOptions;
        _sharedRepository = sharedRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _shopUpdateValidator = shopUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _context.Customers
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => MapToListDto(c))
            .ToListAsync(cancellationToken);

        return Ok(customers);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var customer = await ResolveCustomerAsync(cancellationToken);
        if (customer is null)
            return NotFound(new { Message = "No customer profile linked to this account." });

        return Ok(MapToListDto(customer));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateCustomerShopDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _shopUpdateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new { Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) });

        var customer = await ResolveCustomerAsync(cancellationToken, dto);
        if (customer is null)
            return NotFound(new { Message = "No customer profile linked to this account." });

        var paymentMode = CustomerPaymentModeExtensions.FromString(dto.DefaultPaymentMode) ?? CustomerPaymentMode.Account;

        customer.CompanyName = NullIfWhiteSpace(dto.CompanyName);
        customer.Phone = dto.Phone.Trim();
        customer.Email = NullIfWhiteSpace(dto.Email);
        customer.Gstin = NullIfWhiteSpace(dto.Gstin);
        customer.Address = NullIfWhiteSpace(dto.Address);
        customer.DefaultPaymentMode = paymentMode;

        await _context.SaveChangesAsync(cancellationToken);
        return Ok(MapToListDto(customer));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == id)
            .Select(c => MapToListDto(c))
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new { Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) });

        var paymentMode = CustomerPaymentModeExtensions.FromString(dto.DefaultPaymentMode) ?? CustomerPaymentMode.Account;
        var accountNumber = await _customerRepository.GenerateAccountNumberAsync(cancellationToken);

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            AccountNumber = accountNumber,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            CompanyName = NullIfWhiteSpace(dto.CompanyName),
            Phone = dto.Phone.Trim(),
            Email = NullIfWhiteSpace(dto.Email),
            Gstin = NullIfWhiteSpace(dto.Gstin),
            Address = NullIfWhiteSpace(dto.Address),
            DefaultPaymentMode = paymentMode,
            CreditLimit = dto.CreditLimit,
            CurrentBalance = 0,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.GetAuthenticatedUserId()
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        var welcomeEmailSent = await TryProvisionCustomerPortalAsync(customer, cancellationToken);

        return Ok(new
        {
            Customer = MapToListDto(customer),
            PortalAccessCreated = welcomeEmailSent,
            WelcomeEmailSent = welcomeEmailSent
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new { Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) });

        var customer = await _context.Customers.FindAsync([id], cancellationToken);
        if (customer is null)
            return NotFound();

        var paymentMode = CustomerPaymentModeExtensions.FromString(dto.DefaultPaymentMode) ?? CustomerPaymentMode.Account;

        customer.FirstName = dto.FirstName.Trim();
        customer.LastName = dto.LastName.Trim();
        customer.CompanyName = NullIfWhiteSpace(dto.CompanyName);
        customer.Phone = dto.Phone.Trim();
        customer.Email = NullIfWhiteSpace(dto.Email);
        customer.Gstin = NullIfWhiteSpace(dto.Gstin);
        customer.Address = NullIfWhiteSpace(dto.Address);
        customer.DefaultPaymentMode = paymentMode;
        customer.CreditLimit = dto.CreditLimit;
        customer.UpdatedAt = DateTime.UtcNow;
        customer.UpdatedBy = User.GetAuthenticatedUserId();

        await _context.SaveChangesAsync(cancellationToken);
        return Ok(MapToListDto(customer));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync([id], cancellationToken);
        if (customer is null)
            return NotFound();

        var hasInvoices = await _context.Invoices.AnyAsync(i => i.CustomerId == id, cancellationToken);
        if (hasInvoices)
            return BadRequest(new { Message = "Cannot delete a customer with existing invoices." });

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    private static CustomerListDto MapToListDto(Customer customer) => new()
    {
        Id = customer.Id,
        AccountNumber = customer.AccountNumber,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        CompanyName = customer.CompanyName,
        Phone = customer.Phone,
        Email = customer.Email,
        Gstin = customer.Gstin,
        Address = customer.Address,
        DefaultPaymentMode = customer.DefaultPaymentMode.ToFriendlyName(),
        CreditLimit = customer.CreditLimit,
        CurrentBalance = customer.CurrentBalance,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
    };

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private Guid? GetCustomerIdFromClaim()
    {
        var claim = User.FindFirst("customer_id")?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    private async Task<Customer?> ResolveCustomerAsync(
        CancellationToken cancellationToken,
        UpdateCustomerShopDto? shopDto = null)
    {
        var customerId = GetCustomerIdFromClaim();
        if (customerId.HasValue)
        {
            return await _context.Customers.FindAsync([customerId.Value], cancellationToken);
        }

        var userId = User.GetAuthenticatedUserId();
        if (userId is null)
            return null;

        var mapping = await _context.UserCustomerMappings
            .Include(m => m.Customer)
            .FirstOrDefaultAsync(m => m.UserId == userId.Value && m.IsActive, cancellationToken);

        if (mapping?.Customer is not null)
            return mapping.Customer;

        var user = await _context.Users.FindAsync([userId.Value], cancellationToken);
        if (user is null || user.Role != UserRole.Customer)
            return null;

        var accountNumber = await _customerRepository.GenerateAccountNumberAsync(cancellationToken);
        var paymentMode = shopDto is not null
            ? CustomerPaymentModeExtensions.FromString(shopDto.DefaultPaymentMode) ?? CustomerPaymentMode.Account
            : CustomerPaymentMode.Account;

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            AccountNumber = accountNumber,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CompanyName = NullIfWhiteSpace(shopDto?.CompanyName),
            Phone = shopDto?.Phone.Trim() ?? string.Empty,
            Email = NullIfWhiteSpace(shopDto?.Email) ?? user.Email,
            Gstin = NullIfWhiteSpace(shopDto?.Gstin),
            Address = NullIfWhiteSpace(shopDto?.Address),
            DefaultPaymentMode = paymentMode,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        _context.UserCustomerMappings.Add(new UserCustomerMapping
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CustomerId = customer.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        });
        await _context.SaveChangesAsync(cancellationToken);

        return customer;
    }

    private async Task<bool> TryProvisionCustomerPortalAsync(Customer customer, CancellationToken cancellationToken)
    {
        var email = customer.Email?.Trim();
        if (string.IsNullOrWhiteSpace(email))
            return false;

        email = email.ToLowerInvariant();
        var userName = customer.FullName;

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email, cancellationToken);

        if (existingUser is not null)
        {
            if (existingUser.Role != UserRole.Customer)
                return false;

            var userAlreadyMapped = await _context.UserCustomerMappings
                .AnyAsync(m => m.UserId == existingUser.Id && m.IsActive, cancellationToken);
            if (userAlreadyMapped)
                return false;

            var customerAlreadyMapped = await _context.UserCustomerMappings
                .AnyAsync(m => m.CustomerId == customer.Id && m.IsActive, cancellationToken);
            if (customerAlreadyMapped)
                return false;

            _context.UserCustomerMappings.Add(new UserCustomerMapping
            {
                Id = Guid.NewGuid(),
                UserId = existingUser.Id,
                CustomerId = customer.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var plainPassword = _encryptionService.GenerateTemporaryPassword();
        var encryptedPassword = await _encryptionService.EncryptAsync(
            plainPassword,
            _encryptionOptions.Value.MasterKey);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = email,
            PasswordHash = encryptedPassword,
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        _context.UserCustomerMappings.Add(new UserCustomerMapping
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CustomerId = customer.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync(cancellationToken);

        return await _sharedRepository.SendWelcomeEmailAsync(email, userName, plainPassword, cancellationToken);
    }
}
