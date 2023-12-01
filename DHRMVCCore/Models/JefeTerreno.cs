using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class JefeTerreno
{
    public int IdJefeterreno { get; set; }

    public string Nombre { get; set; } = null!;

    public int CumPlaSem { get; set; }

    public int AsiReuPla { get; set; }

    public int SupPerObra { get; set; }

    public int CumProgGen { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
