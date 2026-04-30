using ClosedXML.Excel;
using mendesEmprestimoLivro.Data;
using mendesEmprestimoLivro.Models;
using mendesEmprestimoLivro.Services.EmprestimoService;
using mendesEmprestimoLivro.Services.EmprestimosService;
using mendesEmprestimoLivro.Services.SessãoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace mendesEmprestimoLivro.Controllers
{
    public class EmprestimoController : Controller
    {
        readonly private ISessaoService _sessaoService;
        readonly private IEmprestimosService _emprestimosService;

        public EmprestimoController( IEmprestimosService emprestimosService
                                    , ISessaoService sessaoService)
        {
            _emprestimosService = emprestimosService;
            _sessaoService = sessaoService;
        }
        public async Task<IActionResult> Index()
        {

            var usuario = _sessaoService.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var emprestimo = await _emprestimosService.BuscarEmprestimos();

            return View(emprestimo.Dados);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            
            var usuario = _sessaoService.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }


            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            var usuario = _sessaoService.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var emprestimo = await _emprestimosService.BuscarEmprestimoPorId(id);

            return View(emprestimo.Dados);
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            var usuario = _sessaoService.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var emprestimo = await _emprestimosService.BuscarEmprestimoPorId(id);

            return View(emprestimo.Dados);
        }

        public async Task<IActionResult> Exportar()
        {
            var dados = await _emprestimosService.BuscaDadosEmprestimoExcel();
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



        [HttpPost]
        public async Task<IActionResult> Cadastrar(EmprestimoModel emprestimo)
        {
            if (ModelState.IsValid)
            {


                var emprestimoResult = await _emprestimosService.CadastrarEmprestimo(emprestimo);


                if(emprestimoResult.Status)
                {
                    TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = emprestimoResult.Mensagem;
                    return View(emprestimo);
                }

                return RedirectToAction("Index");
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Editar(EmprestimoModel emprestimo)
        {
            if (ModelState.IsValid)
            {

                var emprestimoResult = await _emprestimosService.EditarEmprestimo(emprestimo);

                if (ModelState.IsValid)

                    if (emprestimoResult.Status)
                    {
                        TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
                    }
                    else
                    {
                        TempData["MensagemErro"] = emprestimoResult.Mensagem;
                        return View(emprestimo);
                    }

                    return RedirectToAction("Index");
                }
                TempData["MensagemErro"] = "Ocorreu um erro ao editar o empréstimo.";
                return View(emprestimo);

        }

        [HttpPost]
        public async Task<IActionResult> Excluir(EmprestimoModel emprestimo)
        {
            if (emprestimo == null )
            {
                TempData["MensagemErro"] = "Emprestimo nao localizado";
                return View(emprestimo);
            }

            var emprestimoResult = await _emprestimosService.RemoverEmprestimo(emprestimo);

            if(emprestimoResult.Status)
            {

                TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
            }
            else
            {
                TempData["MensagemErro"] = emprestimoResult.Mensagem;
                return View(emprestimo);
            }

            return RedirectToAction("Index");
        }
    }
}
