import "../carInfo.css";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

const VinietaDropDown = () => {
  const { id } = useParams();
  const [vinieta, setVinieta] = useState([]);
  const [car, setCar] = useState(null);
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [selectedImage, setSelectedImage] = useState(null);
  const [openIndex, setOpenIndex] = useState(null);

  const toggleAccordion = (index) => {
    setOpenIndex((prevIndex) => (prevIndex === index ? null : index));
  };

  useEffect(() => {
    axios
      .get(`http://localhost:5277/api/Masina/MasinaList/${id}`)
      .then((response) => {
        const carData = response.data.listMasina;

        setCar(carData);
        if (carData.length > 0) {
          setNrInmatriculare(carData[0].nrInmatriculare);
        }
      })
      .catch((error) => {
        console.error("Error fetching car data:", error);
      });
  }, [id]);

  useEffect(() => {
    if (nrInmatriculare) {
      axios
        .get(`http://localhost:5277/api/Vigneta/VignetaList/${nrInmatriculare}`)
        .then((response) => {
          const vinietaData = response.data.listVigneta;
          setVinieta(vinietaData);
        })
        .catch((error) => {
          console.error("Error fetching Casco data", error);
        });
    }
  }, [nrInmatriculare]);

  const handleImageClick = (imageData) => {
    setSelectedImage(imageData);
  };

  const formatSimpleDate = (dateString) => {
    const date = new Date(dateString);
    const formattedDate = `${date.getDate()}-${
      date.getMonth() + 1
    }-${date.getFullYear()}`;
    return formattedDate;
  };

  if (vinieta) {
    return (
      <div className="drop-down">
        <div
          className={`drop-down-item ${
            openIndex === "vinieta" ? "active" : ""
          }`}
          onClick={() => toggleAccordion("vinieta")}
        >
          <div className="drop-down-header" style={{ "--delay": 1 }}>
            <h3>Vignette</h3>
          </div>
          {vinieta.length > 0 &&
            vinieta.map((vinietaItem, index) => (
              <div className="drop-down-body" key={index}>
                <div className="content-container">
                  {vinietaItem.imageData && (
                    <img
                      className="img-drop-down"
                      src={`data:image/jpeg;base64,${vinietaItem.imageData}`}
                      alt="Vinieta Document"
                      onClick={() => handleImageClick(vinietaItem.imageData)}
                      onError={(e) => {
                        e.target.onerror = null;
                        e.target.src = "placeholder.jpg";
                      }}
                    />
                  )}
                  <div className="text-container">
                    <p>Country: {vinietaItem.tara}</p>
                    <p>
                      Create date: {formatSimpleDate(vinietaItem.dataCreare)}
                    </p>
                    <p>
                      End date: {formatSimpleDate(vinietaItem.dataExpirare)}
                    </p>
                  </div>
                </div>
                {vinietaItem.isValid === 1 && <p className="is-valid">Valid</p>}
                {vinietaItem.isValid === 0 && (
                  <p className="is-not-valid">Not Valid</p>
                )}
              </div>
            ))}
        </div>
        {selectedImage && (
          <div
            className="image-container-mare"
            onClick={() => setSelectedImage(null)}
          >
            <div className="image-mare">
              <button
                className="exit-button"
                onClick={() => setSelectedImage(null)}
              >
                X
              </button>
              <img
                src={`data:image/jpeg;base64,${selectedImage}`}
                alt="Selected Image"
              />
            </div>
          </div>
        )}
      </div>
    );
  }
  else{
    return (
      <div className="drop-down">
        <div
          className={`drop-down-item ${
            openIndex === "vinieta" ? "active" : ""
          }`}
          onClick={() => toggleAccordion("vinieta")}
        >
          <div className="drop-down-header" style={{ "--delay": 1 }}>
            <h3>Vignette</h3>
          </div>
          <div className="drop-down-body">
            <h3 style={{color: "white", display: "flex", justifyContent:"center", alignItems:"center"}}>Don't have</h3>
          </div>
        </div>
      </div>
    );
  }
};

export default VinietaDropDown;
