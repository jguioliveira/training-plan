import React from 'react';
import { Box, Typography } from '@mui/material';

const MainContent: React.FC = () => {
  return (
    <Box component="main" sx={{ flexGrow: 1, p: 3, mt: 8, mb: 4 }}>
      <Typography variant="h4" gutterBottom>
        Welcome to the Admin Dashboard
      </Typography>
      <Typography variant="body1">This is the main content area.</Typography>
    </Box>
  );
};

export default MainContent;
