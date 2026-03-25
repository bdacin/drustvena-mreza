using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class KorisnikDBRepository
    {
        private readonly string _connectionString = "Data Source=Data/drustvena_mreza.db";

        public List<Korisnik> GetAll()
        {
            var korisnici = new List<Korisnik>();

            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, KorisnickoIme, Ime, Prezime, DatumRodjenja FROM Korisnici";

            using SqliteCommand command = new SqliteCommand(query, connection);
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

            return korisnici;
        }

        public Korisnik GetById(int id)
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

            return null;
        }
    }
}