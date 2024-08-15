namespace API.Models.DTOs
{
    public record struct RequestUserDTO(int id,string username, DateTime time, string profilePicturePath);
}
