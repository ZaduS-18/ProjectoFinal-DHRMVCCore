using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DHRMVCCore.Models;

public partial class BdDhrContext : DbContext
{
    public string Conexion { get; }
    public BdDhrContext(string valor)
    {
        Conexion = valor;
    }

    public BdDhrContext(DbContextOptions<BdDhrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actum> Acta { get; set; }

    public virtual DbSet<AdministradorObra> AdministradorObras { get; set; }

    public virtual DbSet<EncargadoCalidad> EncargadoCalidads { get; set; }

    public virtual DbSet<EspBim> EspBims { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<ExpertoPrevencion> ExpertoPrevencions { get; set; }

    public virtual DbSet<FechaCierre> FechaCierres { get; set; }

    public virtual DbSet<JefeBodega> JefeBodegas { get; set; }

    public virtual DbSet<JefeTerreno> JefeTerrenos { get; set; }

    public virtual DbSet<Obra> Obras { get; set; }

    public virtual DbSet<ObsObra> ObsObras { get; set; }

    public virtual DbSet<OficinaTecnica> OficinaTecnicas { get; set; }

    public virtual DbSet<RazonSocial> RazonSocials { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioNotificacione> UsuarioNotificaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=srvdhr.database.windows.net;Database=bbdd_dhr;User Id=admindhr;Password=Pwddhr2023;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actum>(entity =>
        {
            entity.HasKey(e => e.IdActa).HasName("ACTA_PK");

            entity.ToTable("ACTA");

            entity.HasIndex(e => e.JefeTerrenoIdJefeterreno, "ACTA__IDX").IsUnique();

            entity.HasIndex(e => e.OficinaTecnicaIdOfitec, "ACTA__IDXv1").IsUnique();

            entity.HasIndex(e => e.ExpertoPrevencionIdExpertoprev, "ACTA__IDXv2").IsUnique();

            entity.HasIndex(e => e.EncargadoCalidadIdEncargadocali, "ACTA__IDXv3").IsUnique();

            entity.HasIndex(e => e.AdministradorObraIdEncalidad, "ACTA__IDXv4").IsUnique();

            entity.HasIndex(e => e.EspBimIdEspBim, "ACTA__IDXv5").IsUnique();

            entity.HasIndex(e => e.JefeBodegaIdJefeBodega, "ACTA__IDXv6").IsUnique();

            entity.HasIndex(e => e.ObsObrasIdObservacion, "ACTA__IDXv7").IsUnique();

            entity.Property(e => e.IdActa)
                .ValueGeneratedNever()
                .HasColumnName("ID_ACTA");
            entity.Property(e => e.AdministradorObraIdEncalidad).HasColumnName("ADMINISTRADOR_OBRA_ID_ENCALIDAD");
            entity.Property(e => e.EncargadoCalidadIdEncargadocali).HasColumnName("ENCARGADO_CALIDAD_ID_ENCARGADOCALI");
            entity.Property(e => e.EspBimIdEspBim).HasColumnName("ESP_BIM_ID_ESP_BIM");
            entity.Property(e => e.EspecialidadIdEspecialidad).HasColumnName("ESPECIALIDAD_ID_ESPECIALIDAD");
            entity.Property(e => e.EstadoActa).HasColumnName("ESTADO_ACTA");
            entity.Property(e => e.ExpertoPrevencionIdExpertoprev).HasColumnName("EXPERTO_PREVENCION_ID_EXPERTOPREV");
            entity.Property(e => e.FechaApro)
                .HasColumnType("date")
                .HasColumnName("FECHA_APRO");
            entity.Property(e => e.FechaPresentacion)
                .HasColumnType("date")
                .HasColumnName("FECHA_PRESENTACION");
            entity.Property(e => e.JefeBodegaIdJefeBodega).HasColumnName("JEFE_BODEGA_ID_JEFE_BODEGA");
            entity.Property(e => e.JefeTerrenoIdJefeterreno).HasColumnName("JEFE_TERRENO_ID_JEFETERRENO");
            entity.Property(e => e.NumPago).HasColumnName("NUM_PAGO");
            entity.Property(e => e.ObraIdObra).HasColumnName("OBRA_ID_OBRA");
            entity.Property(e => e.ObsObrasIdObservacion).HasColumnName("OBS_OBRAS_ID_OBSERVACION");
            entity.Property(e => e.OficinaTecnicaIdOfitec).HasColumnName("OFICINA_TECNICA_ID_OFITEC");
            entity.Property(e => e.RazonSocialIdRazonSocial).HasColumnName("RAZON_SOCIAL_ID_RAZON_SOCIAL");

            entity.HasOne(d => d.AdministradorObraIdEncalidadNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.AdministradorObraIdEncalidad)
                .HasConstraintName("ACTA_ADMINISTRADOR_OBRA_FK");

            entity.HasOne(d => d.EncargadoCalidadIdEncargadocaliNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.EncargadoCalidadIdEncargadocali)
                .HasConstraintName("ACTA_ENCARGADO_CALIDAD_FK");

            entity.HasOne(d => d.EspBimIdEspBimNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.EspBimIdEspBim)
                .HasConstraintName("ACTA_ESP_BIM_FK");

            entity.HasOne(d => d.EspecialidadIdEspecialidadNavigation).WithMany(p => p.Acta)
                .HasForeignKey(d => d.EspecialidadIdEspecialidad)
                .HasConstraintName("ACTA_ESPECIALIDAD_FK");

            entity.HasOne(d => d.ExpertoPrevencionIdExpertoprevNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.ExpertoPrevencionIdExpertoprev)
                .HasConstraintName("ACTA_EXPERTO_PREVENCION_FK");

            entity.HasOne(d => d.JefeBodegaIdJefeBodegaNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.JefeBodegaIdJefeBodega)
                .HasConstraintName("ACTA_JEFE_BODEGA_FK");

            entity.HasOne(d => d.JefeTerrenoIdJefeterrenoNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.JefeTerrenoIdJefeterreno)
                .HasConstraintName("ACTA_JEFE_TERRENO_FK");

            entity.HasOne(d => d.ObraIdObraNavigation).WithMany(p => p.Acta)
                .HasForeignKey(d => d.ObraIdObra)
                .HasConstraintName("ACTA_OBRA_FK");

            entity.HasOne(d => d.ObsObrasIdObservacionNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.ObsObrasIdObservacion)
                .HasConstraintName("ACTA_OBS_OBRAS_FK");

            entity.HasOne(d => d.OficinaTecnicaIdOfitecNavigation).WithOne(p => p.Actum)
                .HasForeignKey<Actum>(d => d.OficinaTecnicaIdOfitec)
                .HasConstraintName("ACTA_OFICINA_TECNICA_FK");

            entity.HasOne(d => d.RazonSocialIdRazonSocialNavigation).WithMany(p => p.Acta)
                .HasForeignKey(d => d.RazonSocialIdRazonSocial)
                .HasConstraintName("ACTA_RAZON_SOCIAL_FK");
        });

        modelBuilder.Entity<AdministradorObra>(entity =>
        {
            entity.HasKey(e => e.IdEncalidad).HasName("ADMINISTRADOR_OBRA_PK");

            entity.ToTable("ADMINISTRADOR_OBRA");

            entity.HasIndex(e => e.ActaIdActa, "ADMINISTRADOR_OBRA__IDX").IsUnique();

            entity.Property(e => e.IdEncalidad)
                .ValueGeneratedNever()
                .HasColumnName("ID_ENCALIDAD");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.CerAntecedente).HasColumnName("CER_ANTECEDENTE");
            entity.Property(e => e.CerF301).HasColumnName("CER_F30-1");
            entity.Property(e => e.Finiquito).HasColumnName("FINIQUITO");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.IvaCancel).HasColumnName("IVA_CANCEL");
            entity.Property(e => e.LibAsist).HasColumnName("LIB_ASIST");
            entity.Property(e => e.LiqSueldo).HasColumnName("LIQ_SUELDO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");
            entity.Property(e => e.PantImposicion).HasColumnName("PANT_IMPOSICION");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.AdministradorObra)
                .HasForeignKey<AdministradorObra>(d => d.ActaIdActa)
                .HasConstraintName("ADMINISTRADOR_OBRA_ACTA_FK");
        });

        modelBuilder.Entity<EncargadoCalidad>(entity =>
        {
            entity.HasKey(e => e.IdEncargadocali).HasName("ENCARGADO_CALIDAD_PK");

            entity.ToTable("ENCARGADO_CALIDAD");

            entity.HasIndex(e => e.ActaIdActa, "ENCARGADO_CALIDAD__IDX").IsUnique();

            entity.Property(e => e.IdEncargadocali)
                .ValueGeneratedNever()
                .HasColumnName("ID_ENCARGADOCALI");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.Calidad).HasColumnName("CALIDAD");
            entity.Property(e => e.CerEnsayo).HasColumnName("CER_ENSAYO");
            entity.Property(e => e.CumEspTec).HasColumnName("CUM_ESP_TEC");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");
            entity.Property(e => e.SupObra).HasColumnName("SUP_OBRA");
            entity.Property(e => e.TrabjProtocolo).HasColumnName("TRABJ_PROTOCOLO");
            entity.Property(e => e.TrabjTermOtr).HasColumnName("TRABJ_TERM_OTR");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.EncargadoCalidad)
                .HasForeignKey<EncargadoCalidad>(d => d.ActaIdActa)
                .HasConstraintName("ENCARGADO_CALIDAD_ACTA_FK");
        });

        modelBuilder.Entity<EspBim>(entity =>
        {
            entity.HasKey(e => e.IdEspBim).HasName("ESP_BIM_PK");

            entity.ToTable("ESP_BIM");

            entity.HasIndex(e => e.ActaIdActa, "ESP_BIM__IDX").IsUnique();

            entity.Property(e => e.IdEspBim)
                .ValueGeneratedNever()
                .HasColumnName("ID_ESP_BIM");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.CumAvances).HasColumnName("CUM_AVANCES");
            entity.Property(e => e.EjEeTt).HasColumnName("EJ_EE_TT");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.IncuNorma).HasColumnName("INCU_NORMA");
            entity.Property(e => e.InfProblema).HasColumnName("INF_PROBLEMA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.EspBim)
                .HasForeignKey<EspBim>(d => d.ActaIdActa)
                .HasConstraintName("ESP_BIM_ACTA_FK");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("ESPECIALIDAD_PK");

            entity.ToTable("ESPECIALIDAD");

            entity.Property(e => e.IdEspecialidad)
                .ValueGeneratedNever()
                .HasColumnName("ID_ESPECIALIDAD");
            entity.Property(e => e.NombreEsp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_ESP");

            entity.HasMany(d => d.RazonSocialIdRazonSocials).WithMany(p => p.EspecialidadIdEspecialidads)
                .UsingEntity<Dictionary<string, object>>(
                    "RazonSocialEspecialidad",
                    r => r.HasOne<RazonSocial>().WithMany()
                        .HasForeignKey("RazonSocialIdRazonSocial")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_5_RAZON_SOCIAL_FK"),
                    l => l.HasOne<Especialidad>().WithMany()
                        .HasForeignKey("EspecialidadIdEspecialidad")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_5_ESPECIALIDAD_FK"),
                    j =>
                    {
                        j.HasKey("EspecialidadIdEspecialidad", "RazonSocialIdRazonSocial").HasName("Relation_5_PK");
                        j.ToTable("RAZON-SOCIAL_ESPECIALIDAD");
                        j.IndexerProperty<int>("EspecialidadIdEspecialidad").HasColumnName("ESPECIALIDAD_ID_ESPECIALIDAD");
                        j.IndexerProperty<int>("RazonSocialIdRazonSocial").HasColumnName("RAZON_SOCIAL_ID_RAZON_SOCIAL");
                    });
        });

        modelBuilder.Entity<ExpertoPrevencion>(entity =>
        {
            entity.HasKey(e => e.IdExpertoprev).HasName("EXPERTO_PREVENCION_PK");

            entity.ToTable("EXPERTO_PREVENCION");

            entity.HasIndex(e => e.ActaIdActa, "EXPERTO_PREVENCION__IDX").IsUnique();

            entity.Property(e => e.IdExpertoprev)
                .ValueGeneratedNever()
                .HasColumnName("ID_EXPERTOPREV");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.CompRegInsec).HasColumnName("COMP_REG_INSEC");
            entity.Property(e => e.CuenContTrab).HasColumnName("CUEN_CONT_TRAB");
            entity.Property(e => e.CuenRegInterno).HasColumnName("CUEN_REG_INTERNO");
            entity.Property(e => e.CuentaEpp).HasColumnName("CUENTA_EPP");
            entity.Property(e => e.Cumple100).HasColumnName("CUMPLE_100");
            entity.Property(e => e.CumpleIntrucciones).HasColumnName("CUMPLE_INTRUCCIONES");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.IntroEspecifica).HasColumnName("INTRO_ESPECIFICA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.ExpertoPrevencion)
                .HasForeignKey<ExpertoPrevencion>(d => d.ActaIdActa)
                .HasConstraintName("EXPERTO_PREVENCION_ACTA_FK");
        });

        modelBuilder.Entity<FechaCierre>(entity =>
        {
            entity.HasKey(e => e.IdFCierre).HasName("FECHA_CIERRE_PK");

            entity.ToTable("FECHA_CIERRE");

            entity.Property(e => e.IdFCierre)
                .ValueGeneratedNever()
                .HasColumnName("ID_F-CIERRE");
            entity.Property(e => e.FCierre)
                .HasColumnType("date")
                .HasColumnName("F_CIERRE");
            entity.Property(e => e.FRecepcion)
                .HasColumnType("date")
                .HasColumnName("F_RECEPCION");
            entity.Property(e => e.TipPago).HasColumnName("TIP_PAGO");
        });

        modelBuilder.Entity<JefeBodega>(entity =>
        {
            entity.HasKey(e => e.IdJefeBodega).HasName("JEFE_BODEGA_PK");

            entity.ToTable("JEFE_BODEGA");

            entity.HasIndex(e => e.ActaIdActa, "JEFE_BODEGA__IDX").IsUnique();

            entity.Property(e => e.IdJefeBodega)
                .ValueGeneratedNever()
                .HasColumnName("ID_JEFE_BODEGA");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.DescuentoInsumo).HasColumnName("DESCUENTO_INSUMO");
            entity.Property(e => e.EntregaCargos).HasColumnName("ENTREGA_CARGOS");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.JefeBodega)
                .HasForeignKey<JefeBodega>(d => d.ActaIdActa)
                .HasConstraintName("JEFE_BODEGA_ACTA_FK");
        });

        modelBuilder.Entity<JefeTerreno>(entity =>
        {
            entity.HasKey(e => e.IdJefeterreno).HasName("JEFE_TERRENO_PK");

            entity.ToTable("JEFE_TERRENO");

            entity.HasIndex(e => e.ActaIdActa, "JEFE_TERRENO__IDX").IsUnique();

            entity.Property(e => e.IdJefeterreno)
                .ValueGeneratedNever()
                .HasColumnName("ID_JEFETERRENO");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.AsiReuPla).HasColumnName("ASI_REU_PLA");
            entity.Property(e => e.CumPlaSem).HasColumnName("CUM_PLA_SEM");
            entity.Property(e => e.CumProgGen).HasColumnName("CUM_PROG_GEN");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");
            entity.Property(e => e.SupPerObra).HasColumnName("SUP_PER_OBRA");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.JefeTerreno)
                .HasForeignKey<JefeTerreno>(d => d.ActaIdActa)
                .HasConstraintName("JEFE_TERRENO_ACTA_FK");
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasKey(e => e.IdObra).HasName("OBRA_PK");

            entity.ToTable("OBRA");

            entity.Property(e => e.IdObra)
                .ValueGeneratedNever()
                .HasColumnName("ID_OBRA");
            entity.Property(e => e.NombreObra)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_OBRA");

            entity.HasMany(d => d.UsuarioUsuarios).WithMany(p => p.ObraIdObras)
                .UsingEntity<Dictionary<string, object>>(
                    "UsObra",
                    r => r.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioUsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_26_USUARIO_FK"),
                    l => l.HasOne<Obra>().WithMany()
                        .HasForeignKey("ObraIdObra")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_26_OBRA_FK"),
                    j =>
                    {
                        j.HasKey("ObraIdObra", "UsuarioUsuarioId").HasName("Relation_26_PK");
                        j.ToTable("US_OBRA");
                        j.IndexerProperty<int>("ObraIdObra").HasColumnName("OBRA_ID_OBRA");
                        j.IndexerProperty<decimal>("UsuarioUsuarioId")
                            .HasColumnType("numeric(28, 0)")
                            .HasColumnName("USUARIO_USUARIO_ID");
                    });
        });

        modelBuilder.Entity<ObsObra>(entity =>
        {
            entity.HasKey(e => e.IdObservacion).HasName("OBS_OBRAS_PK");

            entity.ToTable("OBS_OBRAS");

            entity.HasIndex(e => e.ActaIdActa, "OBS_OBRAS__IDX").IsUnique();

            entity.Property(e => e.IdObservacion)
                .ValueGeneratedNever()
                .HasColumnName("ID_OBSERVACION");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.Compromisos)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("COMPROMISOS");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.ObsObra)
                .HasForeignKey<ObsObra>(d => d.ActaIdActa)
                .HasConstraintName("OBS_OBRAS_ACTA_FK");
        });

        modelBuilder.Entity<OficinaTecnica>(entity =>
        {
            entity.HasKey(e => e.IdOfitec).HasName("OFICINA_TECNICA_PK");

            entity.ToTable("OFICINA_TECNICA");

            entity.HasIndex(e => e.ActaIdActa, "OFICINA_TECNICA__IDX").IsUnique();

            entity.Property(e => e.IdOfitec)
                .ValueGeneratedNever()
                .HasColumnName("ID_OFITEC");
            entity.Property(e => e.ActaIdActa).HasColumnName("ACTA_ID_ACTA");
            entity.Property(e => e.BoleGar).HasColumnName("BOLE_GAR");
            entity.Property(e => e.ConFirmEmpr).HasColumnName("CON_FIRM_EMPR");
            entity.Property(e => e.Descuento).HasColumnName("DESCUENTO");
            entity.Property(e => e.DetaMotivo).HasColumnName("DETA_MOTIVO");
            entity.Property(e => e.EntrEstPag).HasColumnName("ENTR_EST_PAG");
            entity.Property(e => e.Firma).HasColumnName("FIRMA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Observacion)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("OBSERVACION");

            entity.HasOne(d => d.ActaIdActaNavigation).WithOne(p => p.OficinaTecnica)
                .HasForeignKey<OficinaTecnica>(d => d.ActaIdActa)
                .HasConstraintName("OFICINA_TECNICA_ACTA_FK");
        });

        modelBuilder.Entity<RazonSocial>(entity =>
        {
            entity.HasKey(e => e.IdRazonSocial).HasName("RAZON_SOCIAL_PK");

            entity.ToTable("RAZON_SOCIAL");

            entity.Property(e => e.IdRazonSocial)
                .ValueGeneratedNever()
                .HasColumnName("ID_RAZON_SOCIAL");
            entity.Property(e => e.NombreRs)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_RS");
            entity.Property(e => e.RutRs)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("RUT_RS");

            entity.HasMany(d => d.ObraIdObras).WithMany(p => p.RazonSocialIdRazonSocials)
                .UsingEntity<Dictionary<string, object>>(
                    "ObraRazonSocial",
                    r => r.HasOne<Obra>().WithMany()
                        .HasForeignKey("ObraIdObra")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_6_OBRA_FK"),
                    l => l.HasOne<RazonSocial>().WithMany()
                        .HasForeignKey("RazonSocialIdRazonSocial")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_6_RAZON_SOCIAL_FK"),
                    j =>
                    {
                        j.HasKey("RazonSocialIdRazonSocial", "ObraIdObra").HasName("Relation_6_PK");
                        j.ToTable("OBRA_RAZON-SOCIAL");
                        j.IndexerProperty<int>("RazonSocialIdRazonSocial").HasColumnName("RAZON_SOCIAL_ID_RAZON_SOCIAL");
                        j.IndexerProperty<int>("ObraIdObra").HasColumnName("OBRA_ID_OBRA");
                    });
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("ROL_PK");

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol)
                .ValueGeneratedNever()
                .HasColumnName("ID_ROL");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_ROL");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("USUARIO_PK");

            entity.ToTable("USUARIO");

            entity.Property(e => e.UsuarioId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(28, 0)")
                .HasColumnName("USUARIO_ID");
            entity.Property(e => e.Admin).HasColumnName("ADMIN");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("CONTRASENA");
            entity.Property(e => e.Correo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CORREO");
            entity.Property(e => e.Direccion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.FotoP)
                .HasColumnType("image")
                .HasColumnName("FOTO_P");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PRIMER_APELLIDO");
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PRIMER_NOMBRE");
            entity.Property(e => e.RolIdRol).HasColumnName("ROL_ID_ROL");
            entity.Property(e => e.Rut)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("RUT");
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SEGUNDO_APELLIDO");
            entity.Property(e => e.SegundoNombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SEGUNDO_NOMBRE");
            entity.Property(e => e.Telefono).HasColumnName("TELEFONO");

            entity.HasOne(d => d.RolIdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolIdRol)
                .HasConstraintName("USUARIO_ROL_FK");

            entity.HasMany(d => d.ActaIdActa).WithMany(p => p.UsuarioUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsActum",
                    r => r.HasOne<Actum>().WithMany()
                        .HasForeignKey("ActaIdActa")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_8_ACTA_FK"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioUsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Relation_8_USUARIO_FK"),
                    j =>
                    {
                        j.HasKey("UsuarioUsuarioId", "ActaIdActa").HasName("Relation_8_PK");
                        j.ToTable("US_ACTA");
                        j.IndexerProperty<decimal>("UsuarioUsuarioId")
                            .HasColumnType("numeric(28, 0)")
                            .HasColumnName("USUARIO_USUARIO_ID");
                        j.IndexerProperty<int>("ActaIdActa").HasColumnName("ACTA_ID_ACTA");
                    });
        });

        modelBuilder.Entity<UsuarioNotificacione>(entity =>
        {
            entity.HasKey(e => e.IdNotificaciones).HasName("USUARIO_NOTIFICACIONES_PK");

            entity.ToTable("USUARIO_NOTIFICACIONES");

            entity.Property(e => e.IdNotificaciones)
                .ValueGeneratedNever()
                .HasColumnName("ID_NOTIFICACIONES");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("FECHA");
            entity.Property(e => e.UsuarioUsuarioId)
                .HasColumnType("numeric(28, 0)")
                .HasColumnName("USUARIO_USUARIO_ID");

            entity.HasOne(d => d.UsuarioUsuario).WithMany(p => p.UsuarioNotificaciones)
                .HasForeignKey(d => d.UsuarioUsuarioId)
                .HasConstraintName("USUARIO_NOTIFICACIONES_USUARIO_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
