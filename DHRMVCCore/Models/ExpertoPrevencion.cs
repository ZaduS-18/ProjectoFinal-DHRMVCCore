using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class ExpertoPrevencion
{
    public int IdExpertoprev { get; set; }

    public string Nombre { get; set; } = null!;

    public int CuenContTrab { get; set; }

    public int CuenRegInterno { get; set; }

    public int CompRegInsec { get; set; }

    public int CuentaEpp { get; set; }

    public int IntroEspecifica { get; set; }

    public int Cumple100 { get; set; }

    public int CumpleIntrucciones { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
