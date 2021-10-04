#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NServiceBusService/NServiceBusService.csproj", "NServiceBusService/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "NServiceBusService/NServiceBusService.csproj"
COPY . .
WORKDIR "/src/NServiceBusService"
RUN dotnet build "NServiceBusService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NServiceBusService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NServiceBusService.dll"]