using HomeDecoration.Models.DTO;

namespace HomeDecoration.Repository.Implementation
{
    public interface IUserAuthenServices
    {
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> RegistrationAsync(RegistrationModel model);
        Task LogoutAsync();
    }
}
