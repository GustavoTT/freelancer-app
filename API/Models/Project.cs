using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Budget { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public string? SkillsRequired { get; set; }

        public Boolean Status { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User? User { get; set; }
    }
}