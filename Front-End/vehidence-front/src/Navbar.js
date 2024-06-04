const Navbar = () => {
  return (
    <nav className="navbar">
      <h1>Vehi Dence</h1>
      <div className="links">
        <a href="/login" style={{ 
          color: 'white', 
          backgroundColor: '#3c009d',
          borderRadius: '10px', 
          padding: '8px 20px'
        }}>My account</a>
      </div>
    </nav>
  )
}
 
export default Navbar;