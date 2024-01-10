

Ogni microservizio ha una solution con all'interno i progetti

La struttura cartella di un microservizio:

NomeSoluzione:
- Docker-compose.yaml
- Nomesoluzione.Api
	- Controllers
		- Controller.cs (anche più di uno, se specializzati per tabella)
	- Program.cs
	- Dockerfile
- NomeSoluzione.Client.Http (comunicazione tra microservizi)
	- Abstractions (interfacce)
- NomeSoluzione.Business
	- Kafka
	- Profiles (mapping tra Shared.DTO a Repository.Model)
	- Business.cs
	- IBusiness.cs (espone le funzionalità dell'applicazione)
- NomeSoluzione.Repository (accesso alla base dati)
	- Migration (o scriptingDbContext)
	- Model ()
	- IRepository.cs (magari nella cartella abstractions)
	- Repository.cd (dialogo con il db)
	- NomeSoluzioneDbContext.cs (una o più implementazioni)
- NomeSoluzione.Servlet (può essere unito al .Api)
- NomeSoluzione.Shared  (da condividere con gli altri microservizi)
	- DTO


`#region` e `#endregion` per raggruppamento di blocchi di codice