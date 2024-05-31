import "../carInfo.css";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

const ITPDropDown = () => {
  const { id } = useParams();
  const [itp, setItp] = useState([]);
  const [car, setCar] = useState(null);
  const [nrInmatriculare, setNrInmatriculare] = useState("");
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
        .get(`http://localhost:5277/api/ITP/ITPList/${nrInmatriculare}`)
        .then((response) => {
          const itpData = response.data.listITP
          setItp(itpData);
        })
        .catch((error) => {
          console.error("Error fetching Casco data", error);
        });
    }
  }, [nrInmatriculare]);



  const formatSimpleDate = (dateString) => {
    const date = new Date(dateString);
    const formattedDate = `${date.getDate()}-${date.getMonth() + 1}-${date.getFullYear()}`;
    return formattedDate;
  };

  return (
    <div className="drop-down">
      <div
        className={`drop-down-item ${openIndex === "itp" ? "active" : ""}`}
        onClick={() => toggleAccordion("itp")}
      >
        <div className="drop-down-header" style={{ "--delay": 1 }}>
          <h3>ITP</h3>
        </div>
        {itp.length > 0 &&
          itp.map((itpItem, index) => (
            <div className="drop-down-body" key={index}>
              <div className="content-container">
                <div className="text-container">
                  <p>Create date: {formatSimpleDate(itpItem.dataCreare)}</p>
                  <p>End date: {formatSimpleDate(itpItem.dataExpirare)}</p>
                </div>
              </div>
              {itpItem.isValid === 1 && (
                <p className="is-valid">Valid</p>
              )}
              {itpItem.isValid === 0 && (
                <p className="is-not-valid">Not Valid</p>
              )}
            </div>
          ))}
      </div>
      
    </div>
  );
};

export default ITPDropDown;
