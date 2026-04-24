using mendesEmprestimoLivro.Dtos;
using mendesEmprestimoLivro.Services.LoginService;
using Microsoft.AspNetCore.Mvc;

namespace mendesEmprestimoLivro.Controllers
{
    public class LoginController : Controller
    {
        private readonly IloginInterface _loginInterface;

        public LoginController(IloginInterface loginInterface)
        {
            _loginInterface = loginInterface;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

       
        
        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegisterDto usuarioRegisterDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _loginInterface.RegistrarUsuario(usuarioRegisterDto);

                if(usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(usuarioRegisterDto);
                }


                return RedirectToAction("Index");

            }
            else
            {

            return View(usuarioRegisterDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = await _loginInterface.Login(usuarioLoginDto);

            if (ModelState.IsValid)
            {
                if (usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View("Index", usuarioLoginDto);
                }
            }
            else
            {
                return View("Index",usuarioLoginDto);
            }
        }


    }
}
