using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Authorization;

namespace GroupSpace23.ApiControllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public GroupsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            return await _context.Groups.Where(g => g.Ended > DateTime.Now).ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{name}/{id}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup(string name, int id=0)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            if (id != 0)
                return await _context.Groups.Where(group => group.Id == id && group.Ended > DateTime.Now).ToListAsync();
            else
                if (name != "")
                    return await _context.Groups.Where(group => group.Name.Contains(name) && group.Ended > DateTime.Now).ToListAsync();

            return NotFound();

        }

        //[HttpGet("{name}")]
        //public async Task<ActionResult<IEnumerable<Group>>> GetGroups(string name)
        //{
        //    return await _context.Groups.Where(group => group.Name.Contains(name) && group.Ended > DateTime.Now).ToListAsync();
        //}




        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            if (id != @group.Id)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
          if (_context.Groups == null)
          {
              return Problem("Entity set 'MyDbContext.Groups'  is null.");
          }
            _context.Groups.Add(@group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            //_context.Groups.Remove(@group);
            group.Ended = DateTime.Now;
            _context.Update(@group);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
