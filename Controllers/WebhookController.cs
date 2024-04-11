using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class WebhookController : Controller
{
    [HttpPost("")]
    public async Task<IActionResult> HandleWebhook([FromBody] dynamic payload)
    {
        Console.WriteLine(payload); 
        return Ok();
    }
}