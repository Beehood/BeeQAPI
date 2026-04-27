using BAL.ContractIF;
using DAL.ContractIF;
using Models;

public class BAL_Menu : IBAL_Menu
{
    private readonly IDAL_Menu _dal;

    public BAL_Menu(IDAL_Menu dal)
    {
        _dal = dal;
    }

    public async Task<List<MenuModel>> GetSidebar(long userId)
    {
        try
        {
            var data = await _dal.GetSidebar(userId);

            return data ?? new List<MenuModel>();
        }
        catch (Exception ex)
        {
            throw new Exception("BAL: Error processing sidebar", ex);
        }
    }
}