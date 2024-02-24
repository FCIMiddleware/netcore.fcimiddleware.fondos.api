using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Delete
{
    public class DeleteFondosCommand : IRequest
    {
        public int Id { get; set; }
    }
}