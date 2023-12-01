using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class EspBim
{
    public int IdEspBim { get; set; }

    public string Nombre { get; set; } = null!;

    public int EjEeTt { get; set; }

    public int CumAvances { get; set; }

    public int IncuNorma { get; set; }

    public int InfProblema { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
