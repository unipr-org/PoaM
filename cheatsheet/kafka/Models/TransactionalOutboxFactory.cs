using System.Text.Json;
using Utility.Kafka.Constants;
using Utility.Kafka.Messages.Concrete;

namespace Utility.Kafka.Models {
    public static class TransactionalOutboxFactory {
        public static RET CreateInsert<DTO, MODEL, RET>(DTO dto) where DTO : class, new() where MODEL : class where RET : AbstractTransactionalOutbox, new(){
            return Create<DTO, MODEL, RET>(dto, Operations.INSERT);
        }

        private static RET Create<DTO, MODEL, RET>(DTO dto, string operation) where DTO : class, new() where MODEL : class where RET : AbstractTransactionalOutbox, new(){
            OperationMessage<DTO> opMsg = new OperationMessage<DTO>() {
                Dto_ = dto,
                Operation_ = operation
            };
            opMsg.CheckMsg();

            return new RET(){
                Tabella = nameof(MODEL),
                Messaggio = JsonSerializer.Serialize(opMsg)
            };
        }
    }
}
