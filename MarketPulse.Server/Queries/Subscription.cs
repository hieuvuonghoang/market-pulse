using MarketPulse.Server.Models;

namespace MarketPulse.Server.Queries;

public class Subscription
{
    [Subscribe]
    [Topic("StockPriceUpdated")]
    public StockQuote OnStockPriceUpdated([EventMessage] StockQuote stockQuote) => stockQuote;

}
