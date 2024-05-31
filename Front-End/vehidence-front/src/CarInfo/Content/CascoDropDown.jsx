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
          const cascoData = response.data.listCasco;
          console.log(cascoData);
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

  return (
    <div className="drop-down">
      {casco.length > 0 &&
        casco.map((cascoItem, index) => (
          <div className="drop-down-item" key={index}>
            <div
              className={`drop-down-item ${
                openIndex === `casco-${index}` ? "active" : ""
              }`}
              onClick={() => toggleAccordion(`casco-${index}`)}
            >
              <div className="drop-down-header" style={{ "--delay": 1 }}>
                <h3>CASCO</h3>
              </div>
              <div className="drop-down-body">
                <div className="content-container">
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
                  <div className="text-container">
                    <p>Insurer name: {cascoItem.asigurator}</p>
                    <p>Create date: {cascoItem.dataCreare}</p>
                    <p>End date: {cascoItem.dataExpirare}</p>
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

export default CascoDropDown;
