using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _7.PrintAllMinionNames
{
    public class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(Config.ConfigPath);
            sqlConnection.Open();
            List<string> minions = GetMinionsNames(sqlConnection);
            Console.WriteLine(string.Join("\n",minions));
            sqlConnection.Close();

        }

        public static List<string> GetMinionsNames(SqlConnection sqlConnection)
        {
            List<string> names = new List<string>();
            string namesQuery = @"SELECT Name
                                    FROM Minions";
            SqlCommand sqlCommand = new SqlCommand(namesQuery, sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string name = sqlDataReader.GetString(0);
                names.Add(name);
            }
            List<string> newList = new List<string>();
            List<string> result = new List<string>();
                for (int i = names.Count - 1; i >= 0; i--)
                {
                    if (i >= names.Count / 2)
                    {
                        newList.Add(names[i]);
                    }
                else
                {
                    break;
                }
                }
                for (int i = 0; i < newList.Count; i++)
                {
                    result.Add(names[i]);
                    result.Add(newList[i]);
                }
            if (names.Count%2!=0)
            {
                 string lastName = names[names.Count / 2 + 1];
                result.Add(lastName);
            }
            return result;
        }
    }
}
