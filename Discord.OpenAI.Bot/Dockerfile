#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Discord.OpenAI.Bot/Discord.OpenAI.Bot.csproj", "Discord.OpenAI.Bot/"]
RUN dotnet restore "Discord.OpenAI.Bot/Discord.OpenAI.Bot.csproj"
COPY . .
WORKDIR "/src/Discord.OpenAI.Bot"
RUN dotnet build "Discord.OpenAI.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discord.OpenAI.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discord.OpenAI.Bot.dll"]