FROM microsoft/aspnetcore-build:2.0
COPY dist /app
COPY db /docker-entrypoint-initdb.d
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet","Auth.dll"]