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

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            var korisnik = _repo.GetById(korisnikId);

            if (korisnik == null)
                return NotFound();

            return Ok(korisnik);
        }

        [HttpPost]
        public ActionResult<Korisnik> Create(Korisnik korisnik)
        {
            var novi = _repo.Create(korisnik);

            if (novi == null)
                return StatusCode(500);

            return Ok(novi);
        }

        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik> Update(int korisnikId, Korisnik korisnik)
        {
            korisnik.Id = korisnikId;

            bool success = _repo.Update(korisnik);

            if (!success)
                return NotFound();

            return Ok(korisnik);
        }

        [HttpDelete("{korisnikId}")]
        public IActionResult Delete(int korisnikId)
        {
            bool success = _repo.Delete(korisnikId);

            if (!success)
                return NotFound();

            return Ok();
        }
    }
}