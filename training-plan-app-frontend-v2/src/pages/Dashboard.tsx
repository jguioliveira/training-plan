import React from 'react';
import { Typography, Box } from '@mui/material';

const Dashboard: React.FC = () => {
  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
        Home Page
      </Typography>
      <Typography variant="body1">
        Welcome to the Home Page.
      </Typography>
    </Box>
  );
}

export default Dashboard;