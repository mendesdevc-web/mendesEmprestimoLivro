using System.ComponentModel.DataAnnotations;

namespace mendesEmprestimoLivro.Models
{
    public class EmprestimoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Recebedor é obrigatório.")]
        public string Recebedor { get; set; }

        [Required(ErrorMessage = "O campo Fornecedor é obrigatório.")]      
        public string Fornecedor { get; set; }

        [Required(ErrorMessage = "O campo Livro é obrigatório.")]
        public string LivroEmprestado { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.Now;
    }
}
