import React from 'react';
import { Typography, Box } from '@mui/material';

const Settings: React.FC = () => {
  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
      Settings
      </Typography>
      <Typography variant="body1">
        Welcome to the Settings Page.
      </Typography>
    </Box>
  );
}

export default Settings;