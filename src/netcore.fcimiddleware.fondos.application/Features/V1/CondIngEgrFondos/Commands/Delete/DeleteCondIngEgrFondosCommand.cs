using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Delete
{
    public class DeleteCondIngEgrFondosCommand : IRequest
    {
        public int Id { get; set; }
    }
}
