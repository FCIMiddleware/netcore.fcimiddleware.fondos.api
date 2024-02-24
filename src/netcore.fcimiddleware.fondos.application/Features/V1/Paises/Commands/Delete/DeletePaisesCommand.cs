using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Delete
{
    public class DeletePaisesCommand : IRequest
    {
        public int Id { get; set; }
    }
}
