using Dapper;
using Models;
using System.Data;

public class DAL_User : IDAL_User
{
    private readonly IDbConnection _db;

    public DAL_User(IDbConnection db)
    {
        _db = db;
    }

    public async Task<ProfileResponseDto> GetProfileById(long userId)
    {
        var sql = @"
            SELECT 
                u.user_id AS UserId,
                u.name AS Name,
                r.role_name AS Role,
                b.branch_name AS Branch,
                u.profile_pic AS ProfilePic
            FROM users u
            LEFT JOIN roles r ON u.role_id = r.role_id
            LEFT JOIN branches b ON u.branch_id = b.branch_id
            WHERE u.user_id = @userId
              AND u.status = 1
        ";

        return await _db.QueryFirstOrDefaultAsync<ProfileResponseDto>(
            sql, new { userId });
    }
}