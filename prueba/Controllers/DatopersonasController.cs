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
    public class DatopersonasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DatopersonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Datopersonas
        public async Task<IActionResult> Index(string busqNombre, int? mutualid, int pagina = 1)
        {
            //parte del paginado
            paginador paginador = new paginador()
            {
                cantReg = _context.Datopersona.Count(),
                pagActual = pagina,
                regXpag = 1
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.Datopersona
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            //parte del paginado

            return View(await _context.Datopersona.ToListAsync());
        }


        // GET: Datopersonas/Details/5        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datopersona = await _context.Datopersona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datopersona == null)
            {
                return NotFound();
            }

            return View(datopersona);
        }

        // GET: Datopersonas/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Datopersonas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,apellido,fotopersona,areaid")] Datopersona datopersona)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datopersona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(datopersona);
        }

        // GET: Datopersonas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datopersona = await _context.Datopersona.FindAsync(id);
            if (datopersona == null)
            {
                return NotFound();
            }
            return View(datopersona);
        }

        // POST: Datopersonas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,apellido,fotopersona,areaid")] Datopersona datopersona)
        {
            if (id != datopersona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datopersona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatopersonaExists(datopersona.Id))
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
            return View(datopersona);
        }

        // GET: Datopersonas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datopersona = await _context.Datopersona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datopersona == null)
            {
                return NotFound();
            }

            return View(datopersona);
        }

        // POST: Datopersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datopersona = await _context.Datopersona.FindAsync(id);
            _context.Datopersona.Remove(datopersona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatopersonaExists(int id)
        {
            return _context.Datopersona.Any(e => e.Id == id);
        }
    }
}
