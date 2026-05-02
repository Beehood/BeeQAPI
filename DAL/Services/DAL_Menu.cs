using DAL.ContractIF;
using Dapper;
using Models;
using System.Data;

public class DAL_Menu : IDAL_Menu
{
    private readonly IDbConnection _db;

    public DAL_Menu(IDbConnection db)
    {
        _db = db;
    }

    public async Task<List<MenuModel>> GetSidebar(string email)
    {
        try
        {
            var result = await _db.QueryAsync<MenuModel>(
                "sp_get_sidebar",
                new { p_email = email },
                commandType: CommandType.StoredProcedure
            );

            return result?.ToList() ?? new List<MenuModel>();
        }
        catch (Exception ex)
        {
            throw new Exception("DAL: Error fetching sidebar", ex);
        }
    }
}