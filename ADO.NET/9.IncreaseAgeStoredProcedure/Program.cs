using System;
using System.Data.SqlClient;

namespace _9.IncreaseAgeStoredProcedure
{
    public class Program
    {
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());
            SqlConnection increaseAge=new SqlConnection(Config.ConfigPath);
            increaseAge.Open();
            string result = IncreaseMinionAge(increaseAge, id);
            Console.WriteLine(result);
            increaseAge.Close();
        }

        private static string IncreaseMinionAge(SqlConnection sqlConnection, int id)
        {
            string increaseAgeQuery = @"EXEC [dbo].[usp_GetOlder] @Id";
            SqlCommand increaseAgeCmd = new SqlCommand(increaseAgeQuery, sqlConnection);
            increaseAgeCmd.Parameters.AddWithValue("@Id", id);
            increaseAgeCmd.ExecuteNonQuery();

            string minionNameAndAge = @"SELECT [Name], [Age]
                                          FROM Minions
	                                     WHERE Id = @Id";
            SqlCommand findNameAndAge = new SqlCommand(minionNameAndAge, sqlConnection);
            findNameAndAge.Parameters.AddWithValue("@Id", id);
             using SqlDataReader sqlDataReader = findNameAndAge.ExecuteReader();
            string name = "";
            int age = 0;
            while (sqlDataReader.Read())
            {
             name = (string)sqlDataReader["Name"];
             age = (int)sqlDataReader["Age"];
            }
            return $"{name} – {age} years old";
        } 
    }
}
