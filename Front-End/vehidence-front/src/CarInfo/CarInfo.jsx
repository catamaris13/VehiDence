import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import ErrorPage from "../ErrorPage/ErrorPage";
import "./carInfo.css";

const CarInfo = () => {
  const { id } = useParams();
  const [car, setCar] = useState(null);
  const [casco, setCasco] = useState([]);
  const [asigurare, setAsigurare] = useState([]);
  const [itp, setItp] = useState(null);
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );

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
          setCasco(cascoData);
        })
        .catch((error) => {
          console.error("Error fetching Casco data", error);
        });
    }
  }, [nrInmatriculare]);

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
  }, [nrInmatriculare]);

  useEffect(() => {
    if (nrInmatriculare) {
      axios
        .get(`http://localhost:5277/api/ITP/ITPList/${nrInmatriculare}`)
        .then((response) => {
          const itpData = response.data.listITP;
          setItp(itpData);
        })
        .catch((error) => {
          console.error("Error fetching ITP data", error);
        });
    }
  }, [nrInmatriculare]);

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
            ? `data:image/jpeg;base64,${car.imageData}`
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
                  className={`drop-down-item ${openIndex === `casco-${index}` ? "active" : ""}`}
                  onClick={() => toggleAccordion(`casco-${index}`)}
                >
                  <div className="drop-down-header" style={{ "--delay": 1 }}>
                    <h3>Casco</h3>
                  </div>
                  <div className="drop-down-body">
                    {casco && casco.length > 0 ? (
                      <div>
                        <p>Insurer name: {casco[0].asigurator}</p>
                        <p>Create date: {casco[0].dataCreare}</p>
                        <p>End date: {casco[0].dataExpirare}</p>
                        {casco[0].imageData ? (
                          <img
                            className="img-drop-down"
                            src={`data:image/jpeg;base64,${casco[0].imageData}`}
                            alt="Casco Document"
                            onError={(e) => {
                              e.target.onerror = null;
                              e.target.src = "placeholder.jpg";
                            }}
                          />
                        ) : (
                          <p>No image available</p>
                        )}
                      </div>
                    ) : (
                      <p>Not found</p>
                    )}
                  </div>
                </div>

                <div
                  className={`drop-down-item ${openIndex === `asigurare-${index}` ? "active" : ""}`}
                  onClick={() => toggleAccordion(`asigurare-${index}`)}
                >
                  <div className="drop-down-header" style={{ "--delay": 1 }}>
                    <h3>Asigurare</h3>
                  </div>
                  <div className="drop-down-body">
                    {asigurare && asigurare.length > 0 ? (
                      <div>
                        <p>Insurer name: {asigurare[0].asigurator}</p>
                        <p>Create date: {asigurare[0].dataCreare}</p>
                        <p>End date: {asigurare[0].dataExpirare}</p>
                        {asigurare[0].imageData ? (
                          <img
                            className="img-drop-down"
                            src={`data:image/jpeg;base64,${asigurare[0].imageData}`}
                            alt="Asigurare Document"
                            onError={(e) => {
                              e.target.onerror = null;
                              e.target.src = "placeholder.jpg";
                            }}
                          />
                        ) : (
                          <p>No image available</p>
                        )}
                      </div>
                    ) : (
                      <p>Not found</p>
                    )}
                  </div>
                </div>

                <div
                  className={`drop-down-item ${openIndex === `itp-${index}` ? "active" : ""}`}
                  onClick={() => toggleAccordion(`itp-${index}`)}
                >
                  <div className="drop-down-header" style={{ "--delay": 1 }}>
                    <h3>ITP</h3>
                  </div>
                  <div className="drop-down-body">
                    {itp && itp.length > 0 ? (
                      <div>
                        <p>Create date: {itp[0].dataCreare}</p>
                        <p>End date: {itp[0].dataExpirare}</p>
                      </div>
                    ) : (
                      <p>Not found</p>
                    )}
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
