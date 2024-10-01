import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Route, createBrowserRouter, createRoutesFromElements, RouterProvider } from 'react-router-dom';
import App from './App.tsx';
import './index.css';
import Dashboard from './pages/Dashboard';
import Settings from './pages/Settings';
import Athletes from './pages/athletes/Athletes.tsx';
import NewAthlete from './pages/athletes/New.tsx';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<App />}>
      <Route index element={<Dashboard />} />
      <Route path="athletes" element={<Athletes />} />
      <Route path="athletes/new" element={<NewAthlete />} />
      <Route path="settings" element={<Settings />} />
    </Route>
  )
);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
