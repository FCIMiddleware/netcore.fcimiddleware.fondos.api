using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Delete
{
    public class DeleteMonedasCommand : IRequest
    {
        public int Id { get; set; }
    }
}
