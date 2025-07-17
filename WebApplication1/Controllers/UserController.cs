using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserRepository  _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> Get()
    {
        List<User> users = await _userRepository.GetAll();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        User? user = await _userRepository.GetById(id);
        if(user == null) return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post([FromBody] RegisterUserDto registerUserDto)
    {
        User? user = await _userRepository.Create(registerUserDto);

        return Created();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Put(int id, [FromBody] RegisterUserDto registerUserDto)
    {
        User? user = await _userRepository.GetById(id);

        User? updUser = await _userRepository.Put(registerUserDto);
        
        return Ok(updUser);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> Delete(int id)
    {
        var deletedUser = await _userRepository.Delete(id);

        return deletedUser ? NoContent() : NotFound();
    }

    [HttpPost("/Login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginUserDto? input)
    {
        (User? user , string? token) = await _userRepository.Login(input);
        if (user == null) return Unauthorized("User not found");

        return Ok(new { Token = token, User = user });
    }
}