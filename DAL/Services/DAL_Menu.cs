using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

public class DAL_Menu : IDAL_Menu
{

    private readonly DBConnection _config;

    public DAL_Menu(DBConnection config)
    {
        _config = config;
    }


    public async Task<List<MenuModel>> GetSidebar(string email)
    {
        try
        {
            using var conn = new MySqlConnection(_config.DefaultConnection);

            var result = await conn.QueryAsync<MenuModel>("sp_get_sidebar", new { p_email = email }, commandType: CommandType.StoredProcedure);

            return result?.ToList() ?? new List<MenuModel>();
        }
        catch (Exception ex)
        {
            throw new Exception("DAL: Error fetching sidebar", ex);
        }
    }
}