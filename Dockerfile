FROM microsoft/aspnetcore-build:2.0
COPY dist /app
WORKDIR /app
EXPOSE 8080/tcp
ENTRYPOINT ["dotnet","Auth.dll"]