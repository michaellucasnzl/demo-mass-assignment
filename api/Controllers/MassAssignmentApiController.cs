using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("mass-assignment")]
public class MassAssignmentApiController : Controller
{
    private readonly MyDbContext _dbContext;

    public MassAssignmentApiController(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet("person")]
    public IActionResult GetPerson(int id)
    {
        try
        {
            return Ok(FetchPerson(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("person")]
    public IActionResult UpdatePerson([FromBody] Person person)
    {
        try
        {
            SavePerson(person);
            return Ok(FetchPerson(person.Id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("persondto")]
    public IActionResult UpdatePersonDto([FromBody] PersonDto person)
    {
        try
        {
            SavePerson(person);
            return Ok(FetchPerson(person.Id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private Person FetchPerson(int id)
    {
        var person = _dbContext.Persons.First(p => p.Id == id);
        return person;
    }

    private void SavePerson(Person person)
    {
        var entity = _dbContext.Persons.First(p => p.Id == person.Id);
        _dbContext.Entry(entity).CurrentValues.SetValues(person);
        _dbContext.SaveChanges();
    }
    
    private void SavePerson(PersonDto person)
    {
        var entity = _dbContext.Persons.First(p => p.Id == person.Id);
        entity.FirstName = person.FirstName;
        entity.LastName = person.LastName;
        _dbContext.SaveChanges();
    }
}