#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Auction.Products/Auction.Products.csproj", "Auction.Products/"]
RUN dotnet restore "Auction.Products/Auction.Products.csproj"
COPY . .
WORKDIR "/src/Auction.Products"
RUN dotnet build "Auction.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auction.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.Products.dll"]
