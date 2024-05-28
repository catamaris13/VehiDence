import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

const CarInfo = () => {
  const { id } = useParams();
  const [car, setCar] = useState(null);

  useEffect(() => {
    axios
      .get("http://localhost:5277/api/Masina/" + id)
      .then((response) => {
        setCar(response.data);
      })
      .catch((error) => {
        console.error("Error fetching car data:", error);
      });
  }, [id]);

  if (!car) return <div className="text">Loading...</div>;

  return (
    <div>
      <h1>{car.marca} {car.model}</h1>
      <img
        src={`data:image/jpeg;base64,${car.imageData}`}
        alt={car.marca}
        onError={(e) => {
          e.target.onerror = null;
          e.target.src = "placeholder.jpg";
        }}
      />
      <p>Nr. Inmatriculare: {car.nrInmatriculare}</p>
      <p>Serie Sasiu: {car.serieSasiu}</p>
      {/* Add more car details as needed */}
    </div>
  );
};

export default CarInfo;
