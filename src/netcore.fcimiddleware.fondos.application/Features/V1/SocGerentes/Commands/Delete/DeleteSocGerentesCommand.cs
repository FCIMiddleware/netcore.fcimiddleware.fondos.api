using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Delete
{
    public class DeleteSocGerentesCommand : IRequest
    {
        public int Id { get; set; }
    }
}
