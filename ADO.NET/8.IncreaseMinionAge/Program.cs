using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace _8.IncreaseMinionAge
{
    public class Program
    {
        static void Main(string[] args)
        {
            int[] ids=Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            SqlConnection sqlConnection = new SqlConnection(Config.ConfigPath);
            sqlConnection.Open();
            Console.WriteLine(ChangeMinionsInfo(sqlConnection,ids));
            sqlConnection.Close();
        }

        public static string ChangeMinionsInfo(SqlConnection sqlConnection, int[] ids)
        {
            StringBuilder output = new StringBuilder();

            string changeName = @"SELECT ALL
                                   LOWER (Name)
                                    FROM Minions
                                   WHERE Id =@Id";
            string changeAge = @"UPDATE Minions
                                    SET Age+=1
                                  WHERE Id=@Id";
            SqlCommand sqlCommand = new SqlCommand(changeName,sqlConnection);
            SqlCommand sqlCommand1 = new SqlCommand(changeAge,sqlConnection);
            for (int i = 0; i < ids.Length; i++)
            {
                sqlCommand.Parameters.AddWithValue("@Id", ids[i]);
                sqlCommand.ExecuteNonQuery();
                sqlCommand1.Parameters.AddWithValue("@Id", ids[i]);
                sqlCommand1.ExecuteNonQuery();
            }
            string outputQuery = @"SELECT Name, Age
                                     FROM Minions";
            SqlCommand sqlCommand2 = new SqlCommand(outputQuery,sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string name = (string)sqlDataReader["Name"];
                int age = (int)sqlDataReader["Age"];
                output.AppendLine($"{name} {age}");
            }
            return output.ToString().TrimEnd();
        }
    }
}
