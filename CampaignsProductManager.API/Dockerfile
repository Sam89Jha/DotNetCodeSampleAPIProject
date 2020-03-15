FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CampaignsProductManager.API/CampaignsProductManager.API.csproj", "CampaignsProductManager.API/"]
COPY ["src/CampaignsProductManager.Logics/CampaignsProductManager.Logic.csproj", "src/CampaignsProductManager.Logics/"]
COPY ["src/CampaignsProductManager.Core/CampaignsProductManager.Core.csproj", "src/CampaignsProductManager.Core/"]
COPY ["src/CampaignsProductManager.Controllers/CampaignsProductManager.Controllers.csproj", "src/CampaignsProductManager.Controllers/"]
COPY ["src/CampaignsProductManager.Repository/CampaignsProductManager.Repository.csproj", "src/CampaignsProductManager.Repository/"]
RUN dotnet restore "CampaignsProductManager.API/CampaignsProductManager.API.csproj"
COPY . .
WORKDIR "/src/CampaignsProductManager.API"
RUN dotnet build "CampaignsProductManager.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CampaignsProductManager.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CampaignsProductManager.API.dll"]