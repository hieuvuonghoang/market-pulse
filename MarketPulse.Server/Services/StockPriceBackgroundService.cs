using HotChocolate.Subscriptions;
using MarketPulse.Server.Models;
using MarketPulse.Server.Queries;

namespace MarketPulse.Server.Services;

public class StockPriceBackgroundService : BackgroundService
{
    private readonly ILogger<StockPriceBackgroundService> _logger;
    private readonly FinnhubService _finnhubService;
    private readonly ITopicEventSender _eventSender;
    private static readonly List<string> Symbols = new() { "AAPL", "MSFT", "AMZN", "NVDA" };

    public StockPriceBackgroundService(
        ILogger<StockPriceBackgroundService> logger,
        FinnhubService finnhubService,
        ITopicEventSender eventSender)
    {
        _logger = logger;
        _finnhubService = finnhubService;
        _eventSender = eventSender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                foreach (var symbol in Symbols)
                {
                    var stockQuote = await _finnhubService.GetStockQuoteAsync(symbol);
                    if (stockQuote != null)
                    {
                        _logger.LogInformation($"Sending update for {symbol}: {stockQuote.CurrentPrice}");
                        await _eventSender.SendAsync("StockPriceUpdated", stockQuote, stoppingToken);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Poll every 10 seconds
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stock prices.");
            }
        }
    }
}

