import React, { useState,useEffect } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./addPermis.css";
import axios from "axios";


const AddPermis = () => {
  const [serieSasiu, setSerieSasiu] = useState("");
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [dataCreare, setDataCreare] = useState(new Date());
  const [dataExpirare, setDataExpirare] = useState(new Date());
  const [nume, setNume] = useState("");
  const [username,setUsername] = useState('');
  const [categorie, setCategorie] = useState('');
  const emailUser = localStorage.getItem('email');
  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

  const handleDataCreareChange = (date) => {
    setDataCreare(date);
  };

  const handleDataExpirareChange = (date) => {
    setDataExpirare(date);
  };

  useEffect(() => {
    axios
      .get("http://localhost:5277/api/User/All/Users")
      .then((response) => {
        const userData = response.data;
        if (userData.length > 0) {
          const user = userData.find((user) => user.email === emailUser);
          if (user) {
            setUsername(user.username);
          }
        }
      })
      .catch((error) => {
        console.error("Error fetching user data:", error);
        alert('User not found. Plese try again.')
      });
  }, []);


  if (login) {
    return (
      <div className="content-add-casco">
        <h1 className="text">New Driver License</h1>

        <div className="input-row">
          <div className="inputs-fara-poza">
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Name"
                value={nume}
                onChange={(e) => setNume(e.target.value)}
              />
            </div>
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Car chassis number"
                value={categorie}
                onChange={(e) => setCategorie(e.target.value)}
              />
            </div>
          </div>
        </div>
        <div className="datepickers-container">
          {/* Calendar pentru dataCreare */}
          <div className="datepicker-container">
            <label>Create Date:</label>
            <DatePicker
              selected={dataCreare}
              onChange={handleDataCreareChange}
            />
          </div>

          {/* Calendar pentru dataExpirare */}
          <div className="datepicker-container">
            <label>Expiration Date:</label>
            <DatePicker
              selected={dataExpirare}
              onChange={handleDataExpirareChange}
            />
          </div>
        </div>
        <div className="button-container-add-casco">
          <button className="button-new-casco">Add Driver License</button>
        </div>
      </div>
    );
  } else {
    return (
      <div className="text">
        <h1>Page not found</h1>
        <h2>Error 404</h2>
      </div>
    );
  }
};

export default AddPermis;
