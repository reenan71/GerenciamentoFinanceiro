using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GerenciamentoFinanceiro.Models;

public class FinanceiroModel
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Necessario preencher a descrição!")]
    public string Descricao { get; set; }
    [Required(ErrorMessage = "Necessario preencher o valor!")]
    public double Valor { get; set; }
    [Required(ErrorMessage = "Necessario preencher a data!")]
    public DateTime DataOperacao { get; set; }
    [Required(ErrorMessage = "Necessario preencher a categoria!")]
    public string CategoriaId { get; set; }

    [ValidateNever]
    public CategoriaModel Categoria { get; set; }
    [Required(ErrorMessage = "Necessario preencher o tipo de transação!")]
    public string TransacaoId { get; set; }
    
    [ValidateNever]
    public TransacaoModel Transacao { get; set; }
}