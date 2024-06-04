import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import ErrorPage from "../ErrorPage/ErrorPage";
import "./carInfo.css";
import CascoDropDown from "./Content/CascoDropDown";
import AsigurareDropDown from "./Content/AsigurareDropDown";
import ITPDropDown from "./Content/ITPDropDown";
import ServiceDropDown from "./Content/ServiceDropDown";
import VinietaDropDown from "./Content/VinietaDropDown";
import Navbar from "../NavBar/Navbar";
import { useNavigate } from "react-router-dom";

const CarInfo = () => {
  const { id } = useParams();
  const [car, setCar] = useState(null);
  const [casco, setCasco] = useState([]);
  const [asigurare, setAsigurare] = useState([]);
  const [itp, setItp] = useState(null);
  const [nrInmatriculare, setNrInmatriculare] = useState("");
  const history = useNavigate();
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

  const [openIndex, setOpenIndex] = useState(null);

  const toggleAccordion = (index) => {
    setOpenIndex((prevIndex) => (prevIndex === index ? null : index));
  };
  const handleSubmit = () =>{
    history("/home");
  }

  if (!car) return <div className="text">Loading...</div>;
  if (login) {
    return (
      <div>
        {car.map((car, index) => {
          const imageSrc = car.imageData
            ? `data:image/jpeg;base64,${car.imageData}`
            : "placeholder.jpg";

          return (
            <div className="ceva">
              <Navbar />
              <div className="button-container-add-casco">
                <button className="button-new-casco" onClick={handleSubmit} form="arrow">
                  Return
                </button>
              </div>
              <div className="container-info" key={index}>
                <div className="dropdown">
                  <button className="dropdown-btn">
                    <span>New +</span>
                    <span className="arrow"></span>
                  </button>
                  <ul className="dropdown-content">
                    <li style={{ "--delay": 2 }}>
                      <a href="/new_casco">Casco</a>
                    </li>
                    <li style={{ "--delay": 3 }}>
                      <a href="/new_itp">ITP</a>
                    </li>
                    <li style={{ "--delay": 4 }}>
                      <a href="new_insurance">Insurance</a>
                    </li>
                    <li style={{ "--delay": 4 }}>
                      <a href="new_driver_license">Driver license</a>
                    </li>
                    <li style={{ "--delay": 4 }}>
                      <a href="new_vignette">Vignette</a>
                    </li>
                    <li style={{ "--delay": 4 }}>
                      <a href="new_service">Car Service</a>
                    </li>
                  </ul>
                </div>
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

                <CascoDropDown />
                <AsigurareDropDown />
                <ITPDropDown />
                <ServiceDropDown />
                <VinietaDropDown />
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
