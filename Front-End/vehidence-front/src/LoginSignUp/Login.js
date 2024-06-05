import emailIcon from "../assets/email_img.png";
import passwordIcon from "../assets/password_img.png";
import React, { useState, useContext } from "react";
import "../index.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";


const Login = () => {
  const history = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [token, setToken] = useState("");
  const [id, setId] = useState("");
  const [isValid, setisVaid] = useState("");
  const [isLogin, setIsLogin] = useState(false);
  const handleLogin = (e) => {
    e.preventDefault();

    const data = {
      Username: username,
      Password: password,
      Email: email,
      Token: token,
      Name: name,
      PhoneNo: phoneNumber,
    };
    const url = "http://localhost:5277/api/User/Login";
    axios
      .post(url, data)
      .then((result) => {
        const dt = result.data;

        if (dt.statusCode === 200) {
          setIsLogin(true);
          localStorage.setItem("islogin", true);
          localStorage.setItem('email',email);
          history("/home");
        } else {
          if (dt.statusCode === 100) alert(dt.statusMessage);
        }
      })

      .catch((error) => {
        console.log(error);
      });
  };

  const goToSignUp = () => {
    history("/signup");
  };
  const goToForgotPassword = () => {
    history("/enter_email");
  };

  return (
    <div className="login">
      <h1 className="text">Login</h1>
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
          <img src={passwordIcon} alt="" />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
      </div>
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <a
          onClick={goToForgotPassword}
          style={{
            color: "#3c009d",
            textDecoration: "none",
            marginTop: "20px",
            cursor: "pointer",
            marginRight: "330px",
            marginBottom: "0px"
          }}
        >
          Forgot Password
        </a>
        <div className="button-container" style={{ marginTop: "0px" }}>
          <button className="button-next-login" onClick={handleLogin}>
            Login
          </button>
          <button className="button-next-login" onClick={goToSignUp}>
            Sign up
          </button>
        </div>
      </div>
    </div>
  );
};

export default Login;
