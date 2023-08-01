import {
    BrowserRouter as Router,
    Route,
    Routes,
    Navigate
} from "react-router-dom"

import LoginPage from "../pages/login"
import HomePage from "../pages/HomePage"
import { AuthProvider ,AuthContext } from "../contexts/auth"
import { useContext } from "react"
import Person from "../pages/person"

const AppRoutes = () =>
{
    const Private = ({children}) =>{
        const {authenticade, loading} = useContext(AuthContext);
        if (loading)
        {
            return <div className="loading">Carregando...</div>
        }

        if (!authenticade)
        {
            return <Navigate to="/" />
        }
        return children;
    }

    return(
        
        <Router> 
            <AuthProvider>     
                <Routes>
                    <Route exact path="/" element={<LoginPage />} />
                    <Route exact path="/home" element={<Private> <HomePage /> </Private>} />
                    <Route exact path="/person" element={<Private> <Person /> </Private>} />
                </Routes>
            </AuthProvider>
        </Router>
    

    )
}

export default AppRoutes