namespace API.Models.DTOs
{

    public record struct CreateMessageDTO(string body, int senderId, int receiverId);
    public record struct EditMessageDTO(int messageId, string body);

}
