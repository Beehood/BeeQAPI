using Models;

public class BAL_User : IBAL_User
{
    private readonly IDAL_User _dal;

    public BAL_User(IDAL_User dal)
    {
        _dal = dal;
    }

    public async Task<ProfileResponseDto> GetProfileById(long userId)
    {
        return await _dal.GetProfileById(userId);
    }
}