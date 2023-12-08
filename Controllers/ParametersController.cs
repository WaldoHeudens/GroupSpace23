﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Authorization;

namespace GroupSpace23.Controllers
{
    [Authorize (Roles = "SystemAdministrator")]
    public class ParametersController : Controller
    {
        private readonly MyDbContext _context;

        public ParametersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Parameters
        public async Task<IActionResult> Index()
        {
              return _context.Parameters != null ? 
                          View(await _context.Parameters.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.Parameter'  is null.");
        }

        // GET: Parameters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Parameters == null)
            {
                return NotFound();
            }

            var parameter = await _context.Parameters
                .FirstOrDefaultAsync(m => m.Name == id);
            if (parameter == null)
            {
                return NotFound();
            }

            return View(parameter);
        }

 
        // GET: Parameters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Parameters == null)
            {
                return NotFound();
            }

            var parameter = await _context.Parameters.FindAsync(id);
            if (parameter == null)
            {
                return NotFound();
            }
            return View(parameter);
        }

        // POST: Parameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Value,UserId,LastChanged")] Parameter parameter)
        {
            if (id != parameter.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parameter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParameterExists(parameter.Name))
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
            return View(parameter);
        }


        private bool ParameterExists(string id)
        {
          return (_context.Parameters?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
