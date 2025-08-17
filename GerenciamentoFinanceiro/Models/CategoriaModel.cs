using System.ComponentModel.DataAnnotations;

namespace GerenciamentoFinanceiro.Models;

public class CategoriaModel
{
    [Key]
    public string CategoriaId { get; set; }
    [Required(ErrorMessage = "Preencha o nome da categoria")]
    public string Nome { get; set; }
}