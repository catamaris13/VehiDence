import React, { useState } from "react";
import axios from "axios"; 
import emailIcon from "../assets/email_img.png";
import "../index.css";

const EnterEmailPage = () => {
  const [email, setEmail] = useState("");
  const [showPopup, setShowPopup] = useState(false); 
  const [errorEmail, setErrorEmail] = useState(false);
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [token, setToken] = useState("");

  const handleSubmit = () => {
    const data = {
      Username: username,
      Password: password,
      Email: email,
      Token: token,
      Name: name,
      PhoneNo: phoneNumber,
    };
    axios
      .post("http://localhost:5277/api/User/SendEmailPassword", data)
      .then((response) => {
        console.log(response.data);
        if (response.status == 200) {
          console.log("Email was successfully sent");
          setShowPopup(true); 
          setEmail("");
        } else {
          console.log("Failed to send email");
        }
      })
      .catch((error) => {
        console.log(error);
        setErrorEmail(true);
      });
  };

  return (
    <div className="content-center-column">
      <h1 className="text" style={{ fontSize: "40px", marginTop: "80px" }}>
        Enter your email address
      </h1>
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
        <button className="button smaller-button" onClick={handleSubmit}>
          Submit
        </button>
      </div>

      {showPopup && (
        <div className="popup-container">
          <div className="popup-content">
            <p>Email was successfully sent</p>
            <button
              onClick={() => setShowPopup(false)}
              style={{ marginRight: "30px" }}
            >
              Close
            </button>
          </div>
        </div>
      )}

      {errorEmail && (
        <div className="popup-container">
          <div className="popup-content">
            <p>Email not found...</p>
            <button
              onClick={() => setErrorEmail(false)}
              style={{ marginRight: "30px" }}
            >
              Close
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default EnterEmailPage;
