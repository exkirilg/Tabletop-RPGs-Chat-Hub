using Domain.DTO;

namespace Services.Interfaces;

public interface IIdentityServices
{
    Task<SignInResponseDTO> SignInAsync(SignInRequestDTO request);
    Task<SignInResponseDTO> SignUpAsync(SignUpRequestDTO request);
}
