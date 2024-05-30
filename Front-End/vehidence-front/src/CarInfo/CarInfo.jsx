import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import ErrorPage from "../ErrorPage/ErrorPage";
import "./cardInfo.css";

const CarInfo = () => {
  const { id } = useParams();
  const [car, setCar] = useState(null);
  const [casco, setCasco] = useState(null);
  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

  useEffect(() => {
    axios
      .get("http://localhost:5277/api/Masina/MasinaList/" + id)
      .then((response) => {
        setCar(response.data.listMasina);
        console.log(response.data.listMasina);
      })
      .catch((error) => {
        console.error("Error fetching car data:", error);
      });
  }, [id]);

  useEffect(() => {
    if (car != null) { 
      axios
        .get("http://localhost:5277/api/Casco/CascoList/" + car.nrInmatriculare)
        .then((response) => {
          setCasco(response.data.listCasco);
          console.log(response.data.listCasco);
          console.log(car.nrInmatriculare);
          
        })
        .catch((error) => {
          console.error("Eroare la casco", error);
        });
    }
  }, [car]);

  const [openIndex, setOpenIndex] = useState(null);

  const toggleAccordion = (index) => {
    setOpenIndex((prevIndex) => (prevIndex === index ? null : index));
  };

  if (!car) return <div className="text">Loading...</div>;
  if (login) {
    return (
      <div>
        {car.map((car, index) => {
          const imageSrc = car.imageData
            ? "data:image/jpeg;base64," + car.imageData
            : "placeholder.jpg";

          return (
            <div className="container-info" key={index}>
              <div className="card-info">
                <img
                  src={imageSrc}
                  alt={car.marca}
                  onError={(e) => {
                    e.target.onerror = null;
                    e.target.src = "placeholder.jpg";
                  }}
                />
                <div className="info">
                  <h5>
                    {car.marca} {car.model}
                  </h5>
                  <p>Registration number: {car.nrInmatriculare}</p>
                  <p>Car chassis number: {car.serieSasiu}</p>
                </div>
              </div>
              <div className="drop-down">
                <div
                  className={`drop-down-item ${
                    openIndex === index ? "active" : ""
                  }`}
                  onClick={() => toggleAccordion(index)}
                >
                  <div className="drop-down-header" style={{ "--delay": 1 }}>
                    Accordion Item #{index + 1}
                  </div>
                  <div className="drop-down-body">
                  {casco && <h5>{casco[0].asigurator}</h5>}
                  </div>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    );
  } else {
    return <ErrorPage />;
  }
};

export default CarInfo;
