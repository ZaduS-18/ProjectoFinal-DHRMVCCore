using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class AdministradorObra
{
    public int IdEncalidad { get; set; }

    public string Nombre { get; set; } = null!;

    public int IvaCancel { get; set; }

    public int PantImposicion { get; set; }

    public int LiqSueldo { get; set; }

    public int Finiquito { get; set; }

    public int CerAntecedente { get; set; }

    public int CerF301 { get; set; }

    public int LibAsist { get; set; }

    public string? Observacion { get; set; }

    public bool Firma { get; set; }

    public int? ActaIdActa { get; set; }

    public virtual Actum? ActaIdActaNavigation { get; set; }

    public virtual Actum? Actum { get; set; }
}
