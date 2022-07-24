using System;
using System.Data.SqlClient;

namespace MinionNames
{
    public class Program
    {
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(Config.ConfigName);
            sqlConnection.Open();
            string villian = Console.ReadLine();
            string command = @$"SELECT v.Name AS Vill, m.Name AS Min, m.Age
                                  FROM MINIONS AS m
                             LEFT JOIN MinionsVillains AS mv
                                    ON m.Id=mv.MinionId
                             LEFT JOIN Villains 
                                    AS v 
                                    ON v.Id=mv.VillainId
                                 WHERE VillainId = {villian}
                              ORDER BY m.Name";
            SqlCommand cmd=new SqlCommand(command, sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string vName = (string)reader["Vill"];
                if (vName == null)
                {
                    Console.WriteLine($"No villain with ID {villian} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {vName}");
                    int i = 1;
                    string mName = (string)reader["Min"];
                    int age = (int)reader["Age"];
                    Console.WriteLine($"{i++}. {mName} {age}");
                }
            }
            reader.Close();
            sqlConnection.Close();
        }
    }
}
