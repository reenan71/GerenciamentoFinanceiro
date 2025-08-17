using System.Diagnostics;
using GerenciamentoFinanceiro.Data;
using Microsoft.AspNetCore.Mvc;
using GerenciamentoFinanceiro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GerenciamentoFinanceiro.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index(string id)
    {
        var filtros = new FiltroModel(id);
        
        ViewBag.Filtros = filtros;
        ViewBag.Categorias = _context.Categoria.ToList();
        ViewBag.Transacao = _context.Transacao.ToList();
        ViewBag.DataOperacao = FiltroModel.ListaDataOperacao;

        IQueryable<FinanceiroModel> consulta = _context.Financeiro
            .Include(c => c.Transacao)
            .Include(c => c.Categoria);

        if (filtros.FiltroCategoria)
        {
            consulta = consulta.Where(c => c.CategoriaId == filtros.CategoriaId);
        }; 
        if (filtros.FiltroTransacao)
        {
            consulta = consulta.Where(c => c.TransacaoId == filtros.TransacaoId);
        };
        if (filtros.FiltroDataOperacao)
        {
            var diaHoje = DateTime.Today;
            if (filtros.EhPassado)
            {
                consulta = consulta.Where(c => c.DataOperacao < diaHoje);
            }
            if (filtros.EhFuturo)
            {
                consulta = consulta.Where(c => c.DataOperacao > diaHoje);
            }
            if (filtros.EhHoje)
            {
                consulta = consulta.Where(c => c.DataOperacao == diaHoje);
            }
        }

        var resumoFinanceiro = consulta.OrderBy(d => d.DataOperacao).ToList();
        
        return View(resumoFinanceiro);
    }
    
    public IActionResult AdicionarTransacao()
    {
        ViewBag.Categorias = _context.Categoria.ToList();
        ViewBag.Transacoes = _context.Transacao.ToList();
        return View();
    }
    
    public IActionResult AdicionarCategoria()
    {
        var categoria = new CategoriaModel { CategoriaId = "categoria" };
        return View(categoria);
    }
    
    public IActionResult SomatoriaValores()
    {
        var resultados = from g in _context.Financeiro
                .Include(x => x.Categoria)
                .Include(x => x.Transacao)
                .ToList()
            group g by new { g.CategoriaId }
            into total
            select new
            {
                CategoriaNome = total.First().Categoria.Nome,
                TransacaoNome = total.First().Transacao.Nome,
                DataDaOperacao = total.First().DataOperacao,
                Total = total.Sum(c => c.Valor)
            };
        var ganhos = _context.Financeiro
            .Include(x => x.Categoria)
            .Include(x => x.Transacao)
            .Where(x => x.TransacaoId == "ganho")
            .Sum(x => x.Valor);
        
        var gastos = _context.Financeiro
            .Include(x => x.Categoria)
            .Include(x => x.Transacao)
            .Where(x => x.TransacaoId == "perda")
            .Sum(x => x.Valor);

        var diferenca = ganhos - gastos;

        List<AnaliseFinaceira> registros = new List<AnaliseFinaceira>();
        foreach (var resultado in  resultados)
        {
            var registro = new AnaliseFinaceira()
            {
                CategoriaNome = resultado.CategoriaNome,
                TransacaoNome = resultado.TransacaoNome,
                DataOperacao = resultado.DataDaOperacao.ToString("dd/MM/yyyy"),
                ValorCategoria = resultado.Total.ToString("F"),
                Ganhos = ganhos.ToString("F"),
                Gastos = gastos.ToString("F"),
                Diferenca = diferenca.ToString("F")
            };
            registros.Add(registro);
        }

        return View(registros);
    }
    
    public IActionResult RemoverTransacao(int id)
    {
        var transacao = _context.Financeiro.Find(id);
        _context.Remove(transacao);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult Filtrar(string [] filtro)
    {
        string id = string.Join("-", filtro);
        return RedirectToAction("Index", new {ID = id});
    }

    [HttpPost]
    public IActionResult AdicionarTransacao(FinanceiroModel financeiro)
    {
        if (ModelState.IsValid)
        {
            _context.Financeiro.Add(financeiro);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Transacoes = _context.Transacao.ToList();
            return View(financeiro);
        }
    }

    [HttpPost]
    public IActionResult AdicionarCategoria(CategoriaModel categoria)
    {
        if (ModelState.IsValid)
        {
            var categoriaDb = new CategoriaModel
            {
                CategoriaId = categoria.Nome.ToLower(),
                Nome = categoria.Nome
            };
            _context.Categoria.Add(categoriaDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }        
        else
        {
            return View(categoria);
        }
    }

}