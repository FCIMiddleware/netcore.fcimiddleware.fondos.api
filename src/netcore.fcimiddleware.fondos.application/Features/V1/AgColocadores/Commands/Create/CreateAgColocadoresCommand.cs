﻿using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Create
{
    public class CreateAgColocadoresCommand : IRequest<int>
    {
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
        public bool? IsSincronized { get; set; } = false;
    }
}
