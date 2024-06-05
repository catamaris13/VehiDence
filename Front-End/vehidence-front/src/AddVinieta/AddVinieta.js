import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./addVinieta.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import ErrorPage from "../ErrorPage/ErrorPage";

const AddVinieta = () => {
  const [tara, setTara] = useState("");
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [dataCreare, setDataCreare] = useState(new Date());
  const [dataExpirare, setDataExpirare] = useState(new Date());
  const [imageFile, setImageFile] = useState(null);
  const navigate = useNavigate();

  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

  const handleImageChange = (e) => {
    const image = e.target.files[0];
    setImageFile(image);
  };

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

    const vigneta = new FormData();
    const formatDate = (date) => date.toISOString();

    vigneta.append("NrInmatriculare", nrInmatriculare);
    vigneta.append("DataCreare", formatDate(dataCreare));
    vigneta.append("DataExpirare", formatDate(dataExpirare));
    vigneta.append("Tara", tara);
    if (imageFile) {
      vigneta.append("imageFile", imageFile);
    } else {
      vigneta.append("imageFile", null);
    }

    try {
      const response = await axios.post(
        "http://localhost:5277/api/Vigneta/AddVigneta",
        vigneta,
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
        alert("Vignette was not added...");
      }
    } catch (error) {
      console.error("Eroare la adăugarea vignette:", error);
    }

  };

  if (login) {
    return (
      <div className="content-add-casco">
        <h1 className="text">New Vignette</h1>

        <div className="image-upload">
          <div className="image-upload-button">
            <input
              type="file"
              onChange={handleImageChange}
              className="file-input"
            />
            <p className="file-name">{imageFile && imageFile.name}</p>
          </div>
        </div>

        <div className="input-row">
          <div className="inputs-fara-poza">
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Country"
                value={tara}
                onChange={(e) => setTara(e.target.value.toUpperCase())}
              />
            </div>
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Registration number"
                value={nrInmatriculare}
                onChange={(e) => setNrInmatriculare(e.target.value.toUpperCase())}
              />
            </div>
          </div>
        </div>
        <div className="datepickers-container">
          <div className="datepicker-container">
            <label>Create Date:</label>
            <DatePicker
              selected={dataCreare}
              onChange={handleDataCreareChange}
            />
          </div>

          <div className="datepicker-container">
            <label>Expiration Date:</label>
            <DatePicker
              selected={dataExpirare}
              onChange={handleDataExpirareChange}
            />
          </div>
        </div>
        <div className="button-container-add-casco">
          <button className="button-new-casco" onClick={handleSubmit}>Add Vignette</button>
        </div>
      </div>
    );
  } else {
    return (
      <ErrorPage/>
    );
  }
};

export default AddVinieta;
