using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Delete
{
    public class DeleteTpValorCptFondosCommand : IRequest
    {
        public int Id { get; set; }
    }
}
