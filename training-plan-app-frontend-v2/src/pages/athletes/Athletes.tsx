import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, IconButton, Paper, TextField, Typography } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { AthletesApi, AthletesPagedListDTO, AthleteDTO } from '../../apis';
import InfoIcon from '@mui/icons-material/Info';
import EditIcon from '@mui/icons-material/Edit';
import AddIcon from '@mui/icons-material/Add';
import PersonIcon from '@mui/icons-material/RunCircle'; // Import the icon


const Athletes: React.FC = () => {
  const navigate = useNavigate();
  const [athletes, setAthletes] = useState<AthleteDTO[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(0);
  const [lastId, setLastId] = useState(0);
  const [pageSize, setPageSize] = useState(30);
  const [filterName, setFilterName] = useState('');

  useEffect(() => {
    const fetchAthletes = async () => {
      try {
        const api = new AthletesApi();
        const response = await api.apiAthletesGet(filterName, pageSize, lastId, 'asc');
        const data: AthletesPagedListDTO = response.data;
        setAthletes(data.items || []);
        setTotal(data.total || 0);
        if (data.items && data.items.length > 0) {
          setLastId(data.items[data.items.length - 1].id || 0);
        }
      } catch (error) {
        console.error('Failed to fetch athletes:', error);
      }
    };

    fetchAthletes();
  }, [filterName, page, pageSize]);

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };

  const handlePageSizeChange = (newPageSize: number) => {
    setPageSize(newPageSize);
    setPage(0);
  };

  const handleDetailsClick = (id: number) => {
    // Handle details action
    console.log(`Details clicked for athlete with id: ${id}`);
  };

  const handleEditClick = (id: number) => {
    // Handle edit action
    console.log(`Edit clicked for athlete with id: ${id}`);
  };

  const handleNewAthleteClick = () => {    
    navigate('/athletes/new');
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      setFilterName(event.currentTarget.value);
    }
  };

  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', flex: 0.5 },
    { field: 'name', headerName: 'Name', flex: 1 },
    { field: 'birth', headerName: 'Birth', flex: 1 },
    { field: 'email', headerName: 'Email', flex: 1.5 },
    { field: 'phone', headerName: 'Phone', flex: 1 },
    {
      field: 'actions',
      headerName: 'Actions',
      flex: 0.5,
      renderCell: (params) => (
        <Box>
          <IconButton color="primary" onClick={() => handleDetailsClick(params.row.id)}>
            <InfoIcon />
          </IconButton>
          <IconButton color="secondary" onClick={() => handleEditClick(params.row.id)}>
            <EditIcon />
          </IconButton>
        </Box>
      ),
    },
  ];

  return (
    <Box > {/* Adjust paddingTop to match the height of the AppBar */}
      <Paper sx={{ width: '100%', height: '100%', overflow: 'hidden', padding: '10px' }}>
        <Box sx={{ display: 'flex',   alignItems: 'center', marginBottom: '30px' }}>
          {/* <PersonIcon fontSize="large" color='primary' /> Add the icon here */}
          <Typography variant="h6">Athletes Management</Typography>
        </Box>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
          <TextField
            label="Filter by Name"
            variant="outlined"
            value={filterName}
            // onKeyDown={handleKeyDown}
            sx={{ marginRight: '16px' }}
            fullWidth
          />
          <Button
            variant="outlined"
            color="primary"
            startIcon={<AddIcon />} // Add the icon here
            onClick={handleNewAthleteClick}
          >
            Athlete
          </Button>
        </Box>

        <DataGrid
          rows={athletes}
          columns={columns}
          pagination
          paginationMode="server"
          rowCount={total}
          paginationModel={{ pageSize, page }}
          onPaginationModelChange={(model) => {
            handlePageSizeChange(model.pageSize);
            handlePageChange(model.page);
          }}
          autoHeight
        />
      </Paper>
    </Box>
  );
};

export default Athletes;