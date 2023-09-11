// using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using LittleFish.Core;
using LittleFish.Core.Models;
using LittleFish.Core.Services;
// using LittleFish.Api.Controllers;

namespace LittleFish.Api.Controllers; 

[Controller]
[Route("api/[controller]")]
// public class UserController: ApiControllerBase {
public class UserController: Controller {
    
    private readonly MongoDBService _mongoDBService;

    public UserController(MongoDBService mongoDBService) {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<User>> GetUsersAsync() {
        return await _mongoDBService.GetAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<List<User>> GetUserAsync() {
        return await _mongoDBService.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] User user) {
        await _mongoDBService.CreateAsync(user);
        return CreatedAtAction(nameof(User), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] User user) {
        await _mongoDBService.UpdateAsync(id, user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(string id) {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }
}
