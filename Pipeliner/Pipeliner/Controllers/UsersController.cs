using Microsoft.AspNetCore.Mvc;
using Pipeliner.Models;
using Pipeliner.Services;


namespace Pipeliner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService) =>
        _usersService = usersService;

    [HttpGet]
    public async Task<List<UserGetResponse>> Get()
    {
        var users = await _usersService.Get();

        var responses = new List<UserGetResponse>();

        foreach (var user in users)
            responses.Add(new UserGetResponse(user.Id, user.Name));

        return responses;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserGetResponse>> Get(string id)
    {
        var user = await _usersService.Get(id);

        if (user is null)
            return NotFound();

        return new UserGetResponse(user.Id, user.Name);
    }
}
