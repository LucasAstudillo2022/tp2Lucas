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
    public class MutualsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MutualsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mutuals
        public async Task<IActionResult> Index(int pagina=1)
        {
            paginador paginador = new paginador()
            {
                cantReg = _context.Mutual.Count(),
                pagActual = pagina,
                regXpag = 1
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.Mutual
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);
            //parte del paginado
            return View(await _context.Mutual.ToListAsync());
        }

        // GET: Mutuals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mutual = await _context.Mutual
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mutual == null)
            {
                return NotFound();
            }

            return View(mutual);
        }

        // GET: Mutuals/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mutuals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,mutualpers")] Mutual mutual)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mutual);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mutual);
        }

        // GET: Mutuals/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mutual = await _context.Mutual.FindAsync(id);
            if (mutual == null)
            {
                return NotFound();
            }
            return View(mutual);
        }

        // POST: Mutuals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,mutualpers")] Mutual mutual)
        {
            if (id != mutual.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mutual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MutualExists(mutual.Id))
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
            return View(mutual);
        }

        // GET: Mutuals/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mutual = await _context.Mutual
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mutual == null)
            {
                return NotFound();
            }

            return View(mutual);
        }

        // POST: Mutuals/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mutual = await _context.Mutual.FindAsync(id);
            _context.Mutual.Remove(mutual);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MutualExists(int id)
        {
            return _context.Mutual.Any(e => e.Id == id);
        }
    }
}
