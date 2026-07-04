FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["VikashERP.API/VikashERP.API.csproj", "VikashERP.API/"]
COPY ["VikashERP.Application/VikashERP.Application.csproj", "VikashERP.Application/"]
COPY ["VikashERP.Domain/VikashERP.Domain.csproj", "VikashERP.Domain/"]
COPY ["VikashERP.Infrastructure/VikashERP.Infrastructure.csproj", "VikashERP.Infrastructure/"]
COPY ["VikashERP.SharedKernel/VikashERP.SharedKernel.csproj", "VikashERP.SharedKernel/"]
RUN dotnet restore "VikashERP.API/VikashERP.API.csproj"
COPY . .
WORKDIR "/src/VikashERP.API"
RUN dotnet build "VikashERP.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VikashERP.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VikashERP.API.dll"]
