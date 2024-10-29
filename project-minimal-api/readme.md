Dio Dotnet study note on ASP.NET Minimals APIs

[link Dio: ASP.NET Minimals APIs](https://web.dio.me/lab/trabalhando-com-minimals-apis/learning/fcb434a3-cda8-4ccf-9981-e923a6d0d350?back=/track/coding-future-back-end-dot-net)

---

comands to run:

start
```bash
  docker compose up -d
  sleep 10 # sleep 10 seconds to start db (linux)
  dotnet run # dotnet watch run
```

to check db in docker
```bash
  docker exec -it project-minimal-api-mysql-1 mysql -uroot -ppassword123456 -e "SHOW DATABASES;"
```

others
```bash
  dotnet ef migrations add NameOfMigrations

  dotnet ef database update

  # cria solution
  dotnet new sln
  dotnet sln add Api/project-minimal-api.csproj 

  # add referencia do projetos test para ter acesso a api
  dotnet add reference ../Api/project-minimal-api.csproj 
```

---

problems: (no problens ...)