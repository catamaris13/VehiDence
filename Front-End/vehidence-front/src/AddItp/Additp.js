import React from "react";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./addItp.css"

const AddItp = () => {
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [dataCreare, setDataCreare] = useState("");
  const [dataExpirare, setDataExpirare] = useState("");
  const navigate = useNavigate();

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

  const handleSubmit = async (e) => {
    e.preventDefault();

    const currentDate = new Date();
    if(dataCreare > currentDate){
      alert("Create Date must be in the past or in present")
      return;
    }
    if(dataExpirare < currentDate){
      alert("Expire Date must be in the future")
      return;
    }

    const formatDate = (date) => date.toISOString();

    const itp = {
      NrInmatriculare: nrInmatriculare,
      DataCreare: formatDate(dataCreare),
      DataExpirare: formatDate(dataExpirare),
    };

    console.log(formatDate(dataCreare))
    
    
      const url = "http://localhost:5277/api/ITP/AddItp"
      axios.post(url,itp)
      .then((response) => {
        if(response.data.statusCode == 200){
          navigate("/home");
        }
        else{
         alert("ITP was not added")
        }
      })
    .catch((error) => {
      console.log("Adding Itp failed ",error);
      alert("Itp was not added")
    });

  };

  if (login) {
    return (
      <div className="content-add-casco">
        <h1 className="text">New Itp</h1>

        <div className="input-row">
          <div className="inputs-fara-poza">
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Registration number"
                value={nrInmatriculare}
                onChange={(e) => setNrInmatriculare(e.target.value)}
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
          <button className="button-new-casco" onClick={handleSubmit}>Add Itp</button>
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

export default AddItp;
