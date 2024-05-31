import React, { useState,useEffect } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./addPermis.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import ErrorPage from "../ErrorPage/ErrorPage";


const AddPermis = () => {
  const [dataCreare, setDataCreare] = useState(new Date());
  const [dataExpirare, setDataExpirare] = useState(new Date());
  const [nume, setNume] = useState("");
  const [username,setUsername] = useState('');
  const [categorie, setCategorie] = useState('');
  const [imageFile,setImageFile] = useState('');
  const emailUser = localStorage.getItem('email');
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

    const permis = new FormData();
    const formatDate = (date) => date.toISOString();

    permis.append("Nume", nume);
    permis.append("DataCreare", formatDate(dataCreare));
    permis.append("DataExpirare", formatDate(dataExpirare));
    permis.append("username", username);
    permis.append("Categorie", categorie);
    if (imageFile) {
      permis.append("imageFile", imageFile);
    } else {
      permis.append("imageFile", null);
    }

    try {
      const response = await axios.post(
        "http://localhost:5277/api/PermisConducere/AddPermis",
        permis,
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
        alert("Driver License was not added...");
      }
    } catch (error) {
      console.error("Eroare la adăugarea casco:", error);
      alert("Driver License was not added")
    }

  };


  if (login) {
    return (
      <div className="content-add-casco">
        <h1 className="text">New Driver License</h1>

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
                placeholder="Name"
                value={nume}
                onChange={(e) => setNume(e.target.value)}
              />
            </div>
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Category"
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
          <button className="button-new-casco" onClick={handleSubmit}>Add Driver License</button>
        </div>
      </div>
    );
  } else {
    return (
      <ErrorPage/>
    );
  }
};

export default AddPermis;
