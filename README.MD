# Project
This project contains the source files of the Web API used by the MoveMate Mobile Application.
It is developed using the .NET core framework and generated using the Web API template.

## Prerequisites
- [Dotnet SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [MariaDB](https://mariadb.com/downloads/)

## Getting started
Copy `appsettings.json` and rename it `appsettings.Development.json` configure your Kestrel endpoints as desired. For more info about Kestel visit these [docs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-7.0).

Add a default ConnectionString to your `appsettings.json` and replace the placeholders corresponding to your environment configuration:
```json
{
	"ConnectionStrings": {
		"DefaultConnection": "server=<YOUR_HOST>;database=<YOUR_DATABASE>;user=<YOUR_USER>;password=<YOUR_PASSWORD>"
	}
}
```

Run the application using the .NET core command line tools `dotnet run --environment Development`. Another way to run the application is to configure a build task and launch configuration in vscode like:

<b>`.vscode/tasks.json`</b>
```json
{
	"version": "2.0.0",
	"tasks": [
		{
			"label": "build",
			"command": "dotnet",
			"type": "process",
			"args": [
				"build",
				"${workspaceFolder}/MoveMateWebApi.sln",
				"/property:GenerateFullPaths=true",
				"/consoleloggerparameters:NoSummary"
			],
			"problemMatcher": "$msCompile"
		},
```

<b>`.vscode/launch.json`</b>
```json
{
	"version": "0.2.0",
	"configurations": [
		{
			"name": ".NET Core Launch (web)",
			"type": "coreclr",
			"request": "launch",
			"preLaunchTask": "build",
			"program": "${workspaceFolder}/bin/Debug/net7.0/MoveMateWebApi.dll",
			"args": [],
			"cwd": "${workspaceFolder}",
			"stopAtEntry": false,
			"serverReadyAction": {
				"action": "openExternally",
				"pattern": "\\bNow listening on:\\s+(https?://\\S+)",
				"uriFormat": "%s/swagger"
			},
			"env": {
				"ASPNETCORE_ENVIRONMENT": "Development"
			},
			"sourceFileMap": {
				"/Views": "${workspaceFolder}/Views"
			}
		},
		{
			"name": ".NET Core Attach",
			"type": "coreclr",
			"request": "attach"
		}
	]
}
```