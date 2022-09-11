namespace Domain.DTO;

public record struct MessageDTO(Guid Id, Guid ChatId, string? Author, bool IsSystemMessage, DateTime DateTimeCreated, string TextContent);
