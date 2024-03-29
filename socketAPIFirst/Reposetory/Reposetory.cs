﻿using Dapper;
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
            conn.Query(sql, new { dtoRoomId, UserName, message });
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
            return conn.Query<ServerBroadcastMessageToRoom>(sql, new { romid });
        }
    }

    public void banUser(string userName)
    {
        var sql = $@"INSERT INTO FirstFullStackProjectFourthSemester.banedUser(banedUsername) 
                VALUES (@userName);
        ";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql, new {userName});
        }
    }

    public bool isUserBaned(string userName)
    {
        var sql = $@"
SELECT banedusername FROM FirstFullStackProjectFourthSemester.banedUser WHERE  banedUsername = @userName;
        ";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { userName }) != 0;
        }
    }
}