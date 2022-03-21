using Microsoft.AspNetCore.Mvc;
using Pipeliner.Models;
using Pipeliner.Services;

namespace Pipeliner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PipelinesController : ControllerBase
{
    private readonly UsersService _usersService;
    private readonly TasksService _tasksService;
    private readonly PipelinesService _pipelinesService;
    private readonly TaskPipelineMappingsService _mappingsService;

    public PipelinesController(
        UsersService usersService,
        TasksService tasksService,
        PipelinesService pipelinesService,
        TaskPipelineMappingsService mappingsService
        )
    {
        _usersService = usersService;
        _tasksService = tasksService;
        _pipelinesService = pipelinesService;
        _mappingsService = mappingsService;
    }

    [HttpGet]
    public async Task<List<PipelineGetResponse>> Get()
    {
        var pipelines = await _pipelinesService.Get();
        var responses = new List<PipelineGetResponse>();

        foreach (var pipeline in pipelines)
        {
            var response = await CreateResponse(pipeline);
            if (response != null)
                responses.Add(response);
        }

        return responses;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<PipelineGetResponse>> Get(string id)
    {
        var pipeline = await _pipelinesService.Get(id);

        if (pipeline is null)
            return NotFound();

        var response = await CreateResponse(pipeline);
        if (response is null)
            return NotFound();

        return response;
    }

    [HttpPost]
    public async Task<ActionResult<Pipeline>> CreatePipeline(PipelineCreateRequest createForm)
    {
        if (! await CheckCreateRequest(createForm))
            return NotFound("User or some tasks not found!");

        var pipeline = new Pipeline(createForm.UserId);
        await _pipelinesService.Create(pipeline);

        foreach (var taskId in createForm.TasksIds)
        {
            var mapping = new TaskPipelineMapping(taskId, pipeline.Id);
            await _mappingsService.Create(mapping);
        }

        return CreatedAtAction(nameof(Get), pipeline);
    }

    [HttpPost("{id:length(24)}/{taskId:length(24)}")]
    public async Task<ActionResult<Pipeline>> AddTask(
        string taskId, string id,
        [FromBody] PipelineEditRequest request
        )
    {
        var task = await _tasksService.Get(taskId);
        var pipeline = await _pipelinesService.Get(id);

        if (task is null || pipeline is null)
            return NotFound();

        if (!CheckEditRequest(pipeline, request))
            return StatusCode(403);

        var mapping = new TaskPipelineMapping(taskId, id);
        await _mappingsService.Create(mapping);

        return Ok(pipeline);
    }

    [HttpDelete("{id:length(24)}/{taskId:length(24)}")]
    public async Task<ActionResult<Pipeline>> RemoveTask(
        string taskId, string id, 
        [FromBody] PipelineEditRequest request
        )
    {
        var pipeline = await _pipelinesService.Get(id);
        var mapping = await _mappingsService.Get(taskId, id);

        if (mapping is null || pipeline is null)
            return NotFound();

        if (!CheckEditRequest(pipeline, request))
            return StatusCode(403);

        await _mappingsService.Delete(mapping.Id);

        return Ok(pipeline);
    }

    [HttpPost("{id:length(24)}")]
    public async Task<ActionResult<PipelineRunResponse?>> Run(string id)
    {
        var pipeline = await _pipelinesService.Get(id);

        if (pipeline is null)
            return NotFound();

        var record = await _pipelinesService.Run(pipeline);
        var response = new PipelineRunResponse(record.RunTime);
        return response;
    }

    private async Task<PipelineGetResponse?> CreateResponse(Pipeline pipeline)
    {
        var mappings = await _mappingsService.Get(pipeline.Id);
        var tasksIds = mappings.Select(m => m.TaskId).ToList();

        var response = new List<string>();

        foreach (var taskId in tasksIds)
        {
            var task = await _tasksService.Get(taskId);
            if (task is null)
                return null;
            response.Add(task.Id);
        }

        var countTime = await CountPipelineTime(pipeline.Id);

        return new PipelineGetResponse(pipeline, tasksIds, countTime);
    }

    private async Task<decimal> CountPipelineTime(string id)
    {
        var countTime = 0.0M;
        var mappings = await _mappingsService.Get(id);
        foreach (var mapping in mappings)
        {
            var task = await _tasksService.Get(mapping.TaskId);
            countTime += task.AverageTime;
        }

        return countTime;
    }

    private async Task<bool> CheckCreateRequest(PipelineCreateRequest request)
    {
        var user = await _usersService.Get(request.UserId);

        if (user is null)
            return false;

        foreach (var taskId in request.TasksIds)
        {
            var task = await _tasksService.Get(taskId);
            if (task == null)
                return false;
        }

        return true;
    }

    private bool CheckEditRequest(Pipeline pipeline, PipelineEditRequest request) =>
        request.UserId == pipeline.UserId;
}
