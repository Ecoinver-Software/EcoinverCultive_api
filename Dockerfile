FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY ./publish ./
EXPOSE 80
ENTRYPOINT ["dotnet", "EcoinverGMAO_api.dll"]

