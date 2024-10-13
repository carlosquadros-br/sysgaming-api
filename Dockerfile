FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS base
WORKDIR /app
EXPOSE 5132

ENV ASPNETCORE_URLS=http://+:5132

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
ARG configuration=Release
WORKDIR /src
COPY ["SysgamingApi.csproj", "./"]
RUN dotnet restore "SysgamingApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SysgamingApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SysgamingApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SysgamingApi.dll"]
