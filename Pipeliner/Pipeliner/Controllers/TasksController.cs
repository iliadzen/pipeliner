using Microsoft.AspNetCore.Mvc;
using Pipeliner.Models;
using Pipeliner.Services;
using CustomTask = Pipeliner.Models.CustomTask;


namespace Pipeliner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly UsersService _usersService;
    private readonly TasksService _tasksService;

    public TasksController(
        UsersService usersService,
        TasksService tasksService
       )
    {
        _usersService = usersService;
        _tasksService = tasksService;
    }

    [HttpGet]
    public async Task<List<TaskGetResponse>> Get()
    {
        var tasks = await _tasksService.Get();

        var responses = new List<TaskGetResponse>();

        foreach (var task in tasks)
            responses.Add(new TaskGetResponse(task));

        return responses;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<TaskGetResponse>> Get(string id)
    {
        var task = await _tasksService.Get(id);

        if (task is null)
            return NotFound();

        return new TaskGetResponse(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskGetResponse>> CreateTask(TaskCreateRequest createForm)
    {
        var user = await _usersService.Get(createForm.UserId);

        if (user is null)
            return NotFound("User not found!");

        var task = new CustomTask(createForm.UserId, createForm.Name, createForm.AverageTime);
        await _tasksService.Create(task);
        var response = new TaskGetResponse(task);

        return CreatedAtAction(nameof(Get), response);
    }
}
