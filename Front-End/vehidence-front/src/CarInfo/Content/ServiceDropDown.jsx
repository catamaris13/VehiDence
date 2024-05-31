import "../carInfo.css";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

const ServiceDropDown = () => {
  const { id } = useParams();
  const [service, setService] = useState([]);
  const [car, setCar] = useState(null);
  const [serieSasiu, setSerieSasiu] = useState("");
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
          setSerieSasiu(carData[0].serieSasiu);
        }
      })
      .catch((error) => {
        console.error("Error fetching car data:", error);
      });
  }, [id]);

  useEffect(() => {
    if (serieSasiu) {
      axios
        .get(`http://localhost:5277/api/RevizieService/RevizieServiceList/${serieSasiu}`)
        .then((response) => {
          const serviceData = response.data.listRevizieService
          setService(serviceData);
        })
        .catch((error) => {
          console.error("Error fetching Casco data", error);
        });
    }
  }, [serieSasiu]);



  return (
    <div className="drop-down">
      <div
        className={`drop-down-item ${openIndex === "service" ? "active" : ""}`}
        onClick={() => toggleAccordion("service")}
      >
        <div className="drop-down-header" style={{ "--delay": 1 }}>
          <h3>Service</h3>
        </div>
        {service.length > 0 &&
          service.map((serviceItem, index) => (
            <div className="drop-down-body" key={index}>
              <div className="content-container">
                <div className="text-container">
                  <p>Service name: {serviceItem.serviceName}</p>
                  <p>Last km at service: {serviceItem.kmUltim}</p>
                  <p>Expire km: {serviceItem.kmExpirare}</p>
                </div>
              </div>
              {serviceItem.isValid === 1 && (
                <p className="is-valid">Valid</p>
              )}
              {serviceItem.isValid === 0 && (
                <p className="is-not-valid">Not Valid</p>
              )}
            </div>
          ))}
      </div>
      
    </div>
  );
};

export default ServiceDropDown;
