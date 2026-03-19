using DrustvenaMreza.Models;
using System.Globalization;

namespace DrustvenaMreza.Repositories
{
    public class KorisnikRepository
    {
        private const string filePath = "CSVs/korisnici.csv";

        public static Dictionary<int, Korisnik> Data;

        public KorisnikRepository()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Korisnik>();

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');

                int id = int.Parse(attributes[0]);
                string username = attributes[1];
                string ime = attributes[2];
                string prezime = attributes[3];

                DateTime datum = DateTime.ParseExact(
                    attributes[4],
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture
                );

                Korisnik korisnik = new Korisnik(id, username, ime, prezime, datum);

                Data[id] = korisnik;
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();

            foreach (Korisnik k in Data.Values)
            {
                string datum = k.DatumRodjenja.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                lines.Add($"{k.Id},{k.KorisnickoIme},{k.Ime},{k.Prezime},{datum}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
