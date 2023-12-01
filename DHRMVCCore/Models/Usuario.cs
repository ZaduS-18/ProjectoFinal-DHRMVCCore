using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DHRMVCCore.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string PrimerNombre { get; set; } = null!;

    public string SegundoNombre { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string SegundoApellido { get; set; } = null!;

    public int Telefono { get; set; }

    public string Rut { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public bool Admin { get; set; }

    public string Contrasena { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public byte[]? FotoP { get; set; }

    public int? RolIdRol { get; set; }

    public bool Habilitado { get; set; }

    public decimal UsuarioId { get; set; }

    public virtual Rol? RolIdRolNavigation { get; set; }

    public virtual ICollection<UsuarioNotificacione> UsuarioNotificaciones { get; set; } = new List<UsuarioNotificacione>();

    public virtual ICollection<Actum> ActaIdActa { get; set; } = new List<Actum>();

    public virtual ICollection<Obra> ObraIdObras { get; set; } = new List<Obra>();

    [NotMapped]
    public bool MantenerActivo { get; set; }

    [NotMapped]
    public List<int> ObraIdObrasSeleccionadas { get; set; }
}
