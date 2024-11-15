using macrix_wk_backend.Interfaces;
using macrix_wk_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace macrix_wk_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IPersonService _personService;

        public HomeController(IPersonService personService)
        {
            _personService = personService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonModel>>> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonModel>> GetPersonById(long id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
        [HttpPost]
        public async Task<ActionResult> AddPersonAsync(PersonModel model)
        {
            await _personService.AddPersonAsync(model);
            return CreatedAtAction(nameof(GetPersonById), new { id = model.Id }, model);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PersonModel>> UpdatePerson(long id, PersonModel model)
        {
            try
            {
                await _personService.UpdatePersonAsync(id, model);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(long id)
        {
            try
            {
                await _personService.DeletePersonAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}