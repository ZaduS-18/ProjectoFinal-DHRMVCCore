using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class UsuarioNotificacione
{
    public int IdNotificaciones { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public decimal? UsuarioUsuarioId { get; set; }

    public virtual Usuario? UsuarioUsuario { get; set; }
}
