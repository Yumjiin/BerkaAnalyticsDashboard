using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace YJI.Berka.Data.Connection;

// <summary>
// MySQL 연결 생성 팩토리
// appsettings.json 의 ConnectionStrings:BerkaDb 를 읽어서 연결 생성
// </summary>
public class ConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BerkaDb")
            ?? throw new InvalidOperationException("ConnectionStrings:BerkaDb 설정이 없습니다.");
    }

    public IDbConnection Create()
    {
        return new MySqlConnection(_connectionString);
    }
}
