# Discord.OpenAI.Bot
This repository contains a Discord Bot which allows you to interact with OpenAI's completion API to answer random questions.

## Prerequisites
- Create a Discord Application here: https://discord.com/developers/applications
  - Now create a Bot Account for that application to get the **Bot Token**
- Create an OpenAI account
  - Get the **Organization Id** from here https://beta.openai.com/account/org-settings
  - Create and **Api Key** here: https://beta.openai.com/account/api-keys

## Setup
Download the latest release from the [Releases tab](https://github.com/TheDusty01/Discord.OpenAI.Bot/releases).\
Make sure to provide the settings via environment variables or through the [appsettings.json](Discord.OpenAI.Bot/appsettings.json) file.

You can also run this app in a docker container, a [Dockerfile](/Discord.OpenAI.Bot/Dockerfile) is already ready to be used.

## How to use
1. Adjust the settings
2. Invite the bot to your server
3. Use the ``/ask`` command to interact with the bot (or use the message based ``#ask`` command)
4. Example: ``#ask How big is Germany?``

## Build
### Visual Studio
1. Open the solution with Visual Studio 2022
2. Build the solution
3. (Optional) Publish the solution

### .NET CLI
1. ``dotnet restore "Discord.OpenAI.Bot/Discord.OpenAI.Bot.csproj"``
2. ``dotnet build "Discord.OpenAI.Bot/Discord.OpenAI.Bot.csproj" -c Release``
3. (Optional) ``dotnet publish "Discord.OpenAI.Bot/Discord.OpenAI.Bot.csproj" -c Release``

Output directory: ``Discord.OpenAI.Bot\Discord.OpenAI.Bot\bin\Release\net6.0`` \
Publish directory: ``Discord.OpenAI.Bot\Discord.OpenAI.Bot\bin\Release\net6.0\publish``

## Credits
This project uses the following open-source projects:
- https://github.com/DSharpPlus/DSharpPlus
- https://github.com/betalgo/openai

## License
Discord.OpenAI.Bot is licensed under the MIT License, see [LICENSE.txt](/LICENSE.txt) for more information.