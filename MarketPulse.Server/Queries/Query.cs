using MarketPulse.Server.Models;
using MarketPulse.Server.Services;

namespace MarketPulse.Server.Queries;

public class Query
{
    public async Task<List<StockQuote>> GetStockQuotes([Service] FinnhubService finnhubService)
    {
        return await finnhubService.GetMultipleStockQuotesAsync(new List<string> { "AAPL", "MSFT", "AMZN", "NVDA", "BTC-USD" });
    }
}