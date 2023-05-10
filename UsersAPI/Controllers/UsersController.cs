using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersAPI.Models;

namespace UsersAPI
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
            if (existingUser != null)
            {
                return Conflict("Пользователь с таким логином уже существует.");
            }

            var activeState = await _context.UserStates.FirstOrDefaultAsync(u => u.Code == "Active");
            if (activeState == null)
            {
                return BadRequest("Не удалось найти статус 'Active'.");
            }

            user.UserStateId = activeState.Id;
            user.CreatedDate = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await Task.Delay(5000);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var blockedState = await _context.UserStates.FirstOrDefaultAsync(u => u.Code == "Blocked");
            if (blockedState == null)
            {
                return BadRequest("Не удалось найти статус 'Blocked'.");
            }

            user.UserStateId = blockedState.Id;
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
