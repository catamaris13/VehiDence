import emailIcon from "../assets/email_img.png";
import passwordIcon from "../assets/password_img.png";
import userIcon from "../assets/username_img.png";
import phoneIcon from "../assets/phone_img.png";
import axios from 'axios';
import{ useNavigate} from "react-router-dom";

import React, { useState } from "react";
import '../index.css'

const SignUp = () => {
    const  history=useNavigate();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [token, setToken] = useState('');
    const [id, setId] = useState('');
    const [isValid, setisVaid] = useState('');
    const handleSignup = (e)=>{
        
        e.preventDefault();
        console.log(username);
        
        const data ={
          Username:username,
          Password:password,
          Email:email,
          Token:token,
          Name:name,
          PhoneNo:phoneNumber
        }
        const url='http://localhost:5277/api/User/Registration';
        axios.post(url,data)
        .then((result)=>{
            const dt=result.data;
            console.log(dt.StatusCode);
            
                console.log(data)

                history("/login");
                //window.location.href('/signup')
            
          })
          
        .catch((error)=>{
            console.log(error);
        })
    }

    const goToLogIn = () =>{
        history("/login");
    }

    return (
        <div className="login">
            <h1 className="text">Sign up</h1>
            <div className="inputs">
                <div className="input">
                    <img src={emailIcon} alt="" />
                    <input
                        type="text"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </div>

                <div className="input">
                    <img src={userIcon} alt="" />
                    <input
                        type="text"
                        placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </div>

                <div className="input">
                    <img src={userIcon} alt="" />
                    <input
                        type="text"
                        placeholder="Name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                </div>

                <div className="input">
                    <img src={passwordIcon} alt="" />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>
                <div className="input">
                    <img src={phoneIcon} alt="" />
                    <input
                        type="text"
                        placeholder="Phone"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                    />
                </div>
            </div>
            <div className="button-container">
                <button className="button-next" onClick={handleSignup}>Sign up</button>
                <button className="button-next" onClick={goToLogIn}>Login</button>
            </div>
        </div>
    );
};

export default SignUp;
