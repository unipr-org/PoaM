# Kafka

## Struttura cartella

```bash
.
├── Models
│  ├── AbstractTransictionalOutbox.cs
│  └── TransactionalOutboxFactory.cs
└── README.md
```

## Descrizione contenuto

### AbstractTransactionalOutbox

`AbstractTransactionalOutbox` è una classe astratta che definisce la struttura di una `TransactionalOutbox`, per l'utilizzo:


Esempio utilizzo
```bash
public class ConcreteTransactionalOutbox : AbstractTransactionalOutbox {

}
```


### TransactionalOutboxFactory
`TransactionalOutboxFactory` è una classe che si occupa di creare delle `TransactionalOutbox`, è templatica rispetto `<DTO, MODEL, RET>` dove:
- `DTO` corrisponde al DTO (per un corretto funzionamento il dto non deve presentare la keyword `required` nei parametri)
- `MODEL` indica il modello
- `RET` indica il tepo di ritorno dei metodi (che deve estendere AbstractTransactionalOutbox)


Esempio utilizzo
```cs
// ...
var model = new Concrete_modelDTO {
    // ...
};
await repository_.AddTransactionalOutbox(
    TransactionalOutboxFactory.CreateInsert<Concrete_modelDTO, Concrete_model, ConcreteTransactionalOutbox>(model);
);
// ...
```

> `Concrete_modelDTO` e `Concrete_model` sono sostituibili da qualunque tipo di DTO o model Correlati