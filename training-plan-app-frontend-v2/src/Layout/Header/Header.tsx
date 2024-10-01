import React from 'react';
import { AppBar, Toolbar, Typography, IconButton, Box } from '@mui/material';
import { Brightness4, Brightness7 } from '@mui/icons-material';

interface HeaderProps {
  toggleTheme: () => void;
  isDarkMode: boolean;
  isSidebarOpen: boolean;
}

const Header: React.FC<HeaderProps> = ({
  toggleTheme,
  isDarkMode,
  isSidebarOpen,
}) => {
  return (
    <AppBar
      position="fixed"
      sx={{
        width: isSidebarOpen ? `calc(100% - 250px)` : '100%',
        ml: isSidebarOpen ? '250px' : '0',
        transition: 'width 0.3s ease, margin-left 0.3s ease',
      }}
    >
      <Toolbar>
        <Box sx={{ flexGrow: 1 }} />
        <Typography variant="h6" noWrap component="div">
          Admin Dashboard
        </Typography>
        <IconButton
          edge="end"
          color="inherit"
          aria-label="mode"
          onClick={toggleTheme}
          sx={{ ml: 2 }}
        >
          {isDarkMode ? <Brightness7 /> : <Brightness4 />}
        </IconButton>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
