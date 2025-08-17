namespace GerenciamentoFinanceiro.Models;

public class FiltroModel
{
    public FiltroModel(string filtro)
    {
        FiltroString = filtro ?? "todos-todos-todos";
        string[] filtros = FiltroString.Split('-');

        CategoriaId = filtros[0];
        DataOperacao = filtros[1];
        TransacaoId = filtros[2];
    }
    public string FiltroString { get; set; }
    public string CategoriaId { get; set; }
    public string TransacaoId { get; set; }
    public string DataOperacao { get; set; }

    public bool FiltroCategoria => CategoriaId.ToLower() != "todos";
    public bool FiltroTransacao => TransacaoId.ToLower() != "todos";
    public bool FiltroDataOperacao => DataOperacao.ToLower() != "todos";

    public static Dictionary<string, string> ListaDataOperacao =>
        new Dictionary<string, string>()
        {
            {"passado", "Passado"},//{chave, valor}
            {"futuro", "Futuro"},
            {"hoje", "Hoje"}
        };
    
    public bool EhPassado => DataOperacao.ToLower() == "passado";
    public bool EhFuturo => DataOperacao.ToLower() == "futuro";
    public bool EhHoje => DataOperacao.ToLower() == "hoje";
}