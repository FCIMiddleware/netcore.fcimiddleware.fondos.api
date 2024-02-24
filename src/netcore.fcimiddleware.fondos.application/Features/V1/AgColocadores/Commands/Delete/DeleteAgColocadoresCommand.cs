using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Delete
{
    public class DeleteAgColocadoresCommand : IRequest
    {
        public int Id { get; set; }
    }
}
