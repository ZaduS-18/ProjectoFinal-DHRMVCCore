using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class Actum
{
    public int IdActa { get; set; }

    public DateTime FechaPresentacion { get; set; }

    public int EstadoActa { get; set; }

    public DateTime FechaApro { get; set; }

    public int? JefeTerrenoIdJefeterreno { get; set; }

    public int? OficinaTecnicaIdOfitec { get; set; }

    public int? ExpertoPrevencionIdExpertoprev { get; set; }

    public int? EncargadoCalidadIdEncargadocali { get; set; }

    public int? AdministradorObraIdEncalidad { get; set; }

    public int? EspBimIdEspBim { get; set; }

    public int? JefeBodegaIdJefeBodega { get; set; }

    public int? ObsObrasIdObservacion { get; set; }

    public int? ObraIdObra { get; set; }

    public int? RazonSocialIdRazonSocial { get; set; }

    public int? EspecialidadIdEspecialidad { get; set; }

    public int NumPago { get; set; }

    public virtual AdministradorObra? AdministradorObra { get; set; }

    public virtual AdministradorObra? AdministradorObraIdEncalidadNavigation { get; set; }

    public virtual EncargadoCalidad? EncargadoCalidad { get; set; }

    public virtual EncargadoCalidad? EncargadoCalidadIdEncargadocaliNavigation { get; set; }

    public virtual EspBim? EspBim { get; set; }

    public virtual EspBim? EspBimIdEspBimNavigation { get; set; }

    public virtual Especialidad? EspecialidadIdEspecialidadNavigation { get; set; }

    public virtual ExpertoPrevencion? ExpertoPrevencion { get; set; }

    public virtual ExpertoPrevencion? ExpertoPrevencionIdExpertoprevNavigation { get; set; }

    public virtual JefeBodega? JefeBodega { get; set; }

    public virtual JefeBodega? JefeBodegaIdJefeBodegaNavigation { get; set; }

    public virtual JefeTerreno? JefeTerreno { get; set; }

    public virtual JefeTerreno? JefeTerrenoIdJefeterrenoNavigation { get; set; }

    public virtual Obra? ObraIdObraNavigation { get; set; }

    public virtual ObsObra? ObsObra { get; set; }

    public virtual ObsObra? ObsObrasIdObservacionNavigation { get; set; }

    public virtual OficinaTecnica? OficinaTecnica { get; set; }

    public virtual OficinaTecnica? OficinaTecnicaIdOfitecNavigation { get; set; }

    public virtual RazonSocial? RazonSocialIdRazonSocialNavigation { get; set; }

    public virtual ICollection<Usuario> UsuarioUsuarios { get; set; } = new List<Usuario>();
}
