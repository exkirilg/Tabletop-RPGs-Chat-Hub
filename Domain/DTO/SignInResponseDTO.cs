namespace Domain.DTO;

public record struct SignInResponseDTO(string JWTToken, string UserName);
