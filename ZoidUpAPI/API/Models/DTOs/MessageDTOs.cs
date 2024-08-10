namespace API.Models.DTOs
{

    public record struct CreateMessageDTO(string body, int senderID, int ReceiverID);
    public record struct EditMessageDTO(int MessageID, string body);

}
