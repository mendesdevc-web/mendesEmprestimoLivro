using mendesEmprestimoLivro.Dtos;
using mendesEmprestimoLivro.Models;

namespace mendesEmprestimoLivro.Services.LoginService
{
    public interface IloginInterface
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto);
        Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto);
    }
}
