import React, { useState } from "react";
import axios from 'axios';

import{ useNavigate} from "react-router-dom";


function Login()
{
const history=useNavigate();
  const [Username,setUsername]=useState('')
  const [Password,setPassword]=useState('')
  const [Name,setName]=useState('')
  const [PhoneNo,setPhoneNo]=useState('')
  const [Email,setEmail]=useState('')
  const [Token,setToken]=useState('')
  
     const Registration = (e)=>{
      history("registration");
     }

    const UserLogin = (e)=>{
        
        e.preventDefault();
        console.log(Username);
        
        const data ={
          Username:Username,
          Password:Password,
          Email:Email,
          Token:Token,
          Name:Name,
          PhoneNo:PhoneNo
          
         
        }
        const url='https://localhost:5277/api/User/Login';
        console.log(data)
        axios.post(url,data)
        .then((result)=>{
            const dt=result.data;
            
            if(dt.statusCode===200)
            {
              
                localStorage.setItem("username",Username);
                history("/tasks");
 
            }
            else 
            {
                if(dt.statusCode===100)
                alert(dt.statusMessage);
            }
            
          })
          
        .catch((error)=>{
            console.log(error);
        })
    }
    const clear =()=>{
      setEmail('');
      setPassword('');
      
      
      
    }
    
    return (
        <section class="vh-100 gradient-custom">
  <div class="container py-5 h-100">
    <div class="row justify-content-center align-items-center h-100">
      <div class="col-12 col-lg-9 col-xl-7">
        <div class="card shadow-2-strong card-registration" style={{'border-radius': '15px'}}>
          <div class="card-body p-4 p-md-5">
            <h3 class="mb-4 pb-2 pb-md-0 mb-md-5">Login Form</h3>
            <form>

              <div class="row">
                <div class="col-md mb-4">

                  <div class="form-outline">
                    <input type="text"
                     id="firstName"
                      class="form-control form-control-lg"
                      onChange={(e)=>setEmail(e.target.value)} 
                      value={Email}
                      />
                    <label class="form-label" for="firstName">Email</label>
                  </div>

                </div>
                
               
              </div>

              

              <div class="row">
               
                
                <div class="col-md mb-4 pb-2">

                  <div class="form-outline">
                    <input type="Password" 
                    id="Password" 
                    class="form-control form-control-lg" 
                    onChange={(e)=>setPassword(e.target.value)}
                    value={Password}
                    />
                    <label class="form-label" for="Password">Password</label>
                  </div>

                </div>
              </div>

              

              <div class="d-flex justify-content-end pt-3">
                <button type="button" className="btn btn-light btn-lg"
                 onClick={(e)=> clear()}>
                    Reset all
                </button>
                <button
                 type="button" 
                 className="btn btn-warning btn_lg ms-2"
                 onClick={(e)=> UserLogin(e)}
                 >
                    Submit form
                </button>
                <button
                 type="button" 
                 className="btn btn-warning btn_lg ms-2"
                 onClick={(e)=> Registration(e)}
                 >
                    Registration
                </button>
                
                
              </div>

            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>
    )
}

export default Login;