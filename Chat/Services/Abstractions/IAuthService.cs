using Chat.Models;

namespace Chat.Services.Abstractions;

public interface IAuthService
{
    Task<AuthResponse> Signup(SignupModel signupModel);
    Task<AuthResponse> Login(LoginModel loginModel);
    Task<AuthResponse> Logout();
}
