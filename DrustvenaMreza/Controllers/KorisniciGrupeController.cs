using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/grupe/{grupaId}/korisnici")]
    [ApiController]
    public class KorisniciGrupeController : ControllerBase
    {
        private readonly ClanstvoRepository _repo;

        public KorisniciGrupeController(IConfiguration configuration)
        {
            _repo = new ClanstvoRepository(configuration);
        }

        //  GET članovi grupe
        [HttpGet]
        public ActionResult<List<Korisnik>> GetKorisniciGrupe(int grupaId)
        {
            try
            {
                var grupa = _repo.GetGroupWithMembers(grupaId);

                if (grupa == null)
                    return NotFound();

                return Ok(grupa.Clanovi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        //  POST dodaj korisnika u grupu
        [HttpPost("{korisnikId}")]
        public IActionResult AddUser(int grupaId, int korisnikId)
        {
            try
            {
                bool success = _repo.AddUserToGroup(korisnikId, grupaId);

                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        //  DELETE izbaci korisnika iz grupe
        [HttpDelete("{korisnikId}")]
        public IActionResult RemoveUser(int grupaId, int korisnikId)
        {
            try
            {
                bool success = _repo.RemoveUserFromGroup(korisnikId, grupaId);

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