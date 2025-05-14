import React, { useEffect, useState } from "react";
import { useSubscription, gql } from "@apollo/client";
import {
  Box,
  Flex,
  Heading,
  Text,
  VStack,
  HStack,
  Badge,
} from "@chakra-ui/react";

// Define the GraphQL subscription for real-time updates
const STOCK_PRICE_SUBSCRIPTION = gql`
  subscription OnStockPriceUpdated {
    onStockPriceUpdated {
      symbol
      currentPrice
      highPrice
      lowPrice
      previousClosePrice
      openPrice
    }
  }
`;

interface StockQuote {
  symbol: string;
  currentPrice: number;
  highPrice: number;
  lowPrice: number;
  openPrice: number;
  previousClosePrice: number;
}

const StockList: React.FC = () => {
  const {
    data: subscriptionData,
    error,
    loading,
  } = useSubscription<{ onStockPriceUpdated: StockQuote }>(
    STOCK_PRICE_SUBSCRIPTION
  );
  const [stocks, setStocks] = useState<StockQuote[]>([]);

  useEffect(() => {
    if (loading) {
      console.log("Subscription is loading...");
    }

    if (error) {
      console.error("Subscription error:", error.message); // Log any subscription errors
    }

    if (subscriptionData) {
      const updatedStock = subscriptionData.onStockPriceUpdated;
      setStocks((prevStocks) => {
        const find = prevStocks.findIndex(
          (ele) => ele.symbol === updatedStock.symbol
        );
        if (find === -1) {
          return [...prevStocks, updatedStock]
        } else {
          return prevStocks.map((stock) => {
            return stock.symbol === updatedStock.symbol ? updatedStock : stock;
          });
        }
      });
    }
  }, [subscriptionData, loading, error]);

  return (
    <VStack align="start" p={4}>
      <Heading size="md" mb={4}>
        Stock Prices
      </Heading>
      <Flex wrap="wrap" justify="space-between" width="100%">
        {stocks.map((stock) => (
          <Box
            key={stock.symbol}
            p={4}
            shadow="md"
            borderWidth="1px"
            borderRadius="lg"
            width="30%"
            mb={4}
          >
            <HStack justify="space-between">
              <Text fontWeight="bold">{stock.symbol}</Text>
              <Badge
                colorPalette={
                  stock.currentPrice >= stock.previousClosePrice
                    ? "green"
                    : "red"
                }
              >
                {stock.currentPrice >= stock.previousClosePrice ? "+" : ""}
                {(
                  ((stock.currentPrice - stock.previousClosePrice) /
                    stock.previousClosePrice) *
                  100
                ).toFixed(2)}
                %
              </Badge>
            </HStack>
            <Text mt={2}>Price: ${stock.currentPrice.toFixed(2)}</Text>
            <Text>High: ${stock.highPrice.toFixed(2)}</Text>
            <Text>Low: ${stock.lowPrice.toFixed(2)}</Text>
          </Box>
        ))}
      </Flex>
    </VStack>
  );
};

export default StockList;
