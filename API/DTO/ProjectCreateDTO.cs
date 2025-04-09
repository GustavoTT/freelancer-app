namespace API.DTO
{
    public class ProjectCreateDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string? SkillsRequired { get; set; }
    }
}