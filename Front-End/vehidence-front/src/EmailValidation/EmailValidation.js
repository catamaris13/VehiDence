import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import axios from 'axios';
import{ useNavigate} from "react-router-dom";

const EmailValidation = () => {
    const  navigate = useNavigate();
    const location = useLocation();
    const [loading, setLoading] = useState(true);
    const [validationSuccess, setValidationSuccess] = useState(false);

    const handleHomePage = () =>{
        navigate("/home")
    }

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const username = searchParams.get('username');
        const token = searchParams.get('token');

        if (username && token) {
            axios.get(`http://localhost:5277/api/User/ValidateEmail?username=${username}&token=${token}`)
                .then(response => {
                    if(response.data.statusCode == 200){
                        console.log(response.data);
                        setValidationSuccess(true); 
                    }
                    else{
                        setValidationSuccess(false);
                    }
                    
                })
                .catch(error => {
                    console.error('Error:', error);
                    setValidationSuccess(false); 
                })
                .finally(() => {
                    setLoading(false); 
                });
        } else {
            setLoading(false);
            setValidationSuccess(false);
        }
    }, [location.search]);

    if (loading) {
        return <div>Loading...</div>; 
    } else {
        if (validationSuccess) {
            return (
                <div className="validation">
                    <h1 className="text">Email Validation Successful</h1>
                    <h2 className="text" style={{ fontSize: '50px' }}>Return to Home page</h2>
                    <button className="button" onClick={handleHomePage} style={{ display: 'flex', flexDirection: 'column', cursor:'pointer'  }}>Home Page</button>
                </div>
            );
        } else {
            return (
                <div className="validation">
                    <h1 className="text">Email Validation Failed</h1>
                   
                </div>
            );
        }
    }
};

export default EmailValidation;





