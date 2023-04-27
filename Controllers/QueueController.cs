using APIQueueService.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIQueueService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<QueueController> _logger;

        private IBackgroundTaskQueue _queue;
        public static List<string> processedData = new List<string>();

        public QueueController(ILogger<QueueController> logger, IBackgroundTaskQueue queue)
        {
            _logger = logger;
            _queue = queue;
        }

        [HttpGet("queue")]
        public IActionResult QueueProcess(string data)
        {
            var today = DateTime.Now;
            _queue.QueueBackgroundWorkItem(async token =>
            {
                var random = new Random();
                await Task.Delay(random.Next(1000, 6000));
                var processDate = DateTime.Now;
                var str = $"{today} -> {data} -> {processDate}";
                processedData.Add(str);
            });

            return Ok();
        }

        [HttpGet("processed")]
        public List<string> GetProcess()
        {
            return processedData;
        }
    }
}
