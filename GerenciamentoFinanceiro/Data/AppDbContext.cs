using GerenciamentoFinanceiro.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoFinanceiro.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
    {
    }
    
    public DbSet<CategoriaModel> Categoria { get; set; }
    public DbSet<TransacaoModel> Transacao { get; set; }
    public DbSet<FinanceiroModel> Financeiro { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaModel>().HasData(
            new CategoriaModel
            {
                CategoriaId = "educacao",
                Nome = "Educação"
            },
            new CategoriaModel
            {
                CategoriaId = "salario",
                Nome = "Salário"
            },
            new CategoriaModel
            {
                CategoriaId = "viagem",
                Nome = "Viagem"
            },
            new CategoriaModel
            {
                CategoriaId = "mercado",
                Nome = "Mercado"
            }
        );

        modelBuilder.Entity<TransacaoModel>().HasData(
            new TransacaoModel
            {
                TransacaoId = "ganho",
                Nome = "Ganho"
            },
            new TransacaoModel
            {
                TransacaoId = "perda",
                Nome = "Perda"
            }
        );
        
        base.OnModelCreating(modelBuilder);
    }
}