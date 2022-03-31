#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Auction.Order/Auction.Order.csproj", "Auction.Order/"]
COPY ["Ordering.Application/Ordering.Application.csproj", "Ordering.Application/"]
COPY ["Ordering.Domain/Ordering.Domain.csproj", "Ordering.Domain/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["../Ordering.Infrastructure/Ordering.Infrastructure.csproj", "../Ordering.Infrastructure/"]
RUN dotnet restore "Auction.Order/Auction.Order.csproj"
COPY . .
WORKDIR "/src/Auction.Order"
RUN dotnet build "Auction.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auction.Order.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.Order.dll"]
