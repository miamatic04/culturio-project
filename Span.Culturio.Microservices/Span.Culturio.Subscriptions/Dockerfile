FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Span.Culturio.Subscriptions/Span.Culturio.Subscriptions.csproj", "Span.Culturio.Subscriptions/"]
RUN dotnet restore "Span.Culturio.Subscriptions/Span.Culturio.Subscriptions.csproj"

COPY . .
WORKDIR "/src/Span.Culturio.Subscriptions"
RUN dotnet build "Span.Culturio.Subscriptions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Span.Culturio.Subscriptions.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Span.Culturio.Subscriptions.dll"]