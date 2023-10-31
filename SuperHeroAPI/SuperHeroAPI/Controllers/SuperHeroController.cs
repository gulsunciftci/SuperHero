using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Models;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private static List<SuperHero> hereos = new List<SuperHero>
        {
            new SuperHero
            {
                Id = 1,
                Name = "Spider Man",
                FirstName = "Peter",
                LastName = "Parker",
                Place = "New york city"
            }


        };


        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {

            return Ok(hereos);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Get(int id)
        {
            var hero = hereos.Find(b => b.Id == id);
            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            return Ok(hereos);

        }
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero([FromBody]SuperHero
             superHero)
        {
            hereos.Add(superHero);
            return Ok(hereos);

        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero superHero)
        {
            var hero = hereos.Find(b => b.Id == superHero.Id);
            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            hero.Name = superHero.Name;
            hero.FirstName = superHero.FirstName;
            hero.LastName = superHero.LastName;
            hero.Place = superHero.Place;



            return Ok(hero);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<List<SuperHero>>> Hero(int id)
        {
            var hero = hereos.Find(b => b.Id == id);
            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            hereos.Remove(hero);

            return Ok(hero);

        }

    }
}

