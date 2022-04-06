#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Auction.Ordering/Auction.Ordering.csproj", "Auction.Ordering/"]
COPY ["Ordering.Domain/Ordering.Domain.csproj", "Ordering.Domain/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
RUN dotnet restore "Auction.Ordering/Auction.Ordering.csproj"
COPY . .
WORKDIR "/src/Auction.Ordering"
RUN dotnet build "Auction.Ordering.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auction.Ordering.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.Ordering.dll"]
