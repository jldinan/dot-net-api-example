using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository repository;
    private readonly ILogger<UsersController> logger;

    public UsersController(ILogger<UsersController> logger, IUserRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await repository.GetAll();
        if (users is null)
        {
            return NoContent();
        }
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await repository.GetById(id);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        await repository.Create(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut]
    public async Task<ActionResult<User>> UpdateUser(User user)
    {
        var updatedUser = await repository.Update(user);
        if (updatedUser is null)
        {
            return NotFound();
        }
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteById(int id)
    {
        bool userDeleted = await repository.DeleteById(id);
        if (userDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }    
}
