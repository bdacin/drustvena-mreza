namespace DrustvenaMreza.Models
{
    public class Grupa
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime DatumOsnivanja { get; set; }

        public List<Korisnik> Clanovi { get; set; } = new List<Korisnik>();
    }
}