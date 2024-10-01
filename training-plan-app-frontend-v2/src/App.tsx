import React, { useState } from 'react';
import Sidebar from './Layout/Sidebar/Sidebar';
import Header from './Layout/Header/Header';
import Footer from './Layout/Footer/Footer';
import { ThemeProvider } from '@emotion/react';
import { Box, CssBaseline, useMediaQuery, useTheme } from '@mui/material';
import { lightTheme, darkTheme } from './Layout/Theme/theme';
import './App.css';
import { Outlet, useNavigation } from 'react-router-dom';

const App: React.FC = () => {
  const [isDarkMode, setIsDarkMode] = useState(false);
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const navigation = useNavigation();

  const toggleTheme = () => {
    setIsDarkMode(!isDarkMode);
  };

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  return (
    <ThemeProvider theme={isDarkMode ? darkTheme : lightTheme}>
      <CssBaseline />
      <div className="app">
        <Sidebar isOpen={isSidebarOpen} toggleSidebar={toggleSidebar} />
        <Box
          className={`main-layout ${navigation.state === 'loading' ? 'loading' : ''}`}
          sx={{
            top: 'auto',
            bottom: 0,
            width: isSidebarOpen && !isMobile ? `calc(100% - 250px)` : '100%',
            ml: isSidebarOpen && !isMobile ? '250px' : '0',
            transition: 'width 0.3s ease, margin-left 0.3s ease',
            display: 'flex',
            flexDirection: 'column',
            height: '100vh',
            position: 'fixed',
          }}
        >
          <Header
            toggleTheme={toggleTheme}
            isDarkMode={isDarkMode}
            isSidebarOpen={isSidebarOpen && !isMobile}
          />
          <div style={{ flexGrow: 1, overflow: 'auto' , paddingTop: '100px', width: '100%', height: '100%' }}>
            <Outlet />
          </div>
          <Footer isSidebarOpen={isSidebarOpen && !isMobile} />
        </Box>
      </div>
    </ThemeProvider>
  );
};

export default App;