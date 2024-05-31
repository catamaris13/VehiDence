import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import ErrorPage from "../ErrorPage/ErrorPage";
import "./carInfo.css";
import CascoDropDown from "./Content/CascoDropDown";
import AsigurareDropDown from "./Content/AsigurareDropDown";
import ITPDropDown from "./Content/ITPDropDown"
import ServiceDropDown from "./Content/ServiceDropDown";
import VinietaDropDown from "./Content/VinietaDropDown";

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

              <CascoDropDown/>
              <AsigurareDropDown/>
              <ITPDropDown/>
              <ServiceDropDown/>
              <VinietaDropDown/>
              
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
