namespace Lab11_Sapacayo_Yordan.Domain.Entities;

public class Response
{
    public Guid ResponseId { get; set; }
    public Guid TicketId { get; set; }
    public Guid ResponderId { get; set; }
    public string Message { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
}
