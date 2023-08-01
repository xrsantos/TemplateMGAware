import Reac from 'react';
import MenuScreen from '../MenuScreen'
import { Typography } from '@mui/material';


const HomePage = () => {

    return( 
        <>
            <MenuScreen title='Home'></MenuScreen>
            <div style={{ padding: '16px' }}>
                <Typography variant="h4">Bem-vindo(a) ao MGAware!</Typography>
            </div>
        </>
    );
}

export default HomePage