import "../carInfo.css";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";


const AsigurareDropDown = () => {
  const { id } = useParams();
  const [asigurare, setAsigurare] = useState([]);
  const [car, setCar] = useState(null);
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [selectedImage, setSelectedImage] = useState(null);
  const [openIndex, setOpenIndex] = useState(null);

  const toggleAccordion = (index) => {
    setOpenIndex((prevIndex) => (prevIndex === index ? null : index));
  };

  const formatSimpleDate = (dateString) => {
    const date = new Date(dateString);
    const formattedDate = `${date.getDate()}-${date.getMonth() + 1}-${date.getFullYear()}`;
    return formattedDate;
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
        .get(`http://localhost:5277/api/Asigurare/AsigurareList/${nrInmatriculare}`)
        .then((response) => {
          const asigurareData = response.data.listAsigurare
          setAsigurare(asigurareData);
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            console.error("Asigurare data not found");
            setAsigurare([]); // Handle not found
          } else {
            console.error("Error fetching Asigurare data", error);
          }
        });
    }
  }, [nrInmatriculare,car]);

  const handleImageClick = (imageData) => {
    setSelectedImage(imageData);
  };

  return (
    <div className="drop-down">
      <div
        className={`drop-down-item ${openIndex === "asigurare" ? "active" : ""}`}
        onClick={() => toggleAccordion("asigurare")}
      >
        <div className="drop-down-header" style={{ "--delay": 1 }}>
          <h3>Insurance</h3>
        </div>
        {asigurare.length > 0 &&
          asigurare.map((asigurareItem, index) => (
            <div className="drop-down-body" key={index}>
              <div className="content-container">
                {asigurareItem.imageData && (
                  <img
                    className="img-drop-down"
                    src={`data:image/jpeg;base64,${asigurareItem.imageData}`}
                    alt="Insurance Document"
                    onClick={() => handleImageClick(asigurareItem.imageData)}
                    onError={(e) => {
                      e.target.onerror = null;
                      e.target.src = "placeholder.jpg";
                    }}
                  />
                )}
                <div className="text-container">
                  <p>Insurer name: {asigurareItem.asigurator}</p>
                  <p>Create date: {formatSimpleDate(asigurareItem.dataCreare)}</p>
                  <p>End date: {formatSimpleDate(asigurareItem.dataExpirare)}</p>
                </div>
              </div>
              {asigurareItem.isValid === 1 && (
                <p className="is-valid">Valid</p>
              )}
              {asigurareItem.isValid === 0 && (
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

export default AsigurareDropDown;
