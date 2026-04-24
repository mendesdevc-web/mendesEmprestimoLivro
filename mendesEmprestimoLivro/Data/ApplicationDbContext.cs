using Microsoft.EntityFrameworkCore;

namespace mendesEmprestimoLivro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Models.EmprestimoModel> Emprestimos { get; set; }
        public DbSet<Models.UsuarioModel> Usuarios { get; set; }

    }
}
