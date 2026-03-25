using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private KorisnikDBRepository _repo;

        public KorisnikController()
        {
            _repo = new KorisnikDBRepository();
        }

        // GET svi korisnici (DB)
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            var korisnici = _repo.GetAll();
            return Ok(korisnici);
        }

        // GET jedan korisnik (DB)
        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            var korisnik = _repo.GetById(korisnikId);

            if (korisnik == null)
                return NotFound();

            return Ok(korisnik);
        }
    }
}