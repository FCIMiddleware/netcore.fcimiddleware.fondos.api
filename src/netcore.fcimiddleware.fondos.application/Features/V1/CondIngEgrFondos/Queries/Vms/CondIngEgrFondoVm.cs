﻿using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.Vms
{
    public class CondIngEgrFondoVm
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public int FondoId { get; set; }
        public Fondo Fondo { get; set; }
        public ICollection<CondIngEgrFondo> CondIngEgrFondos { get; set; }
    }
}
