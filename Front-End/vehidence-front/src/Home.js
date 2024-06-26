import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import axios from "axios";
import "./home.css";
import HomePage2 from "./HomePage2/HomePage2";

const Home = () => {
  const [masini, setMasini] = useState([]);
  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );
  const [destination, setDestination] = useState("/login");
  const [displayHorizontal, setDisplayHorizontal] = useState(true);
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [phone, setPhone] = useState("");
  const [token, setToken] = useState("");
  const emailUser = localStorage.getItem("email");

  const toggleDropdown = () => {
    setDropdownOpen(!dropdownOpen);
  };

  useEffect(() => {
    axios
      .get("http://localhost:5277/api/User/All/Users")
      .then((response) => {
        const userData = response.data;
        if (userData.length > 0) {
          const user = userData.find((user) => user.email === emailUser);
          if (user) {
            setName(user.name);
            setUsername(user.username);
            setEmail(user.email);
            setPassword(user.password);
            setPhone(user.phoneNo);
          }
        }
      })
      .catch((error) => {
        console.error("Error fetching user data:", error);
      });
  }, []);

  useEffect(() => {
    if (login) {
      setDestination("/myaccount");
    } else {
      setDestination("/login");
    }
  }, [login]);

  useEffect(() => {
    const handleResize = () => {
      setDisplayHorizontal(window.innerWidth > 768);
    };

    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (username) {
          const response = await axios.get(
            `http://localhost:5277/api/Masina/MasinaList/${username}`
          );
          setMasini(response.data.listMasina);
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [username]);


  const mapMasiniToCards = () => {

    return masini.map((masina, index) => {
      const imageSrc = masina.imageData
        ? "data:image/jpeg;base64," + masina.imageData
        : "";

      return (
        <Link to={"/car/" + masina.id} key={index} className="card-link">
          <div className="card">
            <img
              src={imageSrc}
              className="card-img-top"
              alt={masina.marca}
              onError={(e) => {
                e.target.onerror = null;
                e.target.src = "placeholder.jpg";
              }}
            />
            <div className="card-body">
              <h5 className="card-title">
                {masina.marca} {masina.model}
              </h5>
              <p className="card-text">
                Registration number: {masina.nrInmatriculare}
              </p>
              <p className="card-text">
                Car chassis number: {masina.serieSasiu}
              </p>
            </div>
          </div>
        </Link>
      );
    });
  };

  if (!login) {
    return (
      <div className="home">
        <nav className="navbar">
          <h1>Vehi Dence</h1>
          <div className="links">
            <Link
              to={destination}
              style={{
                color: "white",
                backgroundColor: "#3c009d",
                borderRadius: "10px",
                padding: "8px 20px",
                marginLeft: "20px",
              }}
            >
              My account
            </Link>
            <Link
              to="/login"
              style={{
                color: "white",
                backgroundColor: "#3c009d",
                borderRadius: "10px",
                padding: "8px 20px",
                marginLeft: "40px",
              }}
            >
              Login
            </Link>
            <Link
              to="/signup"
              style={{
                color: "white",
                backgroundColor: "#3c009d",
                borderRadius: "10px",
                padding: "8px 20px",
                marginLeft: "40px",
                marginRight: "40px",
              }}
            >
              Sign Up
            </Link>
          </div>
        </nav>
        <HomePage2/>
      </div>
    );
  } else {
    if (!masini || masini.length === 0) {
      return (
        <div className="home">
          <nav className="navbar">
            <h1>Vehi Dence</h1>
            <div className="links">
              <Link
                to={destination}
                style={{
                  color: "white",
                  backgroundColor: "#3c009d",
                  borderRadius: "10px",
                  padding: "8px 20px",
                  marginLeft: "20px",
                  marginRight: "40px",
                }}
              >
                My account
              </Link>
            </div>
          </nav>
          <h1 className="text">No cars...</h1>
          <div className="dropdown">
            <button className="dropdown-btn">
              <span>New +</span>
              <span className="arrow"></span>
            </button>
            <ul className="dropdown-content">
              <li style={{ "--delay": 1 }}>
                <a href="/new_car">Add Car</a>
              </li>
              
            </ul>
          </div>
          </div>
        
      );
    }
    return(
      <div className="home">
          <nav className="navbar">
            <h1>Vehi Dence</h1>
            <div className="links">
              <Link
                to={destination}
                style={{
                  color: "white",
                  backgroundColor: "#3c009d",
                  borderRadius: "10px",
                  padding: "8px 20px",
                  marginLeft: "20px",
                  marginRight: "40px",
                }}
              >
                My account
              </Link>
            </div>
          </nav>

          <div className="dropdown">
            <button className="dropdown-btn">
              <span>New +</span>
              <span className="arrow"></span>
            </button>
            <ul className="dropdown-content">
              <li style={{ "--delay": 1 }}>
                <a href="/new_car">Add Car</a>
              </li>
            </ul>
          </div>

          <div
            className={`card-container ${displayHorizontal ? "" : "vertical"}`}
          >
            {mapMasiniToCards()}
          </div>
        </div>
    );
  }
};

export default Home;
