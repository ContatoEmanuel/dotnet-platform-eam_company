# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["EAM.Solution.sln", "./"]
COPY ["src/1.Core/EAM.Core.Domain/EAM.Core.Domain.csproj", "src/1.Core/EAM.Core.Domain/"]
COPY ["src/1.Core/EAM.Core.Application/EAM.Core.Application.csproj", "src/1.Core/EAM.Core.Application/"]
COPY ["src/2.Infrastructure/EAM.Infra.Data/EAM.Infra.Data.csproj", "src/2.Infrastructure/EAM.Infra.Data/"]
COPY ["src/2.Infrastructure/EAM.Infra.IoC/EAM.Infra.IoC.csproj", "src/2.Infrastructure/EAM.Infra.IoC/"]
COPY ["src/3.Presentation/EAM.Web.API/EAM.Web.API.csproj", "src/3.Presentation/EAM.Web.API/"]

# Restore dependencies
RUN dotnet restore "src/3.Presentation/EAM.Web.API/EAM.Web.API.csproj"

# Copy source code
COPY . .

# Build
WORKDIR "/src/src/3.Presentation/EAM.Web.API"
RUN dotnet build "EAM.Web.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "EAM.Web.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAM.Web.API.dll"]
