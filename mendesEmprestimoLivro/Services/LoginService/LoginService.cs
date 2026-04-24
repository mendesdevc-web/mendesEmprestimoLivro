using mendesEmprestimoLivro.Data;
using mendesEmprestimoLivro.Dtos;
using mendesEmprestimoLivro.Models;
using mendesEmprestimoLivro.Services.SenhaService;
using mendesEmprestimoLivro.Services.SessãoService;

namespace mendesEmprestimoLivro.Services.LoginService
{
    public class LoginService : IloginInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhainterface _senhaInterface;
        private readonly ISessaoService _sessaoService;
        public LoginService(ApplicationDbContext context, 
                            ISenhainterface senhaInterface, 
                            ISessaoService sessaoService)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _sessaoService = sessaoService;
        }

        
        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
             ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioLoginDto.Email);

                if (usuario == null)
                {
                    response.Mensagem = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                if (!_senhaInterface.VerificaSenha(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }


                _sessaoService.CriarSessao(usuario);

                response.Mensagem = "Login realizado com sucesso.";
                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = $"Erro ao fazer login: {ex.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();


            try
            {
                if (VerificarSeEmailExiste(usuarioRegisterDto))
                {
                    response.Mensagem = "Email já cadastrado.";
                    response.Status = false;
                    return response;

                }

                _senhaInterface.CriarSenhaHash(usuarioRegisterDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);


                var usuario = new UsuarioModel()
                {
                    Name = usuarioRegisterDto.Nome,
                    Sobrenome = usuarioRegisterDto.Sobrenome,
                    Email = usuarioRegisterDto.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                response.Dados = usuario;
                response.Mensagem = "Usuário registrado com sucesso.";
                response.Status = true;

                return response;



            }
            catch (Exception ex)
            {
                response.Mensagem = $"Erro ao registrar usuário: {ex.Message}";
                response.Status = false;
                return response;

            }




        }

        private bool VerificarSeEmailExiste(UsuarioRegisterDto usuarioRegisterDto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == usuarioRegisterDto.Email);
            return usuario != null;
        }
    }
}
