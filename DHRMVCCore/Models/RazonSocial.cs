using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class RazonSocial
{
    public int IdRazonSocial { get; set; }

    public string RutRs { get; set; } = null!;

    public string NombreRs { get; set; } = null!;

    public virtual ICollection<Actum> Acta { get; set; } = new List<Actum>();

    public virtual ICollection<Especialidad> EspecialidadIdEspecialidads { get; set; } = new List<Especialidad>();

    public virtual ICollection<Obra> ObraIdObras { get; set; } = new List<Obra>();
}
