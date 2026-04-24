using mendesEmprestimoLivro.Models;

namespace mendesEmprestimoLivro.Services.SessãoService
{
    public interface ISessaoService
    {
        UsuarioModel BuscarSessao();
        void CriarSessao(UsuarioModel usuarioModel);
        void RemoverSessao();
        
    }
}
