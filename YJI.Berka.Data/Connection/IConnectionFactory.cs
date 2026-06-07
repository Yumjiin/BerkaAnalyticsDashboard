using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace YJI.Berka.Data.Connection;

// <summary>
// DB 연결 생성 인터페이스
// </summary>
public interface IConnectionFactory
{
    IDbConnection Create();
}
