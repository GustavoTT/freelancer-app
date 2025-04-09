using API.Data;
using API.DTO;
using API.Models;
using API.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        public UsersController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Projects)
                .AsNoTracking()
                .ToListAsync();

            var userDtos = users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                FullName = u.FullName,
                CreatedDate = u.CreatedDate,
                Projects = u.Projects.Select(p => new ProjectDTO
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title,
                    Description = p.Description,
                    Budget = p.Budget,
                    Deadline = p.Deadline,
                    SkillsRequired = p.SkillsRequired,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate
                }).ToList()
            });

            return Ok(userDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (existingUser != null)
                return Conflict("Esse nome de usuário já está em uso.");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == login.UserName);                

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return Unauthorized("Senha ou Usuario inválidos!.");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        // [HttpGet("{id:int}")]
        // public async Task<ActionResult<User>> GetUser(int id)
        // {
        //     var user = await _context.Users.FindAsync(id);

        //     if(user == null)
        //         return NotFound();
            
        //     return Ok(user);
        // }
        
        // [HttpDelete("{id:int}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var user = await _context.Users.FindAsync(id);

        //     if(user == null)
        //         return NotFound();

        //     _context.Remove(user);

        //     var result = await _context.SaveChangesAsync();
        //     if(result > 0)
        //         return Ok("Usuário deletado");

        //     return BadRequest("Não foi possivel deletar o usuario");
        // } 

        // [HttpPut("{id:int}")]
        // public async Task<IActionResult> EditUser(int id, User user)
        // {
        //     var userFromDb = await _context.Users.FindAsync(id);

        //     if(userFromDb == null)
        //         return BadRequest("Usuário não encontrado");

        //     userFromDb.UserName = user.UserName;
        //     userFromDb.Email = user.Email;
        //     userFromDb.FullName = user.FullName;

        //     var result = await _context.SaveChangesAsync();

        //     if(result > 0)
        //         return Ok("Usuário atualizado");

        //     return BadRequest("Não foi possível atualizar o usuário"); 
        // }
    }
}