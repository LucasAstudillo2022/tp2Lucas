using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prueba.Data;
using prueba.Models;
using prueba.ViewModels;

namespace prueba.Controllers
{
    public class AreatrabajosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AreatrabajosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Areatrabajos
        public async Task<IActionResult> Index(string busqNombre, int? mutualid, int pagina = 1)
        {
            //parte del paginado
            paginador paginador = new paginador()
            {
                cantReg = _context.Areatrabajo.Count(),
                pagActual = pagina,
                regXpag = 1
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.Areatrabajo
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);
            //parte del paginado
            return View(await _context.Areatrabajo.ToListAsync());
        }

        // GET: Areatrabajos/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var areatrabajo = await _context.Areatrabajo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (areatrabajo == null)
            {
                return NotFound();
            }

            return View(areatrabajo);
        }

        // GET: Areatrabajos/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Areatrabajos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,area")] Areatrabajo areatrabajo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(areatrabajo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(areatrabajo);
        }

        // GET: Areatrabajos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var areatrabajo = await _context.Areatrabajo.FindAsync(id);
            if (areatrabajo == null)
            {
                return NotFound();
            }
            return View(areatrabajo);
        }

        // POST: Areatrabajos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,area")] Areatrabajo areatrabajo)
        {
            if (id != areatrabajo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(areatrabajo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreatrabajoExists(areatrabajo.Id))
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
            return View(areatrabajo);
        }

        // GET: Areatrabajos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var areatrabajo = await _context.Areatrabajo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (areatrabajo == null)
            {
                return NotFound();
            }

            return View(areatrabajo);
        }

        // POST: Areatrabajos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var areatrabajo = await _context.Areatrabajo.FindAsync(id);
            _context.Areatrabajo.Remove(areatrabajo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreatrabajoExists(int id)
        {
            return _context.Areatrabajo.Any(e => e.Id == id);
        }
    }
}
