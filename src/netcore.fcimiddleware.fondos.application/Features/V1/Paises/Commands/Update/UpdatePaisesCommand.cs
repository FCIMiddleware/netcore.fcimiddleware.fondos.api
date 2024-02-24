﻿using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Update
{
    public class UpdatePaisesCommand : IRequest
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
    }
}
