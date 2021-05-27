using System.Data;

namespace AcessoChallenge.Infrastructure.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();

        IDbConnection GetReadConnection();
    }
}