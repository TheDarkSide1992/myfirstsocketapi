using Dapper;
using Npgsql;

namespace socketAPIFirst;

public class Reposetory
{
    private readonly NpgsqlDataSource _dataSource;

    public Reposetory(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public void saveMessage(int dtoRoomId, string message, string UserName)
    {
        var sql = $@"INSERT INTO FirstFullStackProjectFourthSemester.message( roomId, name,message) 
                VALUES (@dtoRoomId, @UserName, @message);
        ";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                conn.Query(sql, new { dtoRoomId, UserName, message});
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }
        }
    }
    
    public IEnumerable<ServerBroadcastMessageToRoom> getMessage(int romid)
    {
        var sql = $@"
SELECT name as {nameof(ServerBroadcastMessageToRoom.UserName)}, 
       message.message as {nameof(ServerBroadcastMessageToRoom.Message)} 
FROM FirstFullStackProjectFourthSemester.message WHERE roomid = @romid;
        ";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Query<ServerBroadcastMessageToRoom>(sql , new{romid});
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }
        }

        return null;
    }
}