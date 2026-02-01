FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Span.Culturio.Users/Span.Culturio.Users.csproj", "Span.Culturio.Users/"]
RUN dotnet restore "Span.Culturio.Users/Span.Culturio.Users.csproj"

COPY . .
WORKDIR "/src/Span.Culturio.Users"
RUN dotnet build "Span.Culturio.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Span.Culturio.Users.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Span.Culturio.Users.dll"]