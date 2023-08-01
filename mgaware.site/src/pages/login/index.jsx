import React, { useState } from 'react';
import { TextField, Button, Container, Typography } from '@mui/material';
import { AuthContext } from '../../contexts/auth';  
import { useContext } from 'react';


const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    
    const [error, setError] = useState("");
    const {authenticade, msgLogin, cleanMsgs, login} = useContext(AuthContext);
    


    const handleLogin = () => {
        login(email, password);
    };

  return (
    <Container maxWidth="xs">
      <div style={{ marginTop: '20px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        <Typography variant="h5">Login</Typography>
        <TextField
          label="Email"
          type="email"
          value={email}
          onChange={(e) => {
              setEmail(e.target.value);
              cleanMsgs('');
            }
          }
          style={{ margin: '10px', width: '100%' }}
        />
        <TextField
          label="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          style={{ margin: '10px', width: '100%' }}
        />
        <Button variant="contained" color="primary" onClick={handleLogin} style={{ margin: '10px', width: '100%' }}>
          Login
        </Button>
        <Typography color="error">{String(msgLogin)}</Typography>
      </div>
    </Container>
  );
};

export default LoginPage;
