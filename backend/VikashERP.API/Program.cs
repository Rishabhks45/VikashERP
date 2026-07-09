using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using VikashERP.Application.Features.Auth.Commands;
using VikashERP.Application.Features.Email.Validators;
using VikashERP.Application.Features.Organization.Validators;
using VikashERP.Application.Features.Users.Validators;
using VikashERP.Application.Features.Customers.Validators;
using VikashERP.Application.Interfaces;
using VikashERP.Infrastructure;
using VikashERP.Infrastructure.Authentication;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Services;
using VikashERP.SharedKernel.Settings;
using VikashERP.API.Services;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "VikashERP API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("Encryption"));
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));

var connString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddInfrastructure(connString);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));
builder.Services.AddScoped<CreateEmailTemplateRequestValidator>();
builder.Services.AddScoped<UpdateEmailTemplateRequestValidator>();
builder.Services.AddScoped<UpdateOrganizationRequestValidator>();
builder.Services.AddScoped<CreateUserAccountDtoValidator>();
builder.Services.AddScoped<UpdateUserAccountDtoValidator>();
builder.Services.AddScoped<CreateCustomerDtoValidator>();
builder.Services.AddScoped<UpdateCustomerDtoValidator>();
builder.Services.AddScoped<UpdateCustomerShopDtoValidator>();

var jwtProvider = new JwtProvider(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
        options.TokenValidationParameters = jwtProvider.GetTokenValidationParameters();
        options.MapInboundClaims = false;
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowBlazorApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
