FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TravelAndAccommodationBookingPlatform.Api/TravelAndAccommodationBookingPlatform.Api.csproj", "src/TravelAndAccommodationBookingPlatform.Api/"]
COPY ["src/TravelAndAccommodationBookingPlatform.Domain/TravelAndAccommodationBookingPlatform.Domain.csproj", "src/TravelAndAccommodationBookingPlatform.Domain/"]
COPY ["src/TravelAndAccommodationBookingPlatform.Infrastructure/TravelAndAccommodationBookingPlatform.Infrastructure.csproj", "src/TravelAndAccommodationBookingPlatform.Infrastructure/"]
COPY ["src/TravelAndAccommodationBookingPlatform.Application/TravelAndAccommodationBookingPlatform.Application.csproj", "src/TravelAndAccommodationBookingPlatform.Application/"]
RUN dotnet restore "src/TravelAndAccommodationBookingPlatform.Api/TravelAndAccommodationBookingPlatform.Api.csproj"
COPY . .
WORKDIR "/src/src/TravelAndAccommodationBookingPlatform.Api"
RUN dotnet build "TravelAndAccommodationBookingPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TravelAndAccommodationBookingPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TravelAndAccommodationBookingPlatform.Api.dll"]
