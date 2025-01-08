FROM mcr.microsoft.com/dotnet/sdk:9.0
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT Development
WORKDIR /src
COPY . .
#RUN dotnet build "./Api/Presentation.csproj" -o /Out/Build /p:RunTests=true
RUN dotnet build "./Api/Presentation.csproj" -o ./Out/Build
WORKDIR "./Out/Build"
ENTRYPOINT ["dotnet", "Presentation.dll"]