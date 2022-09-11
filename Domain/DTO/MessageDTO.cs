namespace Domain.DTO;

public record struct MessageDTO(Guid Id, Guid ChatId, Guid? AuthorId, string? Author, bool IsSystemMessage, DateTime DateTimeCreated, string TextContent);
