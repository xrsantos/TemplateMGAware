import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  GridRowModes,
  DataGrid,
  GridToolbarContainer,
  GridActionsCellItem,
  GridRowEditStopReasons,
} from '@mui/x-data-grid';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';

import { Container, CircularProgress } from '@mui/material';

function MGAdbgrid({ endpoint, columns, onRowSelected }) {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [columnsGrid, setcolumnsGrid] = useState(columns.push({'Name': 123}));

  console.log(
    columnsGrid
  );

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      const result = await axios.get(endpoint);
      setData(result.data.value);
      setLoading(false);
    };

    fetchData();
  }, []);

  return (
    
    <Container maxWidth="md">
      {loading ? (
        <CircularProgress />
      ) : (
        <div style={{ height: 800, width: '100%' }}>
          <DataGrid 
              editMode="row"
              checkboxSelection={false}
              rows={data} 
              columns={columnsGrid} 
              getRowId={(row) => row.id}
          />
        </div>
      )}
    </Container>
  );
}

export default MGAdbgrid;
