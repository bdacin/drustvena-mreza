using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class KorisnikController:ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Korisnik>>GetAll()
        {
            KorisnikRepository repo = new KorisnikRepository();
            List<Korisnik>korisnici=KorisnikRepository.Data.Values.ToList();
            return Ok(korisnici);
        }
        //Get jedan korisnik
        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            KorisnikRepository repo = new KorisnikRepository();
            if (!KorisnikRepository.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }
            Korisnik korisnik=KorisnikRepository.Data[korisnikId];
            return Ok(korisnik);
        }
        //Post 
        [HttpPost]
        public ActionResult<Korisnik>Create(Korisnik korisnik)
        {
            KorisnikRepository repo=new KorisnikRepository();
            KorisnikRepository.Data[korisnik.Id] = korisnik;
            repo.Save();
            return Ok(korisnik);
        }
        //PUT
        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik>Update(int korisnikId, Korisnik korisnik)
        {
            KorisnikRepository repo = new KorisnikRepository();
            if (!KorisnikRepository.Data.ContainsKey (korisnikId))
            { return NotFound(); }
            korisnik.Id= korisnikId;
            KorisnikRepository.Data [korisnik.Id]= korisnik;
            repo.Save();
            return Ok(korisnik);
        }
    }
}
