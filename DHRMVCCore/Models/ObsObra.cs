using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class ObsObra
{
    public int IdObservacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Compromisos { get; set; } = null!;

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
