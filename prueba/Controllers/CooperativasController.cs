using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prueba.Data;
using prueba.Models;
using prueba.ViewModels;

namespace prueba.Controllers
{
    public class CooperativasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public CooperativasController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env; 
        }

        // GET: Cooperativas
        public async Task Importar()
        {
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivoImpo = archivos[0];
                var pathDestino = Path.Combine(env.WebRootPath, "importaciones");
                if (archivoImpo.Length > 0)
                {
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoImpo.FileName);
                    string rutaCompleta = Path.Combine(pathDestino, archivoDestino);
                    using (var filestream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        archivoImpo.CopyTo(filestream);
                    };

                    using (var file = new FileStream(rutaCompleta, FileMode.Open))
                    {
                        List<string> renglones = new List<string>();
                        List<Cooperativa> coopArch = new List<Cooperativa>();

                        StreamReader fileContent = new StreamReader(file); // System.Text.Encoding.Default
                        do
                        {
                            renglones.Add(fileContent.ReadLine());
                        }
                        while (!fileContent.EndOfStream);

                        foreach (string renglon in renglones)
                        {
                            int salida;
                            string[] datos = renglon.Split(';');
                            int mutual = int.TryParse(datos[datos.Length - 1], out salida) ? salida : 0;
                            if (mutual > 0 && _context.Mutual.Where(c => c.Id == mutual).FirstOrDefault() != null)
                            {
                                Mutual mutualtemporal = new Mutual()
                                {
                                    mutualid = mutual,
                                    mutualpers = datos[0]
                                    
                                };
                                coopArch.Add(mutualtemporal);
                            }
                        }
                        if (coopArch.Count > 0)
                        {
                            _context.Cooperativa.AddRange(coopArch);
                            _context.SaveChanges();
                        }

                        ViewBag.cantReng = coopArch.Count + " de " + renglones.Count;
                    }
                }
                        }


                    }
        public async Task<IActionResult> Index(string? busqNombre, int? mutualid, int pagina=1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var applicationDbContext = _context.Cooperativa.Include(c => c.mutualpers).Select(a => a);
            if (!string.IsNullOrEmpty(busqNombre))
            {
                applicationDbContext = applicationDbContext.Where(a => a.Nombre.Contains(busqNombre));
                paginador.ValoresQueryString.Add("busqNombre", busqNombre);
            }
            if (mutualid.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(a => a.mutualid == mutualid.Value);
                paginador.ValoresQueryString.Add("mutualId", mutualid.ToString());
            }
            paginador.cantReg = applicationDbContext.Count();


            paginador.totalPag = (int)Math.Ceiling((decimal)paginador.cantReg / paginador.regXpag);
            var datosAmostrar = applicationDbContext
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);


                PersonasViewModels modelo = new PersonasViewModels()
                {
                    //esto sirve para la vista del filtrado,corregir error 
                    personas = applicationDbContext.ToList(),
                    Mutual = new SelectList(_context.Set<Mutual>(), "Id", "mutualpers", mutualid),
                    busqNombre = busqNombre,
                    paginador = paginador

                };
            //ViewData["mutualid"] = new SelectList(_context.Set<Mutual>(), "Id", "mutualpers", mutualid);
            return View(modelo);
        }

        // GET: Cooperativas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cooperativa = await _context.Cooperativa
                .Include(c => c.mutualpers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cooperativa == null)
            {
                return NotFound();
            }

            return View(cooperativa);
        }

        // GET: Cooperativas/Create
        //[Authorize]
        public IActionResult Create()
        {
        
            ViewData["mutualid"] = new SelectList(_context.Set<Mutual>(), "Id", "mutualpers");
            return View();
        }

        // POST: Cooperativas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,biografia,fechacreacion,fotocoop,mutualid,personaid")] Cooperativa cooperativa)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivofoto = archivos[0];

                    if (archivofoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivofoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivofoto.CopyTo(filestream);
                            cooperativa.fotocoop = archivoDestino;
                        }
                    }
                }
                _context.Add(cooperativa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["mutualid"] = new SelectList(_context.Set<Mutual>(), "Id", "mutualpers", cooperativa.mutualid);
            return View(cooperativa);
        }

        // GET: Cooperativas/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cooperativa = await _context.Cooperativa.FindAsync(id);
            if (cooperativa == null)
            {
                return NotFound();
            }
            ViewData["mutualid"] = new SelectList(_context.Set<Mutual>(), "Id", "mutualpers", cooperativa.mutualid);
            return View(cooperativa);
        }

        // POST: Cooperativas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,nombre,biografia,fechacreacion,fotocoop,mutualid,personaid")] Cooperativa cooperativa)
        {
            if (id != cooperativa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivofoto = archivos[0];

                    if (archivofoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivofoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);
                        string fotoAnterior = Path.Combine(pathDestino, cooperativa.fotocoop);
                        if (string.IsNullOrEmpty(cooperativa.fotocoop))
                        {
                            if (System.IO.File.Exists(fotoAnterior))
                                System.IO.File.Delete(fotoAnterior);
                        }

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivofoto.CopyTo(filestream);
                            cooperativa.fotocoop = archivoDestino;
                        }
                    }
                }
                
                try
                {
                    _context.Update(cooperativa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CooperativaExists(cooperativa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["mutualid"] = new SelectList(_context.Set<Mutual>(), "Id", "mutualpers", cooperativa.mutualid);
            return View(cooperativa);
        }

        // GET: Cooperativas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cooperativa = await _context.Cooperativa
                .Include(c => c.mutualpers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cooperativa == null)
            {
                return NotFound();
            }

            return View(cooperativa);
        }

        // POST: Cooperativas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cooperativa = await _context.Cooperativa.FindAsync(id);
            _context.Cooperativa.Remove(cooperativa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CooperativaExists(string id)
        {
            return _context.Cooperativa.Any(e => e.Id == id);
        }
    }
}
