# Utilizzo di PostgreSQL con Entity Framework

## Link utili
- [.NET 6.0 - Connect to PostgreSQL Database with Entity Framework Core](https://jasonwatmore.com/post/2022/06/23/net-6-connect-to-postgresql-database-with-entity-framework-core)
- [Official PostgreSQL docker image](https://hub.docker.com/_/postgres)

## How to
È necessario come prima cosa installare il pacchetto NuGet che permette di utilizzare Entity Framework con PostgreSQL:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL -v 7.0
```

Dopo aver creato una cartella _Database_ nella root del progetto, bisogna inserirvi il file `docker-compose.yaml` come indicato nella documentazione ufficiale:
```bash
.
├── appsettings.Development.json
├── appsettings.json
├── bin
│   └── Debug
├── Database				# la cartella in questione
│   └── docker-compose.yaml
├── Program.cs

# ...
```

Il contenuto del compose:
```yaml
# Use postgres/example user/password credentials
version: '3.1'

services:
  db:
    image: postgres
    restart: always
    environment:
      POSTGRES_PASSWORD: password
    ports:
      - 2345:5432

  adminer:
    image: adminer
    restart: always
    ports:
      - 8080:8080
```
Questo permette di avviare un database _PostgreSQL_ e il frontend _Adminer_ su due container; al primo è possibile fare richieste tramite la porta **2345** su localhost, mentre al secondo tramite la porta **8080**. Quindi:
- PostgreSQL $\longrightarrow$ localhost:2345
- Adminer $\longrightarrow$ localhost:8080

La **stringa di connessione** da utilizzare in questo caso specifico è
```C#
string connectionString = "Host=localhost;Port=2345;Database=postgres;Uid=postgres;Pwd=password;";
```
che volendo si può inserire nel file `appsettings.json`.

### Variabili d'ambiente utili
|       Env var       | Description                  |
|:-------------------:| -----------------------------|
| `POSTGRES_PASSWORD` | Imposta la password di superuser per PostgreSQL (quella della connectionString) |
| `POSTGRES_USER` | Crea un utente superuser e un database con lo stesso nome (se non specificato è `postgres` di default)|
| `POSTGRES_DB` | Per impostare un nome differente al database di default |
| `PGDATA` | Imposta la cartella dati da utilizzare (default `/var/lib/postgresql/data`) |


### Aggiungere query iniziali
Si possono aggiungere scripts `*.sql`, `*.sql.gz`, o `*.sh` nella cartella `/docker-entrypoint-initdb.d`.

> **Nota Bene**
>
> Il contenuto di questa cartella viene eseguito solo se il container viene lanciato con una cartella dati vuota (ossia `/var/lib/postgresql/data`); quindi se una query di uno script non va a buon fine e provate a rilanciare il container, gli scripts verranno saltati.

### Connessione PGAdminer
Nel campo _Server_ bisogna indicare il nome del servizio scelto nel docker-compose (`db` nell'esempio precedente); infatti viene creata automaticamente una rete __default_database__ di tipo bridge a cui sono collegati i vari container.