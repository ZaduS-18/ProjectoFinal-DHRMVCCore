using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class JefeBodega
{
    public int IdJefeBodega { get; set; }

    public string Nombre { get; set; } = null!;

    public int DescuentoInsumo { get; set; }

    public int EntregaCargos { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
