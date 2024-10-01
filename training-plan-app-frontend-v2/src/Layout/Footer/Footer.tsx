import React from 'react';
import { AppBar, Toolbar, Typography } from '@mui/material';

interface FooterProps {
  isSidebarOpen: boolean;
}

const Footer: React.FC<FooterProps> = ({ isSidebarOpen }) => {
  return (
    <AppBar
      position="fixed"
      sx={{
        top: 'auto',
        bottom: 0,
        width: isSidebarOpen ? `calc(100% - 250px)` : '100%',
        ml: isSidebarOpen ? '250px' : '0',
        transition: 'width 0.3s ease, margin-left 0.3s ease',
      }}
    >
      <Toolbar>
        <Typography
          variant="body2"
          color="inherit"
          align="center"
          sx={{ flexGrow: 1 }}
        >
          © 2023 Company Name. All rights reserved.
        </Typography>
      </Toolbar>
    </AppBar>
  );
};

export default Footer;
