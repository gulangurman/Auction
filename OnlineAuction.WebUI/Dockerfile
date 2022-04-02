#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["OnlineAuction.WebUI/OnlineAuction.WebUI.csproj", "OnlineAuction.WebUI/"]
RUN dotnet restore "OnlineAuction.WebUI/OnlineAuction.WebUI.csproj"
COPY . .
WORKDIR "/src/OnlineAuction.WebUI"
RUN dotnet build "OnlineAuction.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineAuction.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineAuction.WebUI.dll"]
