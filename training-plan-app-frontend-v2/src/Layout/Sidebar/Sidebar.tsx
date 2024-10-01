import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { Drawer, List, ListItemButton, ListItemText, IconButton, useMediaQuery, useTheme, Toolbar } from '@mui/material';
import { Menu as MenuIcon } from '@mui/icons-material';
import styled from '@emotion/styled';


const SidebarContainer = styled(Drawer)`
  width: 250px;
  flex-shrink: 0;
  & .MuiDrawer-paper {
    width: 250px;
    box-sizing: border-box;
  }
`;

interface SidebarProps {
  isOpen: boolean;
  toggleSidebar: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, toggleSidebar }) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  return (
    <>
      <IconButton
        color="inherit"
        aria-label="open drawer"
        edge="start"
        onClick={toggleSidebar}
        sx={{ position: 'fixed', top: 16, left: 16, zIndex: 1300 }}
      >
        <MenuIcon />
      </IconButton>
      <SidebarContainer
        variant={isMobile ? 'temporary' : 'persistent'}
        anchor="left"
        open={isOpen}
        onClose={toggleSidebar}
      >
        <Toolbar />


        <List>
          <ListItemButton component={RouterLink} to="/" onClick={toggleSidebar}>
            <ListItemText primary="Dashboard" />
          </ListItemButton>
          <ListItemButton component={RouterLink} to="/athletes">
            <ListItemText primary="Athletes" />
          </ListItemButton>
          <ListItemButton component={RouterLink} to="/settings" onClick={toggleSidebar}>
            <ListItemText primary="Settings" />
          </ListItemButton>
        </List>

      </SidebarContainer>
    </>
  );
}

export default Sidebar;