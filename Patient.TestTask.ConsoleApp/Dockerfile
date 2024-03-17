FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Patient.TestTask.ConsoleApp/Patient.TestTask.ConsoleApp.csproj", "Patient.TestTask.ConsoleApp/"]
RUN dotnet restore "Patient.TestTask.ConsoleApp/Patient.TestTask.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/Patient.TestTask.ConsoleApp"
RUN dotnet build "Patient.TestTask.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patient.TestTask.ConsoleApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patient.TestTask.ConsoleApp.dll"]
