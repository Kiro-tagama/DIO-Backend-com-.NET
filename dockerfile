# Use uma imagem base do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# Copie o código da sua aplicação para o contêiner
WORKDIR /app
COPY . .
# Restaure as dependências
RUN dotnet restore
# Construa a imagem da aplicação
RUN dotnet publish -c Release -o /app/publish
# Use uma imagem base mínima para a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
# Copie os arquivos publicados da etapa de build
WORKDIR /app
COPY --from=build-env /app/publish .
# Defina a porta que a aplicação irá escutar
EXPOSE 80
# Comando para executar a aplicação
ENTRYPOINT ["dotnet", "app.dll"]