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

        public KorisnikController(IConfiguration configuration)
        {
            _repo = new KorisnikDBRepository(configuration);
        }

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            try
            {
                return Ok(_repo.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            try
            {
                var korisnik = _repo.GetById(korisnikId);

                if (korisnik == null)
                    return NotFound();

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<Korisnik> Create(Korisnik korisnik)
        {
            try
            {
                var novi = _repo.Create(korisnik);
                return Ok(novi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik> Update(int korisnikId, Korisnik korisnik)
        {
            try
            {
                korisnik.Id = korisnikId;

                bool success = _repo.Update(korisnik);

                if (!success)
                    return NotFound();

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        [HttpDelete("{korisnikId}")]
        public IActionResult Delete(int korisnikId)
        {
            try
            {
                bool success = _repo.Delete(korisnikId);

                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }
    }
}