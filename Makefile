build:
	@dotnet build

run:
	@dotnet run --project ./MarketAPI/MarketAPI.csproj

watch:
	@dotnet watch --project ./MarketAPI/MarketAPI.csproj

publish:
	@dotnet publish -c Release -o ./publish