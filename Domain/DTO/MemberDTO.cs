namespace Domain.DTO;

public record struct MemberDTO(Guid Id, Guid ChatId, string Username, string Nickname);
