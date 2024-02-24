﻿using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.domain
{
    public class SocDepositaria : BaseDomainModel
    {
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public ICollection<Fondo> Fondos { get; set; }
    }
}
