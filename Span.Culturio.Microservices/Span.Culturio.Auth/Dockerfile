FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Span.Culturio.Auth/Span.Culturio.Auth.csproj", "Span.Culturio.Auth/"]
RUN dotnet restore "Span.Culturio.Auth/Span.Culturio.Auth.csproj"

COPY . .
WORKDIR "/src/Span.Culturio.Auth"
RUN dotnet build "Span.Culturio.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Span.Culturio.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Span.Culturio.Auth.dll"]