using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Importacao.Dados
{
    public class DbSessao : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public DbSessao(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration
                     .GetConnectionString("DefaultConnection"));
            Connection.Open();
        }
        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
