using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/grupe/{grupaId}/korisnici")]
    [ApiController]
    public class KorisniciGrupeController : ControllerBase
    {
        private KorisnikRepository korisnikRepository = new KorisnikRepository();
        private ClanstvoRepository clanstvoRepository = new ClanstvoRepository();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetKorisniciGrupe(int grupaId)
        {
            List<Korisnik> korisniciGrupe = new List<Korisnik>();

            foreach (Clanstvo c in ClanstvoRepository.Data.Values)
            {
                if (c.GrupaId == grupaId)
                {
                    korisniciGrupe.Add(KorisnikRepository.Data[c.KorisnikId]);
                }
            }

            return Ok(korisniciGrupe);
        }
    }
}