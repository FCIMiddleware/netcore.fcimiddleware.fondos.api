using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Delete
{
    public class DeleteSocDepositariasCommand : IRequest
    {
        public int Id { get; set; }
    }
}
