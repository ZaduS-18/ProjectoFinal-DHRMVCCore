using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class Obra
{
    public int IdObra { get; set; }

    public string NombreObra { get; set; } = null!;

    public virtual ICollection<Actum> Acta { get; set; } = new List<Actum>();

    public virtual ICollection<RazonSocial> RazonSocialIdRazonSocials { get; set; } = new List<RazonSocial>();

    public virtual ICollection<Usuario> UsuarioUsuarios { get; set; } = new List<Usuario>();
}
