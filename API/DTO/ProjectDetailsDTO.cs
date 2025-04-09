namespace API.DTO
{
    public class ProjectDetailsDTO
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string? SkillsRequired { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public SimpleUserDTO? User { get; set; }
    }
}