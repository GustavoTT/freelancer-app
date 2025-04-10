using System.Security.Claims;
using API.Data;
using API.DTO;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDetailsDTO>>> GetMyProjects()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userIdClaim, out int UserId))
                return Unauthorized("Id não encontrado no token!");

            var projects = await _context.Projects
                .Include(p => p.User)
                .Where(p => p.UserId == UserId)
                .AsNoTracking()
                .ToListAsync();

            var projectDtos = projects.Select(p => new ProjectDetailsDTO
            {
                ProjectId = p.ProjectId,
                UserId = p.UserId,
                Title = p.Title,
                Description = p.Description,
                Budget = p.Budget,
                Deadline = p.Deadline,
                SkillsRequired = p.SkillsRequired,
                Status = p.Status,
                CreatedDate = p.CreatedDate,
                User = p.User == null ? null : new SimpleUserDTO
                {
                    UserId = p.User.UserId,
                    UserName = p.User.UserName,
                    FullName = p.User.FullName
                }
            });

            return Ok(projectDtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectReadDTO>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var dto = new ProjectReadDTO
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                Description = project.Description,
                Budget = project.Budget,
                Deadline = project.Deadline,
                SkillsRequired = project.SkillsRequired,
                Status = project.Status,
                UserId = project.UserId
            };

            return Ok(dto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreateDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!int.TryParse(userIdClaim, out int UserId))
                return Unauthorized("Id não encontrado no token!");

            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                Budget = dto.Budget,
                Deadline = dto.Deadline,
                SkillsRequired = dto.SkillsRequired,
                UserId = UserId
            };

            await _context.Projects.AddAsync(project);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? Ok(new { message = "Projeto criado com sucesso"}) : BadRequest(new { message = "Erro ao criar projeto"});
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectUpdateDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Id não encontrado no token!");

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id && p.UserId == userId);
            if (project == null)
                return NotFound("Projeto não encontrado!");

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Budget = dto.Budget;
            project.Deadline = dto.Deadline;
            project.SkillsRequired = dto.SkillsRequired;
            project.Status = dto.Status;

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Atualizado com sucesso" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Id não encontrado no token!");

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id && p.UserId == userId);
            if (project == null)
                return NotFound("Projeto não encontrado!");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Projeto removido com sucesso."});
        }


        // Get All Projects
        // [Authorize]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ProjectDetailsDTO>>> GetProjects()
        // {
        //     var projects = await _context.Projects
        //         .Include(p => p.User)
        //         .AsNoTracking()
        //         .ToListAsync();

        //     var projectDtos = projects.Select(p => new ProjectDetailsDTO
        //     {
        //         ProjectId = p.ProjectId,
        //         UserId = p.UserId,
        //         Title = p.Title,
        //         Description = p.Description,
        //         Budget = p.Budget,
        //         Deadline = p.Deadline,
        //         SkillsRequired = p.SkillsRequired,
        //         Status = p.Status,
        //         CreatedDate = p.CreatedDate,
        //         User = p.User == null ? null : new SimpleUserDTO
        //         {
        //             UserId = p.User.UserId,
        //             UserName = p.User.UserName,
        //             FullName = p.User.FullName
        //         }
        //     });

        //     return Ok(projectDtos);
        // }
    }
}