using System;
using System.Data.SqlClient;
using System.Text;

namespace _6.RemoveVillain
{
    public class Program
    {
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());
            SqlConnection sqlConnection = new SqlConnection(Config.ConfigPath);
            sqlConnection.Open();
            string result = DeleteVillain(sqlConnection,id);
            Console.WriteLine(result);
        }

        private static string DeleteVillain(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder output = new StringBuilder();
            string villainQuery = @"SELECT Name
                                      FROM Villains
                                     WHERE Id = @Id";
            SqlCommand sqlCommand = new SqlCommand(villainQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id",villainId);
            string name = (string)sqlCommand.ExecuteScalar();
            if (name==null)
            {
                return "No such villain was found.";
            }


            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            string releaseMinionsQuery = @"DELETE FROM MinionsVillains
                                            WHERE VillainId = @Id";
            SqlCommand releaseCmd = new SqlCommand(releaseMinionsQuery, sqlConnection, sqlTransaction);
            releaseCmd.Parameters.AddWithValue("@Id", villainId);
           int releasedMinionsCmd= releaseCmd.ExecuteNonQuery();

            string deleteVillain = @"DELETE FROM Villains
                                           WHERE Id = @Id";
            SqlCommand delVillCmd = new SqlCommand(deleteVillain, sqlConnection, sqlTransaction);
            delVillCmd.Parameters.AddWithValue("@Id", villainId);
            int villainsDeleted=delVillCmd.ExecuteNonQuery();

            if (villainsDeleted!=1)
            {
                sqlTransaction.Rollback();
            }
            sqlTransaction.Commit();
            output.AppendLine($"{name} was deleted.");
            output.AppendLine($"{releasedMinionsCmd} minions were released.");
            return output.ToString().TrimEnd();
        }
    }
}
