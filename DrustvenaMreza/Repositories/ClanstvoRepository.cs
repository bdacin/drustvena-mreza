using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class ClanstvoRepository
    {
        private readonly string _connectionString;

        public ClanstvoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteConnection");
        }

        // GRUPA + ČLANOVI
        public Grupa GetGroupWithMembers(int groupId)
        {
            var grupa = new Grupa();
            grupa.Clanovi = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                // 1. uzmi grupu
                string groupQuery = "SELECT Id, Name, CreationDate FROM Groups WHERE Id = @Id";

                using SqliteCommand groupCmd = new SqliteCommand(groupQuery, connection);
                groupCmd.Parameters.AddWithValue("@Id", groupId);

                using SqliteDataReader groupReader = groupCmd.ExecuteReader();

                if (!groupReader.Read())
                    return null;

                grupa.Id = Convert.ToInt32(groupReader["Id"]);
                grupa.Ime = groupReader["Name"].ToString();

                groupReader.Close();

                // 2. uzmi članove (JOIN)
                string membersQuery = @"
                    SELECT u.Id, u.KorisnickoIme, u.Ime, u.Prezime, u.DatumRodjenja
                    FROM GroupMemberships gm
                    JOIN Korisnici u ON gm.UserId = u.Id
                    WHERE gm.GroupId = @GroupId";

                using SqliteCommand membersCmd = new SqliteCommand(membersQuery, connection);
                membersCmd.Parameters.AddWithValue("@GroupId", groupId);

                using SqliteDataReader reader = membersCmd.ExecuteReader();

                while (reader.Read())
                {
                    grupa.Clanovi.Add(new Korisnik(
                        Convert.ToInt32(reader["Id"]),
                        reader["KorisnickoIme"].ToString(),
                        reader["Ime"].ToString(),
                        reader["Prezime"].ToString(),
                        DateTime.Parse(reader["DatumRodjenja"].ToString())
                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }

            return grupa;
        }

        //  DODAJ ČLANA U GRUPU
        public bool AddUserToGroup(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "INSERT INTO GroupMemberships (UserId, GroupId) VALUES (@UserId, @GroupId)";

                using SqliteCommand cmd = new SqliteCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@GroupId", groupId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }
        }

        //  UKLONI ČLANA IZ GRUPE
        public bool RemoveUserFromGroup(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "DELETE FROM GroupMemberships WHERE UserId = @UserId AND GroupId = @GroupId";

                using SqliteCommand cmd = new SqliteCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@GroupId", groupId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }
        }
    }
}