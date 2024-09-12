using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Songcash.Dto;
using Songcash.Helpers;
using Songcash.Service;

namespace Songcash.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly RequestService _requestService;

    public RequestsController(RequestService requestService)
    {
        _requestService = requestService;
    }

    // POST: api/requests
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rawToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var token = JwtHelper.FromRawTokenToJwtToken(rawToken);
        var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value!;

        var createdRequest = await _requestService.CreateRequest(requestDto, email);

        return Created($"/api/requests/{createdRequest.InsertedId}", createdRequest.Request);
    }

    // PUT: api/requests
    [HttpPut]
    //[Authorize]
    public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _requestService.UpdateRequest(requestDto);

        return Ok(new
        {
            Message = "Request created successfully",
            RequestId = result.Id,
        });
    }
}
