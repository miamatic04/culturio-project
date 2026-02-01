FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Span.Culturio.Packages/Span.Culturio.Packages.csproj", "Span.Culturio.Packages/"]
RUN dotnet restore "Span.Culturio.Packages/Span.Culturio.Packages.csproj"

COPY . .
WORKDIR "/src/Span.Culturio.Packages"
RUN dotnet build "Span.Culturio.Packages.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Span.Culturio.Packages.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Span.Culturio.Packages.dll"]