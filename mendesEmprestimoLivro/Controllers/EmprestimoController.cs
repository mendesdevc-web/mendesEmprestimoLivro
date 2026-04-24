using ClosedXML.Excel;
using mendesEmprestimoLivro.Data;
using mendesEmprestimoLivro.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace mendesEmprestimoLivro.Controllers
{
    public class EmprestimoController : Controller
    {
        readonly private ApplicationDbContext _db;
        public EmprestimoController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<EmprestimoModel> emprestimo = _db.Emprestimos;


            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimoModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }


        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimoModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id.Value);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }


        public IActionResult Exportar()
        {
            var dados = GetDados();
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados,"Dados Empréstimo");

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DadosEmprestimo.xlsx");
                }
            }
        }

        private DataTable GetDados()
        { 
            DataTable dataTable = new DataTable();
            dataTable.TableName = "Dados de empréstimo";

            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));

            var dados = _db.Emprestimos.ToList();

            if (dados.Count > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado);
                });
            }
            return dataTable;
        }


        [HttpPost]
        public IActionResult Cadastrar(EmprestimoModel emprestimo)
        {
            if (ModelState.IsValid)
            {
                emprestimo.DataUltimaAtualizacao = DateTime.Now;

                _db.Emprestimos.Add(emprestimo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Empréstimo cadastrado com sucesso!";


                return RedirectToAction("Index");
            }

            return View();

        }

        [HttpPost]
        public IActionResult Editar(EmprestimoModel emprestimo)
        {
            if (ModelState.IsValid)
            {
                var emprestimoDb = _db.Emprestimos.Find(emprestimo.Id);

                emprestimoDb.Fornecedor = emprestimo.Fornecedor;
                emprestimoDb.Recebedor = emprestimo.Recebedor;
                emprestimoDb.LivroEmprestado = emprestimo.LivroEmprestado;

                _db.Emprestimos.Update(emprestimoDb);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizadox     com sucesso!";

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu um erro ao editar o empréstimo. Verifique os dados e tente novamente.";

            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimoModel emprestimo)
        {
            if (emprestimo == null )
            {
                return NotFound();
            }

            _db.Emprestimos.Remove(emprestimo);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Remoção realizado com sucesso!";

            return RedirectToAction("Index");
        }
    }
}
