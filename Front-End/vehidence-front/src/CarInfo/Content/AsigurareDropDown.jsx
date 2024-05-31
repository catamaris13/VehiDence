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
          const asigurareData = response.data.listAsigurare;
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
      {asigurare.length > 0 &&
        asigurare.map((asigurareItem, index) => (
          <div className="drop-down-item" key={index}>
            <div
              className={`drop-down-item ${
                openIndex === `asigurare-${index}` ? "active" : ""
              }`}
              onClick={() => toggleAccordion(`asigurare-${index}`)}
            >
              <div className="drop-down-header" style={{ "--delay": 1 }}>
                <h3>Insurance</h3>
              </div>
              <div className="drop-down-body">
                <div className="content-container">
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
                  <div className="text-container">
                    <p>Insurer name: {asigurareItem.asigurator}</p>
                    <p>Create date: {asigurareItem.dataCreare}</p>
                    <p>End date: {asigurareItem.dataExpirare}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
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
