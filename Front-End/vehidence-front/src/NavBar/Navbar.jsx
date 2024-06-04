import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
const Navbar = () => {
  const [login, setLogin] = useState(
    localStorage.getItem("islogin")
      ? JSON.parse(localStorage.getItem("islogin"))
      : false
  );
  const [destination, setDestination] = useState("/login");
  useEffect(() => {
    if (login) {
      setDestination("/myaccount");
    } else {
      setDestination("/login");
    }
  }, [login]);

  if (login!=null) {
    return (
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
    );
  }
  else{
    return (
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
    );
    
  }
};

export default Navbar;
