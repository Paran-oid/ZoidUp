namespace API.Models.DTOs
{

    public record struct CreateMessageDTO(string body, int senderID, int receiverID);
    public record struct EditMessageDTO(int messageID, string body);

}
