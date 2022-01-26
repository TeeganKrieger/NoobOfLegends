using Microsoft.AspNetCore.Mvc;

namespace NoobOfLegends_BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private static readonly string[] messages = new string[]
        {
            "Hello World!",
            "React is pretty neat?",
            "I couldn't think of other messages, so here I am!"
        };

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            
        }

        [HttpGet]
        public Object Get()
        {
            Random random = new Random();
            string message = messages[random.Next(0, messages.Length)];

            return new { Message = message };
        }
    }
}