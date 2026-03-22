using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        // GET svi korisnici
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            KorisnikRepository repo = new KorisnikRepository();
            List<Korisnik> korisnici = KorisnikRepository.Data.Values.ToList();
            return Ok(korisnici);
        }

        // GET jedan korisnik
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

        // POST - dodavanje korisnika
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

        // PUT - izmena korisnika
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