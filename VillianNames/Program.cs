using System;
using System.Data.SqlClient;
using System.Text;

namespace VillianNames
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(Config.ConfigName);
            sqlConnection.Open();
            int villainId = int.Parse(Console.ReadLine());
           string result = GetVillainsWithMinions(sqlConnection,villainId);
            Console.WriteLine(result);
            sqlConnection.Close();
        }

        private static string GetVillainsWithMinions(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder output = new StringBuilder();
            string villainName = @"SELECT Name 
                                     FROM Villains
                                    WHERE Id=@VillainId";
            SqlCommand command = new SqlCommand(villainName, sqlConnection);
            command.Parameters.AddWithValue("@VillainId", villainId);

            string villainNams=(string)command.ExecuteScalar();
            if (villainName==null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }
            output.AppendLine($"Villain: {villainNams}"); 

            string minionsQuery = @"SELECT m.Name, m.Age
                              FROM Minions AS m
                                 LEFT JOIN MinionsVillains AS mv
                                        ON m.Id=mv.MinionId
                                     WHERE mv.VillainId=@VillainId
ORDER BY m.Name";

            SqlCommand getMinions = new SqlCommand(minionsQuery, sqlConnection);
            getMinions.Parameters.AddWithValue("@VillainId", villainId);

            using SqlDataReader reader = getMinions.ExecuteReader();
            if (!reader.HasRows)
            {
                output.AppendLine($"(no minions)");
            }
            else
            {
                while (reader.Read())
                {
                    int rowNumber = 1;

                    output.AppendLine($"{rowNumber++}. {reader["Name"]} - {reader["Age"]}");

                }
            }
            return output.ToString();

        }
    }
}
