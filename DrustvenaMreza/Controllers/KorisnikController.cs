using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly string _connectionString = "Data Source=Data/drustvena_mreza.db";

        // GET svi korisnici (IZ BAZE)
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            try
            {
                var korisnici = GetAllFromDatabase();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        private List<Korisnik> GetAllFromDatabase()
        {
            var korisnici = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id, KorisnickoIme, Ime, Prezime, DatumRodjenja FROM Korisnici";

                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    try
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
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Greška u formatu podataka: {ex.Message}");
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri radu sa bazom: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Greška sa konekcijom: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return korisnici;
        }

        // GET jedan korisnik (CSV)
        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            KorisnikRepository repo = new KorisnikRepository();

            if (!KorisnikRepository.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            return Ok(KorisnikRepository.Data[korisnikId]);
        }

        // POST - dodavanje korisnika (CSV)
        [HttpPost]
        public ActionResult<Korisnik> Create(Korisnik korisnik)
        {
            KorisnikRepository repo = new KorisnikRepository();

            int newId = 1;
            if (KorisnikRepository.Data.Count > 0)
            {
                newId = KorisnikRepository.Data.Keys.Max() + 1;
            }

            korisnik.Id = newId;

            KorisnikRepository.Data[korisnik.Id] = korisnik;

            repo.Save();

            return Ok(korisnik);
        }

        // PUT - izmena korisnika (CSV)
        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik> Update(int korisnikId, Korisnik korisnik)
        {
            KorisnikRepository repo = new KorisnikRepository();

            if (!KorisnikRepository.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            korisnik.Id = korisnikId;

            KorisnikRepository.Data[korisnik.Id] = korisnik;

            repo.Save();

            return Ok(korisnik);
        }
    }
}