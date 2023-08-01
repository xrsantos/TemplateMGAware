import Reac, {useState} from 'react';
import MenuScreen from '../MenuScreen'
import MGAdbgrid from '../../components/mgadbgrid';
import { Container, CircularProgress, Dialog, DialogTitle, DialogContent, Typography } from '@mui/material';



function Maintenance({ row }) {
    return (
      <Dialog open={true}>
        <DialogTitle>Manutenção Pessoa</DialogTitle>
        <DialogContent>
          <Typography>Nome: {row.name}</Typography>
          <Typography>Email: {row.email}</Typography>
        </DialogContent>
      </Dialog>
    );
  }

const Person = () => {
    const [selectedRow, setSelectedRow] = useState(null);

    const columns = [
      { field: 'name', headerName: 'Nome', width: 150,  editable: false },
      { field: 'email', headerName: 'Email', width: 250, editable: false }
    ];

    const endpoint = 'http://localhost:5282/api/v1/Person/GetAll'


    return( 
            <>
                <div>
                    <MenuScreen title='Manutenção de Pessoas'></MenuScreen>
                    <div style={{ padding: '16px' }}>
                        <Typography variant="h4">Manutenção de Pessoas</Typography>
                    </div>
                </div>
                <div>
                    <MGAdbgrid 
                      columns={columns} 
                      endpoint={endpoint} >
                      onRowSelected={setSelectedRow}
                    </MGAdbgrid>
                </div>
                {selectedRow && <Maintenance row={selectedRow} />}
            </>
    );
}

export default Person