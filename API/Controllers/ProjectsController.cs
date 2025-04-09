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
        [HttpGet("me/projects")]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDetailsDTO>>> GetProjects()
        {
            var projects = await _context.Projects
                .Include(p => p.User)
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

            return result > 0 ? Ok("Projeto criado com sucesso!") : BadRequest("Erro ao criar projeto.");
        }
    }
}