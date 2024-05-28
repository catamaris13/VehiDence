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
          <button className="button-new-casco">Add Itp</button>
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
