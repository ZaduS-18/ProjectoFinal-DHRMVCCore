using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class EncargadoCalidad
{
    public int IdEncargadocali { get; set; }

    public string Nombre { get; set; } = null!;

    public int SupObra { get; set; }

    public int CumEspTec { get; set; }

    public int TrabjProtocolo { get; set; }

    public int CerEnsayo { get; set; }

    public int Calidad { get; set; }

    public int TrabjTermOtr { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
