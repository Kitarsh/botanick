FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY /BotANick.MainConsole/bin/Release/netcoreapp3.1/linux-arm/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "MainConsole.dll", "discord", "twitch"]
