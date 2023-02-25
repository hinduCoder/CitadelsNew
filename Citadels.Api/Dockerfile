FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Citadels.Api.csproj", "Citadels.Api/"]
RUN dotnet restore "Citadels.Api/Citadels.Api.csproj"
COPY . ./Citadels.Api
WORKDIR "/src/Citadels.Api"
RUN dotnet build "Citadels.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Citadels.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Citadels.Api.dll"]
