FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN mkdir -p /ssl/certs /ssl/private
COPY ./publish ./
COPY ssl/server.crt /ssl/certs/server.crt
COPY ssl/server.key /ssl/private/server.key
EXPOSE 80 443
ENTRYPOINT ["dotnet", "EcoinverGMAO_api.dll"]