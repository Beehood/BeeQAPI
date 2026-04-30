using Models;

public interface IBAL_User
{
    Task<ProfileResponseDto> GetProfileById(long userId);
}