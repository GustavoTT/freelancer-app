using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string UserName { get; set;}

        [Required]
        [MaxLength(150)]
        public required string Email { get; set;}

        [Required]
        [MaxLength(150)]
        public required string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Password { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        //Navigation property
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}