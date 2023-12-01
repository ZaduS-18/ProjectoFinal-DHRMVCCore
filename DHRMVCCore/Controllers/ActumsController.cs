using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DHRMVCCore.Models;
using Microsoft.AspNetCore.Authorization;
using PuppeteerSharp;
using System.Threading.Tasks;
using PuppeteerSharp.Media;


namespace DHRMVCCore.Controllers
{
   
    public class ActumsController : Controller
    {
        private readonly BdDhrContext _context;

        public ActumsController(BdDhrContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> ListarActas()
        {
            var actumsList = await _context.Acta
                .Include(a => a.AdministradorObraIdEncalidadNavigation)
                .Include(a => a.EncargadoCalidadIdEncargadocaliNavigation)
                .Include(a => a.EspBimIdEspBimNavigation)
                .Include(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(a => a.ExpertoPrevencionIdExpertoprevNavigation)
                .Include(a => a.JefeBodegaIdJefeBodegaNavigation)
                .Include(a => a.JefeTerrenoIdJefeterrenoNavigation)
                .Include(a => a.ObraIdObraNavigation)
                .ThenInclude(o => o.UsuarioUsuarios)
                .Include(a => a.ObsObrasIdObservacionNavigation)
                .Include(a => a.OficinaTecnicaIdOfitecNavigation)
                .Include(a => a.RazonSocialIdRazonSocialNavigation)
                .ToListAsync();

            var singleActum = actumsList.FirstOrDefault(); // o cualquier otro método para obtener un Actum individual

            var model = new Tuple<IEnumerable<Actum>, Actum>(actumsList, singleActum);
            


            return View(model);
        }

        public interface IActasService
        {
            Task<Tuple<IEnumerable<Actum>, Actum>> ListarActasAsync();
        }
        // GET: Actums
        public async Task<IActionResult> Index()
        {
            var bdDhrContext = _context.Acta.Include(a => a.AdministradorObraIdEncalidadNavigation).Include(a => a.EncargadoCalidadIdEncargadocaliNavigation).Include(a => a.EspBimIdEspBimNavigation).Include(a => a.EspecialidadIdEspecialidadNavigation).Include(a => a.ExpertoPrevencionIdExpertoprevNavigation).Include(a => a.JefeBodegaIdJefeBodegaNavigation).Include(a => a.JefeTerrenoIdJefeterrenoNavigation).Include(a => a.ObraIdObraNavigation).Include(a => a.ObsObrasIdObservacionNavigation).Include(a => a.OficinaTecnicaIdOfitecNavigation).Include(a => a.RazonSocialIdRazonSocialNavigation);
            return View(await bdDhrContext.ToListAsync());
        }

        // GET: Actums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Acta == null)
            {
                return NotFound();
            }

            var actum = await _context.Acta
                .Include(a => a.AdministradorObraIdEncalidadNavigation)
                .Include(a => a.EncargadoCalidadIdEncargadocaliNavigation)
                .Include(a => a.EspBimIdEspBimNavigation)
                .Include(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(a => a.ExpertoPrevencionIdExpertoprevNavigation)
                .Include(a => a.JefeBodegaIdJefeBodegaNavigation)
                .Include(a => a.JefeTerrenoIdJefeterrenoNavigation)
                .Include(a => a.ObraIdObraNavigation)
                .Include(a => a.ObsObrasIdObservacionNavigation)
                .Include(a => a.OficinaTecnicaIdOfitecNavigation)
                .Include(a => a.RazonSocialIdRazonSocialNavigation)
                .FirstOrDefaultAsync(m => m.IdActa == id);
            if (actum == null)
            {
                return NotFound();
            }

            return View(actum);
        }

        // GET: Actums/Create
        public IActionResult Create()
        {
            ViewData["AdministradorObraIdEncalidad"] = new SelectList(_context.AdministradorObras, "IdEncalidad", "IdEncalidad");
            ViewData["EncargadoCalidadIdEncargadocali"] = new SelectList(_context.EncargadoCalidads, "IdEncargadocali", "IdEncargadocali");
            ViewData["EspBimIdEspBim"] = new SelectList(_context.EspBims, "IdEspBim", "IdEspBim");
            ViewData["EspecialidadIdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "NombreEsp");
            ViewData["ExpertoPrevencionIdExpertoprev"] = new SelectList(_context.ExpertoPrevencions, "IdExpertoprev", "IdExpertoprev");
            ViewData["JefeBodegaIdJefeBodega"] = new SelectList(_context.JefeBodegas, "IdJefeBodega", "IdJefeBodega");
            ViewData["JefeTerrenoIdJefeterreno"] = new SelectList(_context.JefeTerrenos, "IdJefeterreno", "IdJefeterreno");
            ViewData["ObraIdObra"] = new SelectList(_context.Obras, "IdObra", "NombreObra");
            ViewData["ObsObrasIdObservacion"] = new SelectList(_context.ObsObras, "IdObservacion", "IdObservacion");
            ViewData["OficinaTecnicaIdOfitec"] = new SelectList(_context.OficinaTecnicas, "IdOfitec", "IdOfitec");
            ViewData["RazonSocialIdRazonSocial"] = new SelectList(_context.RazonSocials, "IdRazonSocial", "NombreRs");
            return View();
        }

        public async Task<IActionResult> CrearActa([Bind("IdActa,FechaPresentacion,EstadoActa,FechaApro,JefeTerrenoIdJefeterreno,OficinaTecnicaIdOfitec,ExpertoPrevencionIdExpertoprev,EncargadoCalidadIdEncargadocali,AdministradorObraIdEncalidad,EspBimIdEspBim,JefeBodegaIdJefeBodega,ObsObrasIdObservacion,ObraIdObra,RazonSocialIdRazonSocial,EspecialidadIdEspecialidad")] Actum actum)
        {
            if (ModelState.IsValid)
            {
                // Verifica si hay elementos en la tabla Actas
                if (await _context.Acta.AnyAsync())
                {
                    // Si hay, obten el máximo ID y agrégale 1
                    var nextId = await _context.Acta.MaxAsync(a => a.IdActa) + 1;
                    actum.IdActa = nextId;
                }
                else
                {
                    // Si no hay registros, asigna 1 como el primer ID
                    actum.IdActa = 1;
                }

                _context.Add(actum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListarActas));
            }
            ViewData["AdministradorObraIdEncalidad"] = new SelectList(_context.AdministradorObras, "IdEncalidad", "IdEncalidad", actum.AdministradorObraIdEncalidad);
            ViewData["EncargadoCalidadIdEncargadocali"] = new SelectList(_context.EncargadoCalidads, "IdEncargadocali", "IdEncargadocali", actum.EncargadoCalidadIdEncargadocali);
            ViewData["EspBimIdEspBim"] = new SelectList(_context.EspBims, "IdEspBim", "IdEspBim", actum.EspBimIdEspBim);
            ViewData["EspecialidadIdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "IdEspecialidad", actum.EspecialidadIdEspecialidad);
            ViewData["ExpertoPrevencionIdExpertoprev"] = new SelectList(_context.ExpertoPrevencions, "IdExpertoprev", "IdExpertoprev", actum.ExpertoPrevencionIdExpertoprev);
            ViewData["JefeBodegaIdJefeBodega"] = new SelectList(_context.JefeBodegas, "IdJefeBodega", "IdJefeBodega", actum.JefeBodegaIdJefeBodega);
            ViewData["JefeTerrenoIdJefeterreno"] = new SelectList(_context.JefeTerrenos, "IdJefeterreno", "IdJefeterreno", actum.JefeTerrenoIdJefeterreno);
            ViewData["ObraIdObra"] = new SelectList(_context.Obras, "IdObra", "IdObra", actum.ObraIdObra);
            ViewData["ObsObrasIdObservacion"] = new SelectList(_context.ObsObras, "IdObservacion", "IdObservacion", actum.ObsObrasIdObservacion);
            ViewData["OficinaTecnicaIdOfitec"] = new SelectList(_context.OficinaTecnicas, "IdOfitec", "IdOfitec", actum.OficinaTecnicaIdOfitec);
            ViewData["RazonSocialIdRazonSocial"] = new SelectList(_context.RazonSocials, "IdRazonSocial", "IdRazonSocial", actum.RazonSocialIdRazonSocial);
            return View(actum);
        }

        // POST: Actums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdActa,FechaPresentacion,EstadoActa,FechaApro,JefeTerrenoIdJefeterreno,OficinaTecnicaIdOfitec,ExpertoPrevencionIdExpertoprev,EncargadoCalidadIdEncargadocali,AdministradorObraIdEncalidad,EspBimIdEspBim,JefeBodegaIdJefeBodega,ObsObrasIdObservacion,ObraIdObra,RazonSocialIdRazonSocial,EspecialidadIdEspecialidad,NumPago")] Actum actum)
        {
            if (ModelState.IsValid)
            {
                actum.FechaPresentacion = DateTime.Now;
                // Verifica si hay elementos en la tabla Actas
                if (await _context.Acta.AnyAsync())
                {

                    // Si hay, obten el máximo ID y agrégale 1
                    var nextId = await _context.Acta.MaxAsync(a => a.IdActa) + 1;
                    actum.IdActa = nextId;
                }
                else
                {
                    // Si no hay registros, asigna 1 como el primer ID
                    actum.IdActa = 1;
                }

                _context.Add(actum);
                await _context.SaveChangesAsync();
                // Crear un nuevo JefeTerreno y asociarlo al Actum recién creado
                JefeTerreno newJefeTerreno = new JefeTerreno
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:

                    IdJefeterreno = actum.IdActa,

                    Nombre = "Nombre del Jefe",
                    CumPlaSem = 0,

                    AsiReuPla = 0,

                    SupPerObra = 0,

                    CumProgGen = 0,

                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newJefeTerreno);
                await _context.SaveChangesAsync();

                // Asignar el ID del JefeTerreno recién creado al Actum
                actum.JefeTerrenoIdJefeterreno = newJefeTerreno.IdJefeterreno;

                //-------------------------------------
                AdministradorObra newAdministradoObra = new AdministradorObra
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:
                    IdEncalidad = actum.IdActa,

                    Nombre = "Nombre del Aadministrador",
                    IvaCancel = 0,

                    PantImposicion = 0,

                    Finiquito = 0,

                    LiqSueldo = 0,

                    CerAntecedente = 0,

                    CerF301 = 0,

                    LibAsist = 0,

                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newAdministradoObra);
                await _context.SaveChangesAsync();
                //---------------------------------------

                EncargadoCalidad newEncargadoCalidad = new EncargadoCalidad
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:
                    IdEncargadocali = actum.IdActa,

                    Nombre = "Nombre del Encargado Calidad",
                    SupObra = 0,

                    CumEspTec = 0,

                    TrabjProtocolo = 0,

                    CerEnsayo = 0,

                    Calidad = 0,

                    TrabjTermOtr = 0,

                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newEncargadoCalidad);
                await _context.SaveChangesAsync();
                //---------------------------------------

                EspBim newEspBim = new EspBim
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:
                    IdEspBim = actum.IdActa,

                    Nombre = "Nombre del EspBim",
                    CumAvances = 0,

                    IncuNorma = 0,

                    InfProblema = 0,

                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newEspBim);
                await _context.SaveChangesAsync();

                ExpertoPrevencion newExpertoPrevencion = new ExpertoPrevencion
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:
                    IdExpertoprev = actum.IdActa,

                    Nombre = "Nombre del ExpertoPrevencion",
                    CuenContTrab = 0,

                    CuenRegInterno = 0,

                    CompRegInsec = 0,

                    IntroEspecifica = 0,

                    Cumple100 = 0,

                    CumpleIntrucciones = 0,


                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newExpertoPrevencion);
                await _context.SaveChangesAsync();

                JefeBodega newJefeBodega = new JefeBodega
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:

                    IdJefeBodega = actum.IdActa,
                    Nombre = "Nombre del JefeBodega",
                    DescuentoInsumo = 0,

                    EntregaCargos = 0,


                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newJefeBodega);
                await _context.SaveChangesAsync();

                ObsObra newObsObra = new ObsObra
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:
                    IdObservacion = actum.IdActa,

                    Nombre = "Nombre del Obs Obra",
                    Compromisos = "Compromisos",



                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newObsObra);
                await _context.SaveChangesAsync();


                OficinaTecnica newOficinaTecnica = new OficinaTecnica
                {
                    // Aquí puedes asignar los valores que desees para las propiedades de JefeTerreno
                    // Por ejemplo:
                    IdOfitec = actum.IdActa,

                    Nombre = "Nombre del OficinaTecnica",

                    EntrEstPag = 0,

                    BoleGar = 0,

                    ConFirmEmpr = 0,

                    Descuento = 0,

                    DetaMotivo = 0,


                    Firma = false,// Puedes cambiar esto según lo que necesites
                    ActaIdActa = actum.IdActa // Asociar el JefeTerreno al Actum
                };

                _context.Add(newOficinaTecnica);
                await _context.SaveChangesAsync();


                // Asignar el ID del JefeTerreno recién creado al Actum
                actum.OficinaTecnicaIdOfitec = newOficinaTecnica.IdOfitec;
                actum.ObsObrasIdObservacion = newObsObra.IdObservacion;
                actum.JefeBodegaIdJefeBodega = newJefeBodega.IdJefeBodega;
                actum.ExpertoPrevencionIdExpertoprev = newExpertoPrevencion.IdExpertoprev;
                actum.EspBimIdEspBim = newEspBim.IdEspBim;
                actum.EncargadoCalidadIdEncargadocali = newEncargadoCalidad.IdEncargadocali;
                actum.AdministradorObraIdEncalidad = newAdministradoObra.IdEncalidad;
                actum.JefeTerrenoIdJefeterreno = newJefeTerreno.IdJefeterreno;
                _context.Update(actum);
                await _context.SaveChangesAsync();

                // Obtén la obra relacionada con el acta
                var obra = await _context.Obras
                                         .Include(o => o.UsuarioUsuarios) // Asegúrate de que tienes una propiedad para los usuarios asociados
                                         .FirstOrDefaultAsync(o => o.IdObra == actum.ObraIdObra);

                if (obra != null)
                {
                    int nextNotificationId = 1; // Valor inicial para el ID de la notificación
                    if (await _context.UsuarioNotificaciones.AnyAsync())
                    {
                        // Si hay notificaciones, obtén el máximo ID y súmale 1
                        nextNotificationId = await _context.UsuarioNotificaciones.MaxAsync(n => n.IdNotificaciones) + 1;
                    }

                    foreach (var usuario in obra.UsuarioUsuarios)
                    {
                        // Verifica si el usuario existe en la base de datos
                        var usuarioExistente = await _context.Usuarios
                                                             .AnyAsync(u => u.UsuarioId == usuario.UsuarioId);
                        if (usuarioExistente)
                        {
                            UsuarioNotificacione notificacion = new UsuarioNotificacione
                            {
                                IdNotificaciones = nextNotificationId++, // Asigna el ID y luego incrementa
                                Descripcion = "Nueva acta N"+ actum.IdActa +" creada para la obra: " + obra.NombreObra,
                                Fecha = DateTime.Now,
                                UsuarioUsuarioId = usuario.UsuarioId
                            };

                            _context.UsuarioNotificaciones.Add(notificacion);
                        }
                    }
                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(ListarActas));
            }
            ViewData["AdministradorObraIdEncalidad"] = new SelectList(_context.AdministradorObras, "IdEncalidad", "IdEncalidad", actum.AdministradorObraIdEncalidad);
            ViewData["EncargadoCalidadIdEncargadocali"] = new SelectList(_context.EncargadoCalidads, "IdEncargadocali", "IdEncargadocali", actum.EncargadoCalidadIdEncargadocali);
            ViewData["EspBimIdEspBim"] = new SelectList(_context.EspBims, "IdEspBim", "IdEspBim", actum.EspBimIdEspBim);
            ViewData["EspecialidadIdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "NombreEsp", actum.EspecialidadIdEspecialidad);
            ViewData["ExpertoPrevencionIdExpertoprev"] = new SelectList(_context.ExpertoPrevencions, "IdExpertoprev", "IdExpertoprev", actum.ExpertoPrevencionIdExpertoprev);
            ViewData["JefeBodegaIdJefeBodega"] = new SelectList(_context.JefeBodegas, "IdJefeBodega", "IdJefeBodega", actum.JefeBodegaIdJefeBodega);
            ViewData["JefeTerrenoIdJefeterreno"] = new SelectList(_context.JefeTerrenos, "IdJefeterreno", "IdJefeterreno", actum.JefeTerrenoIdJefeterreno);
            ViewData["ObraIdObra"] = new SelectList(_context.Obras, "IdObra", "NombreObra", actum.ObraIdObra);
            ViewData["ObsObrasIdObservacion"] = new SelectList(_context.ObsObras, "IdObservacion", "IdObservacion", actum.ObsObrasIdObservacion);
            ViewData["OficinaTecnicaIdOfitec"] = new SelectList(_context.OficinaTecnicas, "IdOfitec", "IdOfitec", actum.OficinaTecnicaIdOfitec);
            ViewData["RazonSocialIdRazonSocial"] = new SelectList(_context.RazonSocials, "IdRazonSocial", "NombreRs", actum.RazonSocialIdRazonSocial);
            return View(actum);
        }

        // GET: Actums/Edit/5
        // GET: Actums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Acta == null)
            {
                return NotFound();
            }

            var actum = await _context.Acta.FindAsync(id);
            if (actum == null)
            {
                return NotFound();
            }

            // Crea una lista de opciones para el campo select
            var estadoActaOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "Proceso" },
        new SelectListItem { Value = "1", Text = "Finalizado" },
        new SelectListItem { Value = "2", Text = "Expirado" }
    };

            // Establece el valor seleccionado en función de la propiedad EstadoActa
            foreach (var item in estadoActaOptions)
            {
                if (item.Value == actum.EstadoActa.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

            ViewData["EstadoActaOptions"] = estadoActaOptions;

            ViewData["AdministradorObraIdEncalidad"] = new SelectList(_context.AdministradorObras, "IdEncalidad", "IdEncalidad", actum.AdministradorObraIdEncalidad);
            ViewData["EncargadoCalidadIdEncargadocali"] = new SelectList(_context.EncargadoCalidads, "IdEncargadocali", "IdEncargadocali", actum.EncargadoCalidadIdEncargadocali);
            ViewData["EspBimIdEspBim"] = new SelectList(_context.EspBims, "IdEspBim", "IdEspBim", actum.EspBimIdEspBim);
            ViewData["EspecialidadIdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "NombreEsp", actum.EspecialidadIdEspecialidad);
            ViewData["ExpertoPrevencionIdExpertoprev"] = new SelectList(_context.ExpertoPrevencions, "IdExpertoprev", "IdExpertoprev", actum.ExpertoPrevencionIdExpertoprev);
            ViewData["JefeBodegaIdJefeBodega"] = new SelectList(_context.JefeBodegas, "IdJefeBodega", "IdJefeBodega", actum.JefeBodegaIdJefeBodega);
            ViewData["JefeTerrenoIdJefeterreno"] = new SelectList(_context.JefeTerrenos, "IdJefeterreno", "IdJefeterreno", actum.JefeTerrenoIdJefeterreno);
            ViewData["ObraIdObra"] = new SelectList(_context.Obras, "IdObra", "NombreObra", actum.ObraIdObra);
            ViewData["ObsObrasIdObservacion"] = new SelectList(_context.ObsObras, "IdObservacion", "IdObservacion", actum.ObsObrasIdObservacion);
            ViewData["OficinaTecnicaIdOfitec"] = new SelectList(_context.OficinaTecnicas, "IdOfitec", "IdOfitec", actum.OficinaTecnicaIdOfitec);
            ViewData["RazonSocialIdRazonSocial"] = new SelectList(_context.RazonSocials, "IdRazonSocial", "NombreRs", actum.RazonSocialIdRazonSocial);

            return View(actum);
        }


        // POST: Actums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("IdActa,FechaPresentacion,EstadoActa,FechaApro,JefeTerrenoIdJefeterreno,OficinaTecnicaIdOfitec,ExpertoPrevencionIdExpertoprev,EncargadoCalidadIdEncargadocali,AdministradorObraIdEncalidad,EspBimIdEspBim,JefeBodegaIdJefeBodega,ObsObrasIdObservacion,ObraIdObra,RazonSocialIdRazonSocial,EspecialidadIdEspecialidad,NumPago")] Actum actum)
        {
            if (id != actum.IdActa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActumExists(actum.IdActa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarActas));
            }
            ViewData["AdministradorObraIdEncalidad"] = new SelectList(_context.AdministradorObras, "IdEncalidad", "IdEncalidad", actum.AdministradorObraIdEncalidad);
            ViewData["EncargadoCalidadIdEncargadocali"] = new SelectList(_context.EncargadoCalidads, "IdEncargadocali", "IdEncargadocali", actum.EncargadoCalidadIdEncargadocali);
            ViewData["EspBimIdEspBim"] = new SelectList(_context.EspBims, "IdEspBim", "IdEspBim", actum.EspBimIdEspBim);
            ViewData["EspecialidadIdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "IdEspecialidad", actum.EspecialidadIdEspecialidad);
            ViewData["ExpertoPrevencionIdExpertoprev"] = new SelectList(_context.ExpertoPrevencions, "IdExpertoprev", "IdExpertoprev", actum.ExpertoPrevencionIdExpertoprev);
            ViewData["JefeBodegaIdJefeBodega"] = new SelectList(_context.JefeBodegas, "IdJefeBodega", "IdJefeBodega", actum.JefeBodegaIdJefeBodega);
            ViewData["JefeTerrenoIdJefeterreno"] = new SelectList(_context.JefeTerrenos, "IdJefeterreno", "IdJefeterreno", actum.JefeTerrenoIdJefeterreno);
            ViewData["ObraIdObra"] = new SelectList(_context.Obras, "IdObra", "IdObra", actum.ObraIdObra);
            ViewData["ObsObrasIdObservacion"] = new SelectList(_context.ObsObras, "IdObservacion", "IdObservacion", actum.ObsObrasIdObservacion);
            ViewData["OficinaTecnicaIdOfitec"] = new SelectList(_context.OficinaTecnicas, "IdOfitec", "IdOfitec", actum.OficinaTecnicaIdOfitec);
            ViewData["RazonSocialIdRazonSocial"] = new SelectList(_context.RazonSocials, "IdRazonSocial", "IdRazonSocial", actum.RazonSocialIdRazonSocial);
            return View(actum);
        }

        // GET: Actums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Acta == null)
            {
                return NotFound();
            }

            var actum = await _context.Acta



                .Include(a => a.AdministradorObraIdEncalidadNavigation)
                .Include(a => a.EncargadoCalidadIdEncargadocaliNavigation)
                .Include(a => a.EspBimIdEspBimNavigation)
                .Include(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(a => a.ExpertoPrevencionIdExpertoprevNavigation)
                .Include(a => a.JefeBodegaIdJefeBodegaNavigation)
                .Include(a => a.JefeTerrenoIdJefeterrenoNavigation)
                .Include(a => a.ObraIdObraNavigation)
                .Include(a => a.ObsObrasIdObservacionNavigation)
                .Include(a => a.OficinaTecnicaIdOfitecNavigation)
                .Include(a => a.RazonSocialIdRazonSocialNavigation)
                .FirstOrDefaultAsync(m => m.IdActa == id);
            if (actum == null)
            {
                return NotFound();
            }

            return View(actum);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // POST: Actums/Delete/5
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Acta == null)
            {
                return Problem("Entity set 'BdDhrContext.Acta'  is null.");
            }

            var actum = await _context.Acta.FindAsync(id);
            if (actum != null)
            {
                // 1. Elimina la referencia de Actum a JefeTerreno
                actum.JefeTerrenoIdJefeterreno = null;
                actum.AdministradorObraIdEncalidad = null;
                actum.EncargadoCalidadIdEncargadocali = null;
                actum.EspBimIdEspBim = null;
                actum.ExpertoPrevencionIdExpertoprev = null;
                actum.JefeBodegaIdJefeBodega = null;
                actum.ObsObrasIdObservacion = null;
                actum.OficinaTecnicaIdOfitec = null;
                _context.Update(actum);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los JefeTerreno asociados a este Actum
                var jefeTerrenos = _context.JefeTerrenos.Where(j => j.ActaIdActa == id);
                _context.JefeTerrenos.RemoveRange(jefeTerrenos);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los administrador obra asociados a este Actum
                var administradorobra = _context.AdministradorObras.Where(j => j.ActaIdActa == id);
                _context.AdministradorObras.RemoveRange(administradorobra);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los encargado calidad asociados a este Actum
                var encargadoCalidad = _context.EncargadoCalidads.Where(j => j.ActaIdActa == id);
                _context.EncargadoCalidads.RemoveRange(encargadoCalidad);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los encargado calidad asociados a este Actum
                var espBim = _context.EspBims.Where(j => j.ActaIdActa == id);
                _context.EspBims.RemoveRange(espBim);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los encargado calidad asociados a este Actum
                var expertoPrev = _context.ExpertoPrevencions.Where(j => j.ActaIdActa == id);
                _context.ExpertoPrevencions.RemoveRange(expertoPrev);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los encargado calidad asociados a este Actum
                var jefeBodega = _context.JefeBodegas.Where(j => j.ActaIdActa == id);
                _context.JefeBodegas.RemoveRange(jefeBodega);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los encargado calidad asociados a este Actum
                var obsObra = _context.ObsObras.Where(j => j.ActaIdActa == id);
                _context.ObsObras.RemoveRange(obsObra);
                await _context.SaveChangesAsync();

                // 2. Eliminar todos los encargado calidad asociados a este Actum
                var oficinaTecnica = _context.OficinaTecnicas.Where(j => j.ActaIdActa == id);
                _context.OficinaTecnicas.RemoveRange(oficinaTecnica);
                await _context.SaveChangesAsync();

                // 3. Ahora elimina el Actum
                _context.Acta.Remove(actum);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ListarActas));
        }

        private bool ActumExists(int id)
        {
            return (_context.Acta?.Any(e => e.IdActa == id)).GetValueOrDefault();
        }



        //////////////
        ///
        [HttpGet]
        public async Task<IActionResult> DownloadAsPdf(int id)
        {
            // Configura Puppeteer para que conecte con un navegador
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(); // Descarga la última revisión de Chromium
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();

            // Navegar a la página específica en tu aplicación
            await page.GoToAsync($"http://localhost:5265/Actums/Details/{id}");

            // Inyectar los archivos CSS antes de generar el PDF
            await page.AddStyleTagAsync(new AddTagOptions { Url = "https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" });
            await page.AddStyleTagAsync(new AddTagOptions { Url = "http://localhost:5265//plugins/fontawesome-free/css/all.min.css" });
            await page.AddStyleTagAsync(new AddTagOptions { Url = "http://localhost:5265//plugins/icheck-bootstrap/icheck-bootstrap.min.css" });
            await page.AddStyleTagAsync(new AddTagOptions { Url = "http://localhost:5265//dist/css/adminlte.min.css" });

            // Generar PDF del contenido de la página
            var pdfOptions = new PdfOptions { Format = PaperFormat.A4 };
            var pdf = await page.PdfDataAsync(pdfOptions);

            // Cerrar el navegador
            await browser.CloseAsync();

            // Devolver el archivo PDF generado
            return File(pdf, "application/pdf", $"Acta-{id}.pdf");
        }

        // Método para identificar firmas faltantes
        private List<string> FaltanFirmas(Actum item)
        {
            var firmasFaltantes = new List<string>();

            if (item.EncargadoCalidadIdEncargadocaliNavigation != null && !item.EncargadoCalidadIdEncargadocaliNavigation.Firma)
                firmasFaltantes.Add("Firma de Encargado de Calidad");

            if (item.JefeBodegaIdJefeBodegaNavigation != null && !item.JefeBodegaIdJefeBodegaNavigation.Firma)
                firmasFaltantes.Add("Firma de Jefe de Bodega");

            if (item.EspBimIdEspBimNavigation != null && !item.EspBimIdEspBimNavigation.Firma)
                firmasFaltantes.Add("Firma de Especialista BIM");

            if (item.AdministradorObraIdEncalidadNavigation != null && !item.AdministradorObraIdEncalidadNavigation.Firma)
                firmasFaltantes.Add("Firma de Administrador de Obra");

            if (item.ExpertoPrevencionIdExpertoprevNavigation != null && !item.ExpertoPrevencionIdExpertoprevNavigation.Firma)
                firmasFaltantes.Add("Firma de Experto en Prevención");

            if (item.OficinaTecnicaIdOfitecNavigation != null && !item.OficinaTecnicaIdOfitecNavigation.Firma)
                firmasFaltantes.Add("Firma de Oficina Técnica");

            if (item.JefeTerrenoIdJefeterrenoNavigation != null && !item.JefeTerrenoIdJefeterrenoNavigation.Firma)
                firmasFaltantes.Add("Firma de Jefe de Terreno");

            return firmasFaltantes;
        }

    }
}

