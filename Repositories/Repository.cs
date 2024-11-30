using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ATDapi.Repositories;

public class Repository
{
    private readonly string dbConexion = "SERVER=server-terciario.hilet.com,11333;DATABASE=rokev;UID=sa;PWD=1234!\"qwerQW;TrustServerCertificate=True";
    public Repository() { }

    //este es para los inserts
    public async Task<dynamic> ExecuteProcedure(string procedureName, DynamicParameters parameters)
    {
        using (SqlConnection connection = new SqlConnection(dbConexion))
        {
            try
            {
                return await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public async Task<List<T>> GetListFromProcedure<T>(string procedureName, DynamicParameters parameters)
    {
        using (SqlConnection connection = new SqlConnection(dbConexion))
        {
            IEnumerable<T> rows = await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return rows.AsList();
        }
    }

    public async Task<List<T>> GetListFromProcedure<T>(string procedureName)
    {
        using (SqlConnection connection = new SqlConnection(dbConexion))
        {
            IEnumerable<T> rows = await connection.QueryAsync<T>(procedureName, commandType: CommandType.StoredProcedure);
            return rows.AsList();
        }
    }
}




