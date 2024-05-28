import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import ErrorPage from "../ErrorPage/ErrorPage";

const CarInfo = () => {
  const { id } = useParams();
  const [car, setCar] = useState(null);
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
      })
      .catch((error) => {
        console.error("Error fetching car data:", error);
      });
  }, [id]);

  if (!car) return <div className="text">Loading...</div>;
  if (login) {
    return car.map((car, index) => {
      const imageSrc = car.imageData
        ? "data:image/jpeg;base64," + car.imageData
        : "";

      return (
        <div>
          <img
            src={imageSrc}
            alt={car.marca}
            onError={(e) => {
              e.target.onerror = null;
              e.target.src = "placeholder.jpg";
            }}
          />
          <div>
            <h5>
              {car.marca} {car.model}
            </h5>
            <p>Registration number: {car.nrInmatriculare}</p>
            <p>Car chassis number: {car.serieSasiu}</p>
          </div>
        </div>
      );
    });
  }
  else {
    return(
      <ErrorPage/>
    );
  }
};

export default CarInfo;
