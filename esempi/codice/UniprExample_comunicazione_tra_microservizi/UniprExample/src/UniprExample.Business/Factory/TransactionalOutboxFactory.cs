using System.Text.Json;
using UniprExample.Repository.Model;
using UniprExample.Shared.Dto;
using Utility.Kafka.Constants;
using Utility.Kafka.Messages;

namespace UniprExample.Business.Factory;

public static class TransactionalOutboxFactory
{

    #region Studenti

    public static TransactionalOutbox CreateInsert(StudenteDto dto) => Create(dto, Operations.Insert);

    public static TransactionalOutbox CreateUpdate(StudenteDto dto) => Create(dto, Operations.Update);

    public static TransactionalOutbox CreateDelete(StudenteDto dto) => Create(dto, Operations.Delete);

    private static TransactionalOutbox Create(StudenteDto dto, string operation) => Create(nameof(Studenti), dto, operation);

    #endregion

    private static TransactionalOutbox Create<TDTO>(string table, TDTO dto, string operation) where TDTO : class, new()
    {

        OperationMessage<TDTO> opMsg = new OperationMessage<TDTO>() {
            Dto = dto,
            Operation = operation
        };
        opMsg.CheckMessage();

        return new TransactionalOutbox(){
            Tabella = table,
            Messaggio = JsonSerializer.Serialize(opMsg)
        };
    }
}
