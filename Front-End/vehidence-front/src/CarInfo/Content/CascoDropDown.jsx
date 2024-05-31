import "../carInfo.css";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

const CascoDropDown = () => {
  const { id } = useParams();
  const [casco, setCasco] = useState([]);
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
        .get(`http://localhost:5277/api/Casco/CascoList/${nrInmatriculare}`)
        .then((response) => {
          const cascoData = response.data.listCasco
          setCasco(cascoData);
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
    const formattedDate = `${date.getDate()}-${date.getMonth() + 1}-${date.getFullYear()}`;
    return formattedDate;
  };

  return (
    <div className="drop-down">
      <div
        className={`drop-down-item ${openIndex === "casco" ? "active" : ""}`}
        onClick={() => toggleAccordion("casco")}
      >
        <div className="drop-down-header" style={{ "--delay": 1 }}>
          <h3>CASCO</h3>
        </div>
        {casco.length > 0 &&
          casco.map((cascoItem, index) => (
            <div className="drop-down-body" key={index}>
              <div className="content-container">
                {cascoItem.imageData && (
                  <img
                    className="img-drop-down"
                    src={`data:image/jpeg;base64,${cascoItem.imageData}`}
                    alt="Casco Document"
                    onClick={() => handleImageClick(cascoItem.imageData)}
                    onError={(e) => {
                      e.target.onerror = null;
                      e.target.src = "placeholder.jpg";
                    }}
                  />
                )}
                <div className="text-container">
                  <p>Insurer name: {cascoItem.asigurator}</p>
                  <p>Create date: {formatSimpleDate(cascoItem.dataCreare)}</p>
                  <p>End date: {formatSimpleDate(cascoItem.dataExpirare)}</p>
                </div>
              </div>
              {cascoItem.isValid === 1 && (
                <p className="is-valid">Valid</p>
              )}
              {cascoItem.isValid === 0 && (
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
};

export default CascoDropDown;
