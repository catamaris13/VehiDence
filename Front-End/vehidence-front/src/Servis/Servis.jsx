import React, { useState, useEffect } from "react";
import axios, { all } from "axios";
import { useNavigate } from "react-router-dom";
import ErrorPage from "../ErrorPage/ErrorPage";

const Servis = () => {
  const [serieSasiu, setSerieSasiu] = useState("");
  const [kmUltim, setKmUltim] = useState("");
  const [kmExpirare, setKmExpirare] = useState("");
  const [serviceName, setServiceName] = useState("");
  const [id, setId] = useState("");
  const navigate = useNavigate();

  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

  const handleSubmit = async (e) => {
    
    /*const servis = {
        serieSasiu: serieSasiu,
        kmUltim: parseInt(kmUltim),
        kmExpirare: parseInt(kmExpirare),
        serviceName: serviceName,
    };*/
    const service = new FormData();
    service.append("SerieSasiu", serieSasiu);
    service.append("KmUltim", kmUltim);
    service.append("KmExpirare", kmExpirare);
    service.append("ServiceName", serviceName);

    try {
      const response = await axios.post(
        "http://localhost:5277/api/RevizieService/AddRevizieService",
        service,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      console.log(response.data);

      if (response.status === 200) {
        navigate("/home");
      } else {
        alert("Service was not added...");
      }
    } catch (error) {
      console.error("Eroare la adăugarea servis:", error);
      alert("Service was not added")
    }


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
                onChange={(e) => setSerieSasiu(e.target.value.toUpperCase())}
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
