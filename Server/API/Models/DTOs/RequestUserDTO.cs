namespace API.Models.DTOs
{
    public record struct RequestUserDTO(int Id, string username, DateTime time, string profilePicturePath);
}
