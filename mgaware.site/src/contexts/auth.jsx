import React, { createContext, useEffect } from "react";
import { useState } from "react"
import axios from 'axios';
import {
    json,
    useNavigate
} from "react-router-dom"

export const AuthContext = createContext();


export const AuthProvider = ({children}) => {
    const navigate = useNavigate();
    const [user, setuser] = useState(null); 
    const [loading, setLoading] = useState(true);
    const [msgLogin, setmsgLogin] = useState("");


    useEffect(() => {
        const logedUser = localStorage.getItem('userResponse');

        if (logedUser)
        {
            setuser(JSON.parse(logedUser))
        }
        setLoading(false);

    }, []);  

    const login = (email, password) => {
        
        const config = {
            headers: {
                'Content-Type': 'application/json',
                'accept': 'application/json;odata.metadata=minimal;odata.streaming=true'
            }
        };
        
        const url = 'http://localhost:5282/api/v1/Login';
        const data =  { 'userID': email, 'password': password };
        axios.post(url, data, config)
        .then((response) => {
            setuser(response.data);
            localStorage.setItem("user", email);
            localStorage.setItem("userResponse", JSON.stringify(response.data));  
            navigate("/home");
        })
        .catch(() => {
            setmsgLogin('Acesso negado. Nome de usuário ou senha inválido(s).');
        });
    };

 

    const logout = () => {
        console.log("logout");
        setuser(null);
        navigate("/");
    };


    const cleanMsgs = () => {
        setmsgLogin('');
    };

    return(
        <AuthContext.Provider value={{authenticade: !!user, user, loading, msgLogin, cleanMsgs, login, logout}}>
            {children}
        </AuthContext.Provider>
    );

}
