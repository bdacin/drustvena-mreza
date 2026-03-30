using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class KorisnikDBRepository
    {
        private readonly string _connectionString;

        public KorisnikDBRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteConnection");
        }

        public List<Korisnik> GetAll(int page, int pageSize)
        {
            var korisnici = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                int offset = (page - 1) * pageSize;

                string query = @"SELECT Id, KorisnickoIme, Ime, Prezime, DatumRodjenja 
                                 FROM Korisnici 
                                 LIMIT @PageSize OFFSET @Offset";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", offset);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var korisnik = new Korisnik(
                        Convert.ToInt32(reader["Id"]),
                        reader["KorisnickoIme"].ToString(),
                        reader["Ime"].ToString(),
                        reader["Prezime"].ToString(),
                        DateTime.Parse(reader["DatumRodjenja"].ToString())
                    );

                    korisnici.Add(korisnik);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }

            return korisnici;
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Korisnici";

                using SqliteCommand command = new SqliteCommand(query, connection);

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }
        }

        public Korisnik GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id, KorisnickoIme, Ime, Prezime, DatumRodjenja FROM Korisnici WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Korisnik(
                        Convert.ToInt32(reader["Id"]),
                        reader["KorisnickoIme"].ToString(),
                        reader["Ime"].ToString(),
                        reader["Prezime"].ToString(),
                        DateTime.Parse(reader["DatumRodjenja"].ToString())
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }

            return null;
        }

        public Korisnik Create(Korisnik korisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = @"INSERT INTO Korisnici (KorisnickoIme, Ime, Prezime, DatumRodjenja) 
                                 VALUES (@KorisnickoIme, @Ime, @Prezime, @DatumRodjenja);
                                 SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@KorisnickoIme", korisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Ime", korisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", korisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                int newId = Convert.ToInt32(command.ExecuteScalar());
                korisnik.Id = newId;

                return korisnik;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }
        }

        public bool Update(Korisnik korisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = @"UPDATE Korisnici 
                                 SET KorisnickoIme = @KorisnickoIme,
                                     Ime = @Ime,
                                     Prezime = @Prezime,
                                     DatumRodjenja = @DatumRodjenja
                                 WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@KorisnickoIme", korisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Ime", korisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", korisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Id", korisnik.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "DELETE FROM Korisnici WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
                throw;
            }
        }
    }
}