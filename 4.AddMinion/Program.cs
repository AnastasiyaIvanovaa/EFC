using System;
using System.Data.SqlClient;
using System.Text;

namespace _4.AddMinion
{
        public class StartUp
        {
            static void Main(string[] args)
            {


                string[] minionInfo = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                string villainName = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries)[1];

                using SqlConnection sqlConnection = new SqlConnection(Config.ConfigName);
                sqlConnection.Open();
                string result = AddNewMinion(minionInfo, sqlConnection, villainName);
                Console.WriteLine(result);
                sqlConnection.Close();
            }

            private static string AddNewMinion(string[] minionInfo, SqlConnection sqlConnection, string villainName)
            {
                StringBuilder output = new StringBuilder();
                string minionName = minionInfo[0];
                int minionAge = int.Parse(minionInfo[1]);
                string townName = minionInfo[2];

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    int townId = GetTownId(sqlConnection, sqlTransaction, townName, output);
                    int villainId = GetVillainId(sqlConnection, sqlTransaction, villainName, output);
                    int minionId = AddMinion(sqlConnection, sqlTransaction, minionName, minionAge, townId);

                    string addMinionToVillainQuery = @"INSERT INTO MinionsVillains(MinionId, VillainId)
                                                        VALUES (@MinionId, @VillainId) ";
                    SqlCommand addMinToVillQuery = new SqlCommand(addMinionToVillainQuery, sqlConnection, sqlTransaction);
                    addMinToVillQuery.Parameters.AddWithValue("@MinionId", minionId);
                    addMinToVillQuery.Parameters.AddWithValue("@VillainId", villainId);
                    addMinToVillQuery.ExecuteNonQuery();
                    output.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
                    sqlTransaction.Commit();
                }
                catch (Exception e)
                {
                    sqlTransaction.Rollback();
                    return e.ToString();
                }

                return output.ToString().TrimEnd();
            }

            private static int AddMinion(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string minionName, int minionAge, int townId)
            {
                string addMinionQuery = @"INSERT INTO Minions (Name, Age, TownId)
                                                VALUES (@Name, @Age, @TownId)";
                SqlCommand addMinionCmd = new SqlCommand(addMinionQuery, sqlConnection, sqlTransaction);
                addMinionCmd.Parameters.AddWithValue("@Name", minionName);
                addMinionCmd.Parameters.AddWithValue("@Age", minionAge);
                addMinionCmd.Parameters.AddWithValue("@TownId", townId);
                addMinionCmd.ExecuteNonQuery();

                string addedMinionId = @"SELECT Id
                                       FROM Minions
                                      WHERE Name = @Name AND Age = @Age AND TownId =@TownId";
                SqlCommand getMinionId = new SqlCommand(addedMinionId, sqlConnection, sqlTransaction);
                getMinionId.Parameters.AddWithValue("@Name", minionName);
                getMinionId.Parameters.AddWithValue("@Age", minionAge);
                getMinionId.Parameters.AddWithValue("@TownId", townId);

                int minionId = (int)getMinionId.ExecuteScalar();
                return minionId;
            }
            private static int GetVillainId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string villainName, StringBuilder output)
            {
                string villainIdQuery = @"SELECT Id
                                            FROM Villains
                                           WHERE Name=@VillainName";
                SqlCommand villainIdCommand = new SqlCommand(villainIdQuery, sqlConnection, sqlTransaction);
                villainIdCommand.Parameters.AddWithValue("@VillainName", villainName);
                object villainIdObject = villainIdCommand.ExecuteScalar();

                if (villainIdObject == null)
                {
                    string addVillainCommand = @"INSERT INTO Villains (Name, EvilnessFactorId)
                                                      VALUES ('@VillainName', 4)";
                    SqlCommand addVillain = new SqlCommand(addVillainCommand, sqlConnection, sqlTransaction);
                    addVillain.Parameters.AddWithValue("@VillainName", villainName);
                    addVillain.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    output.AppendLine($"Villain {villainName} was added to the database.");

                }
                villainIdObject = villainIdCommand.ExecuteScalar();
                return (int)villainIdObject;
            }
            private static int GetTownId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string townName, StringBuilder output)
            {
                string townNameQwery = @"SELECT Id 
                                           FROM Towns
                                          WHERE Name = @TownName";
                SqlCommand townIdCommand = new SqlCommand(townNameQwery, sqlConnection, sqlTransaction);
                townIdCommand.Parameters.AddWithValue("@TownName", townName);
                object townIdObject = townIdCommand.ExecuteScalar();

                if (townIdObject == null)
                {
                    string addTownQuery = @"INSERT INTO Towns (Name)
                                                 VALUES ('@Town')";
                    SqlCommand addTownCommand = new SqlCommand(addTownQuery, sqlConnection, sqlTransaction);
                    addTownCommand.Parameters.AddWithValue("@Town", townName);
                    addTownCommand.ExecuteNonQuery();
                    output.AppendLine($"Town {townName} was added to the database");
                    sqlTransaction.Commit();
                    townIdObject = townIdCommand.ExecuteScalar();

                }
                return (int)townIdObject;

            }
        }
    }

