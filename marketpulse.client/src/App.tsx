import React from 'react';
import { Container } from '@chakra-ui/react';
import StockList from './components/StockList';

const App: React.FC = () => {
  return (
    <Container maxW="container.xl" p={4}>
      <StockList />
    </Container>
  );
};

export default App;
