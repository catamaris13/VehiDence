import React, { useState, useEffect } from "react";
import axios, { all } from "axios";
import { useNavigate } from "react-router-dom";
import ErrorPage from "../ErrorPage/ErrorPage";

const Servis = () => {
  const [serieSasiu, setSerieSasiu] = useState("");
  const [kmUltim, setKmUltim] = useState("");
  const [kmExpirare, setKmExpirare] = useState("");
  const [serviceName, setServiceName] = useState("");
  //const username = localStorage.getItem('username');
  const [id, setId] = useState("");
  const navigate = useNavigate();

  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

  const handleSubmit = async (e) => {
    
    const servis = {
        serieSasiu: serieSasiu,
        kmUltim: parseInt(kmUltim),
        kmExpirare: parseInt(kmExpirare),
        serviceName: serviceName,
    };
    axios.post("http://localhost:5277/api/RevizieService/AddRevizieService", servis)
    .then((rezultat) => {
        if(rezultat.data.statusCode == 200){
            navigate("/home");
        }
        else {
            alert("Service was not added")
        }
    })
    .catch((error)=>{
        console.log("Servisul are o eroare ",error);
        alert("Service was not added");
    })

  };
  if (login) {
    return (
      <div className="content-add-masina">
        <h1 className="text">New Car Service</h1>

        <div className="input-row">
          <div className="inputs-fara-poza">
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Service Name"
                value={serviceName}
                onChange={(e) => setServiceName(e.target.value)}
              />
            </div>

            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Last Km at service"
                value={kmUltim}
                onChange={(e) => setKmUltim(e.target.value)}
              />
            </div>
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Expiration Km"
                value={kmExpirare}
                onChange={(e) => setKmExpirare(e.target.value)}
              />
            </div>
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Car chassis number"
                value={serieSasiu}
                onChange={(e) => setSerieSasiu(e.target.value)}
              />
            </div>
          </div>

          
        </div>
        <div className="button-container-add-casco">
          <button className="button-new-casco" onClick={handleSubmit}>
            Add Service
          </button>
        </div>
      </div>
    );
  } else {
    return (
      <ErrorPage/>
    );
  }
};

export default Servis;
