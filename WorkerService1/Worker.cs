using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var data = new
            {
                a = 16,
                b = 15,
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                try
                {
                    var response = await _httpClient.PostAsJsonAsync("http://localhost:5269/api/arth/sum", data, stoppingToken);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Response: {Response}", responseContent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending POST"); 
                }

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
