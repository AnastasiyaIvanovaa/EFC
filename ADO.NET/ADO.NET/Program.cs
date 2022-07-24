using System;
using System.Data.SqlClient;

namespace ADO.NET
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using  SqlConnection sqlConnection = new SqlConnection(Config.ConfigPath);
            sqlConnection.Open();
            Console.WriteLine("Connection completed!");
            string query = "SELECT COUNT(*) FROM Employees";
            SqlCommand sqlCommand= new SqlCommand(query,sqlConnection);
            int count=(int)sqlCommand.ExecuteScalar();
            Console.WriteLine(count);
            string lastNae = Console.ReadLine();
            string emplInfo = @$"SELECT FirstName, LastName FROM Employees";
            SqlCommand empIn = new SqlCommand(emplInfo,sqlConnection);
            using SqlDataReader sqlData = empIn.ExecuteReader();
            int i = 1;

            while (sqlData.Read())
            {
                string firstName = (string)sqlData["FirstName"];
                string lastName = (string)sqlData["LastName"];

                Console.WriteLine($"{i++}. {firstName} {lastName}");
            }
            sqlData.Close();
            sqlConnection.Close();
        }
    }
}
