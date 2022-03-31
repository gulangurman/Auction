#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.15 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.15 AS build
WORKDIR /src
COPY ["Auction.Auction/Auction.Auction.csproj", "Auction.Auction/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
RUN dotnet restore "Auction.Auction/Auction.Auction.csproj"
COPY . .
WORKDIR "/src/Auction.Auction"
RUN dotnet build "Auction.Auction.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auction.Auction.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.Auction.dll"]