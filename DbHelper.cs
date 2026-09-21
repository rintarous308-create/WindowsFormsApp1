using Microsoft.Data.SqlClient;

namespace WindowsFormsApp1
{
    public static class DbHelper
    {
        // แก้ Server และ Database ให้ตรงกับเครื่องของคุณ
        private const string ConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=Midori;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}