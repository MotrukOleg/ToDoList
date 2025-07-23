
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.BLL.Commands.Users.Create;
using ToDoList.BLL.Commands.Users.Login;
using ToDoList.BLL.Queries.User;
using ToDoList.DAL.Models;


namespace ToDoList.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _mediator.Send(new GetUserListQuery()));
    }

    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateUserCommand request)
    {
        return Ok(await _mediator.Send(request));
    }


    [HttpPost("Login")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(User), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> Login([FromBody] LoginCommand? input)
    {
        return Ok(await _mediator.Send(input));
    }
}