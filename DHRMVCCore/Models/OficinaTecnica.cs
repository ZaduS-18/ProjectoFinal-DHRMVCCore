using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class OficinaTecnica
{
    public int IdOfitec { get; set; }

    public string Nombre { get; set; } = null!;

    public int EntrEstPag { get; set; }

    public int BoleGar { get; set; }

    public int ConFirmEmpr { get; set; }

    public int Descuento { get; set; }

    public int DetaMotivo { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
