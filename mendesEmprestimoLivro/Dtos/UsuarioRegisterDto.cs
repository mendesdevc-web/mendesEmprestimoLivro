using System.ComponentModel.DataAnnotations;

namespace mendesEmprestimoLivro.Dtos
{
    public class UsuarioRegisterDto
    {
        [Required(ErrorMessage = "Digite o Nome .")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite Sobrenome.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Digite o Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite Senha.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Digite a Confirmação de Senha!."),
            Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmaSenha { get; set; }
    }
}
