import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./AddMasina.css";

const AddMasina = () => {
  const [serieSasiu, setSerieSasiu] = useState("");
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [marca, setMarca] = useState("");
  const [model, setModel] = useState("");
  const [imageFile, setImageFile] = useState(null);
  //const username = localStorage.getItem('username');
  const [id, setId] = useState("");
  const [imageData, setImageData] = useState("");
  const navigate = useNavigate();
  const email = localStorage.getItem("email");
  const [username, setUsername] = useState("");

  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

  const handleImageChange = (e) => {
    const image = e.target.files[0];
    setImageFile(image);
  };

  useEffect(() => {
    axios
      .get("http://localhost:5277/api/User/All/Users")
      .then((response) => {
        const userData = response.data;
        if (userData.length > 0) {
          const user = userData.find((user) => user.email === email);
          if (user) {
            setUsername(user.username);
            console.log(username);
          }
        }
      })
      .catch((error) => {
        console.error("Error fetching user data:", error);
      });
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const masina = new FormData();
    masina.append("SerieSasiu", serieSasiu);
    masina.append("NrInmatriculare", nrInmatriculare);
    masina.append("Marca", marca);
    masina.append("Model", model);
    masina.append("Username", username);
    if (imageFile) {
      masina.append("imageFile", imageFile);
    } else {
      masina.append("imageFile", null);
    }

    try {
      const response = await axios.post(
        "http://localhost:5277/api/Masina/AddMasina",
        masina,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      console.log(response.data);

      if (response.data.statusCode === 200) {
        navigate("/home");
      } else {
        alert("The car was not added...");
      }
    } catch (error) {
      console.error("Eroare la adăugarea mașinii:", error);
    }
  };
  if (login) {
    return (
      <div className="content-add-masina">
        <h1 className="text">New Car</h1>

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
                placeholder="Car brand"
                value={marca}
                onChange={(e) => setMarca(e.target.value)}
              />
            </div>

            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Car model"
                value={model}
                onChange={(e) => setModel(e.target.value)}
              />
            </div>
          </div>

          <div className="inputs-fara-poza">
            <div className="input-fara-poza">
              <input
                type="text"
                placeholder="Registration number"
                value={nrInmatriculare}
                onChange={(e) => setNrInmatriculare(e.target.value)}
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
            Add car
          </button>
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

export default AddMasina;
