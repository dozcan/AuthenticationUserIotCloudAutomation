FROM microsoft/aspnetcore-build:2.0
COPY dist /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["","Auth.dll"]

