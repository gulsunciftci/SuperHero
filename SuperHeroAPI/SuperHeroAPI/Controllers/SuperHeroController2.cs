using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Data;
using SuperHeroAPI.Models;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController2 : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SuperHeroController2(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            var SuperHeroes = await _dataContext.SuperHeroes.ToListAsync();
            return Ok(SuperHeroes);

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<SuperHero>>> Get(int id)
        {
            var hero = _dataContext.SuperHeroes.Where(b => b.Id == id).SingleOrDefaultAsync();
            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            return Ok(hero);

        }
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero([FromBody]SuperHero
             superHero)
        {
            _dataContext.SuperHeroes.Add(superHero);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());

        }
        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(request.Id);
            if (dbHero == null)
                return BadRequest("Hero not found.");

            dbHero.Name = request.Name;
            dbHero.FirstName = request.FirstName;
            dbHero.LastName = request.LastName;
            dbHero.Place = request.Place;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(id);
            if (dbHero == null)
                return BadRequest("Hero not found.");

            _dataContext.SuperHeroes.Remove(dbHero);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());

        }

        [HttpPatch("{id:int}")] //Puttan farkı putta nesneyi bir bütün olarak güncelliyoruz burada ise kısmi güncelleme yapabiliyoruz.
        // Normalde bir array içinde tanımlanır
        public async Task<ActionResult<List<SuperHero>>> PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<SuperHero> superHeroPatch)
        {
            //check entity
            var entity = await _dataContext.SuperHeroes.Where(b => b.Id.Equals(id)).SingleOrDefaultAsync();
            if (entity is null)
            {
                return NotFound(); //404
            }

            superHeroPatch.ApplyTo(entity);
            return NoContent(); //204
        }
    }
}
