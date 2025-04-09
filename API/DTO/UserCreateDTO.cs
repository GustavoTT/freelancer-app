namespace API.DTO
{
    public class UserCreateDTO
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Password { get; set; }
    }
}