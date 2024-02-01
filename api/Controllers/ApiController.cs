using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api")]
public class ApiController : Controller
{
    private readonly MyDbContext _dbContext;

    public ApiController(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("person/{id:int}")]
    public Person GetPerson(int id)
    {
        return FetchPerson(id);
    }

    [HttpPut("person/{id:int}")]
    public Person UpdatePerson(int id, [FromBody] Person person)
    {
        person.Id = id;
        SavePerson(person);
        return FetchPerson(person.Id);
    }

    [HttpGet("person-dto/{id:int}")]
    public PersonDto GetPersonDto(int id)
    {
        return FetchPersonDto(id);
    }

    [HttpPut("person-dto/{id:int}")]
    public Person UpdatePersonDto(int id, [FromBody] PersonDto person)
    {
        person.Id = id;
        SavePerson(person);
        return FetchPerson(person.Id);
    }

    [HttpGet("subscriptions")]
    public List<Subscription> GetSubscriptions()
    {
        return FetchSubscriptions();
    }

    private Person FetchPerson(int id)
    {
        var person = _dbContext.Persons.First(p => p.Id == id);
        return person;
    }

    private PersonDto FetchPersonDto(int id)
    {
        var person = _dbContext.Persons
            .Include(_ => _.Subscription)
            .Where(p => p.Id == id)
            .Select(p => new PersonDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                SubscriptionName = p.Subscription.Name
            })
            .First();

        return person;
    }

    private void SavePerson(Person person)
    {
        var entity = _dbContext.Persons.First(p => p.Id == person.Id);
        foreach (var property in person.GetType().GetProperties())
        {
            var newValue = property.GetValue(person);
            if (newValue != null && property.CanWrite)
            {
                property.SetValue(entity, newValue);
            }
        }

        _dbContext.SaveChanges();
    }

    private void SavePerson(PersonDto person)
    {
        var entity = _dbContext.Persons.First(p => p.Id == person.Id);
        entity.FirstName = person.FirstName;
        entity.LastName = person.LastName;
        _dbContext.SaveChanges();
    }

    private List<Subscription> FetchSubscriptions()
    {
        return _dbContext.Subscriptions.ToList();
    }
}