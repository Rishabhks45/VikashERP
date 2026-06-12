using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using VikashERP.Application.Features.Auth.Commands;
using VikashERP.Application.Features.Email.Validators;
using VikashERP.Application.Features.Organization.Validators;
using VikashERP.Application.Features.Users.Validators;
using VikashERP.Application.Interfaces;
using VikashERP.Infrastructure;
using VikashERP.Infrastructure.Authentication;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Services;
using VikashERP.SharedKernel.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("Encryption"));
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));

var connString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddInfrastructure(connString);

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));
builder.Services.AddScoped<CreateEmailTemplateRequestValidator>();
builder.Services.AddScoped<UpdateEmailTemplateRequestValidator>();
builder.Services.AddScoped<UpdateOrganizationRequestValidator>();
builder.Services.AddScoped<CreateUserAccountDtoValidator>();
builder.Services.AddScoped<UpdateUserAccountDtoValidator>();

var jwtProvider = new JwtProvider(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
        options.TokenValidationParameters = jwtProvider.GetTokenValidationParameters();
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7297",
                "http://localhost:5090",
                "https://localhost:7013",
                "http://localhost:5263")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
