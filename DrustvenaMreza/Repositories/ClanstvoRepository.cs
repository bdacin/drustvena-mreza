using DrustvenaMreza.Models;

namespace DrustvenaMreza.Repositories
{
    public class ClanstvoRepository
    {
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "CSVs", "clanstva.csv");

        public static Dictionary<int, Clanstvo> Data;

        public ClanstvoRepository()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Clanstvo>();

            string[] lines = File.ReadAllLines(filePath);

            int i = 0;

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                int korisnikId = int.Parse(parts[0]);
                int grupaId = int.Parse(parts[1]);

                Clanstvo c = new Clanstvo(korisnikId, grupaId);

                Data[i++] = c;
            }
        }
    }
}