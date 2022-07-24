using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace _5.ChangeTownNamesCasing
{
    public class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(Config.ConfigName);
            sqlConnection.Open();
            string country = Console.ReadLine();
            int countryCode = FindCountryId(country,sqlConnection);
            Console.WriteLine(CountOfTheChangedNames(sqlConnection, countryCode));
            Console.WriteLine(ChangedNames(sqlConnection, countryCode));
            sqlConnection.Close();
        }

        public static string ChangedNames(SqlConnection sqlConnection, int countryId)
        {
            List<string> output = new List<string>();
            string townsQuery = @"SELECT ALL
                                   UPPER (Name)
                                    FROM Towns
                                    WHERE CountryCode=@CountryId";
            SqlCommand sqlCommand = new SqlCommand(townsQuery,sqlConnection);
            sqlCommand.Parameters.AddWithValue("@CountryId", countryId);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string name = sqlDataReader.GetString(0);
                output.Add(name);
            }
            string result = string.Join(", ", output);
            return $"[{result}]";
        }
        public static int FindCountryId(string name, SqlConnection sqlConnection)
        {
            string countryIdQuery = @"SELECT Id
                                        FROM Countries
                                       WHERE Name = @Name";
            SqlCommand sqlCommand = new SqlCommand(countryIdQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name",name);
            object countryId = sqlCommand.ExecuteScalar();
            return (int)countryId;
        }

        public static string CountOfTheChangedNames(SqlConnection sqlConnection, int countryId)
        {
            StringBuilder output = new StringBuilder();
            string countQuery = @"SELECT COUNT(Name)
                                    FROM Towns
                                   WHERE CountryCode = @CountryId";
            SqlCommand sqlCommand = new SqlCommand(countQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@CountryId", countryId);
            int count = (int)sqlCommand.ExecuteScalar();
            if (count == 0)
            {
                return $"No town names were affected.";
            }
            return $"{count} town names were affected.";
        }
    }
}
