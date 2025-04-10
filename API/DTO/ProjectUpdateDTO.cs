namespace API.DTO
{
    public class ProjectUpdateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string? SkillsRequired { get; set; }
        public bool Status { get; set; }
    }
}