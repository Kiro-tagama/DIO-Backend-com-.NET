start a dump in docker

instructions used in test
```bash
  docker exec -it project-minimal-api-mysql-1 bash

  # in mysql
  mysql -uroot -p'password123456'

  # a new db
  create database minimal_api_test;

  exit
  exit


  docker exec project-minimal-api-mysql-1 mysqldump -uroot -p'password123456' minimal_api > ./minimal_api.dump.sql

  # creating a copy in docker
  docker cp ./minimal_api.dump.sql project-minimal-api-mysql-1:/tmp/minimal_api.dump.sql

  # add dump in minimal_api_test
  docker exec -it project-minimal-api-mysql-1 bash
  mysqldump -uroot -p'password123456' minimal_api_test < /tmp/minimal_api.dump.sql

  # confirm data
  mysql -uroot -p'password123456'

  use minimal_api_test;

  show tables;
  
  select * from Administrators;

```