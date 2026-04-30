using mendesEmprestimoLivro.Models;
using System.Data;

namespace mendesEmprestimoLivro.Services.EmprestimosService
{
    public interface IEmprestimosService
    {
       Task<ResponseModel<List<EmprestimoModel>>> BuscarEmprestimos();
       Task<ResponseModel<EmprestimoModel>> BuscarEmprestimoPorId(int? id);
       Task<ResponseModel<EmprestimoModel>> CadastrarEmprestimo(EmprestimoModel emprestimoModel);
       Task<ResponseModel<EmprestimoModel>> EditarEmprestimo(EmprestimoModel emprestimoModel);
       Task<ResponseModel<EmprestimoModel>> RemoverEmprestimo(EmprestimoModel emprestimoModel);
        Task<DataTable> BuscaDadosEmprestimoExcel();

    }
}
