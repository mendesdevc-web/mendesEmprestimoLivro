using mendesEmprestimoLivro.Data;
using mendesEmprestimoLivro.Models;
using mendesEmprestimoLivro.Services.EmprestimosService;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace mendesEmprestimoLivro.Services.EmprestimoService
{
    public class EmprestimosService : IEmprestimosService
    {
        private readonly ApplicationDbContext _context;
        public EmprestimosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<EmprestimoModel>>> BuscarEmprestimos()
        {
            ResponseModel<List<EmprestimoModel>> response = new ResponseModel<List<EmprestimoModel>>();

            try
            {

                var emprestimos = await _context.Emprestimos.ToListAsync();

                response.Dados = emprestimos;
                response.Mensagem = "Empréstimos encontrados com sucesso.";

                return response;


            }
            catch (Exception ex)
            {

                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<EmprestimoModel>> BuscarEmprestimoPorId(int? id)
        {
            ResponseModel<EmprestimoModel> response = new ResponseModel<EmprestimoModel>();

            try
            {

                if (id == null)
                {
                    response.Mensagem = "Id do empréstimo não fornecido.";
                    response.Status = false;
                    return response;
                }

                var emprestimo = await _context.Emprestimos.FirstOrDefaultAsync(x => x.Id == id);

                if (emprestimo == null)
                {
                    response.Mensagem = "Empréstimo não encontrado.";
                    response.Status = false;
                    return response;
                }

                response.Dados = emprestimo;
                response.Mensagem = "Empréstimo encontrado com sucesso.";

                return response;


            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<DataTable> BuscaDadosEmprestimoExcel()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = "Dados Emprestimos";

            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));

            dataTable.Columns.Add("Data Empréstimo", typeof(DateTime));

            var emprestimo = await BuscarEmprestimos();

            if (emprestimo.Dados.Count > 0)
            {
                emprestimo.Dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataUltimaAtualizacao);
                });
            }
            return dataTable;

        }

        public async Task<ResponseModel<EmprestimoModel>> CadastrarEmprestimo(EmprestimoModel emprestimoModel)
        {

            ResponseModel<EmprestimoModel> response = new ResponseModel<EmprestimoModel>();

            try
            {

                _context.Add(emprestimoModel);
                await _context.SaveChangesAsync();

                response.Mensagem = "Cadastrado realizado com sucesso.";

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<EmprestimoModel>> EditarEmprestimo(EmprestimoModel emprestimoModel)
        {
            ResponseModel<EmprestimoModel> response = new ResponseModel<EmprestimoModel>();

            try
            {
                var emprestimo = await BuscarEmprestimoPorId(emprestimoModel.Id);

                if (emprestimo.Status == false)
                {
                    return emprestimo;
                }

                emprestimo.Dados.LivroEmprestado = emprestimoModel.LivroEmprestado;
                emprestimo.Dados.Fornecedor = emprestimoModel.Fornecedor;
                emprestimo.Dados.Recebedor = emprestimoModel.Recebedor;

                _context.Update(emprestimo.Dados);
                await _context.SaveChangesAsync();

                response.Mensagem = "Edição realizada com sucesso.";

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<EmprestimoModel>> RemoverEmprestimo(EmprestimoModel emprestimoModel)
        {
            ResponseModel<EmprestimoModel> response = new ResponseModel<EmprestimoModel>();

            try
            {


                _context.Remove(emprestimoModel);
                await _context.SaveChangesAsync();

                response.Mensagem = "Remoção realizada com sucesso.";

                return response;


            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }

        }
    }
}
