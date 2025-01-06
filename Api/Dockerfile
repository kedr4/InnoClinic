FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080


WORKDIR /app
COPY ./out ./
ENTRYPOINT ["dotnet", "Presentation.dll"]