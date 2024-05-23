import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./addCasco.css";

const AddCasco = () => {
  const [serieSasiu, setSerieSasiu] = useState("");
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [dataCreare, setDataCreare] = useState(new Date());
  const [dataExpirare, setDataExpirare] = useState(new Date());
  const [asigurator, setAsigurator] = useState("");
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

  return (
    <div className="content-add-casco">
      <h1 className="text">New Casco</h1>

      <div className="input-row">
        <div className="inputs-fara-poza">
          <div className="input-fara-poza">
            <input
              type="text"
              placeholder="Insurance"
              value={asigurator}
              onChange={(e) => setAsigurator(e.target.value)}
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
          <DatePicker selected={dataCreare} onChange={handleDataCreareChange} />
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
        <button className="button-new-casco">
          Add Casco
        </button>
      </div>
    </div>
  );
};

export default AddCasco;
