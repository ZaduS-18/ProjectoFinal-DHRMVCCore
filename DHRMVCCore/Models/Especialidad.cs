using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class Especialidad
{
    public int IdEspecialidad { get; set; }

    public string NombreEsp { get; set; } = null!;

    public virtual ICollection<Actum> Acta { get; set; } = new List<Actum>();

    public virtual ICollection<RazonSocial> RazonSocialIdRazonSocials { get; set; } = new List<RazonSocial>();
}
