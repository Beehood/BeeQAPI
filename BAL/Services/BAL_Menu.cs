using BAL.ContractIF;
using DAL.ContractIF;
using Models;
/// <summary>
/// Menu BAL - Get Sidebar Menu
/// Description:- Retrieves the sidebar menu items based on the authenticated user's roles and permissions.
/// Access:
/// - Authenticated Users
/// </summary>
public class BAL_Menu : IBAL_Menu
{
    private readonly IDAL_Menu _dal;

    public BAL_Menu(IDAL_Menu dal)
    {
        _dal = dal;
    }

    public async Task<List<MenuModel>> GetSidebar(string email)
    {
        try
        {
            var data = await _dal.GetSidebar(email);

            return data ?? new List<MenuModel>();
        }
        catch (Exception ex)
        {
            throw new Exception("BAL: Error processing sidebar", ex);
        }
    }
}