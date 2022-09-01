using Domain.DTO;

namespace Services.Interfaces;

public interface IIdentityServices
{
    public Task<int> GetNumberOfUsersAsync();
    public Task<SignInResponseDTO> SignInAsync(SignInRequestDTO request);
    public Task<SignInResponseDTO> SignUpAsync(SignUpRequestDTO request);
}
