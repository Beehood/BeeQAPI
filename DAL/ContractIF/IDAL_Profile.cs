using Models;

public interface IDAL_User
{
    Task<ProfileResponseDto> GetProfileById(long userId);
}