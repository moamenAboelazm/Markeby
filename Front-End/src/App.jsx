import React from "react";
import { Routes, Route } from "react-router-dom";
import Register from "./Pages/Auth/Register";
import Home from "./Pages/Home";
import Login from "./Pages/Auth/Login";
import GoBack from "./Pages/Auth/GoBack";
import Dashboard from "./Pages/Dashboard/Dashboard";
import AddCaptain from "./Pages/Dashboard/Captains/AddCaptain";
import "./App.css";
import Captains from "./Pages/Dashboard/Captains/Captains";
import ProtectedRoutes from "./Pages/Auth/ProtectedRoutes";
import UpdateCaptain from "./Pages/Dashboard/Captains/UpdateCaptain";
import AddShip from "./Pages/Dashboard/Ships/AddShip";
import Vessels from "./Pages/Dashboard/Ships/Vessels";
import UpdateVessel from "./Pages/Dashboard/Ships/UpdateVessel";
import AddTrip from "./Pages/Dashboard/Trips/AddTrip";
import Trips from "./Pages/Dashboard/Trips/Trips";
import Protoflio from "./Pages/Dashboard/Trips/Protoflio";
import UpdateTrip from "./Pages/Dashboard/Trips/UpdateTrip";
import Users from "./Pages/Dashboard/Users/Users";
const App = () => {
  return (
    <div className="bg-background min-h-screen">
      <Routes>
        <Route path="/" element={<Home />} />
        {/* <Route element={<GoBack />}> */}
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        {/* </Route> */}
        <Route element={<ProtectedRoutes />}>
          <Route path="/dashboard" element={<Dashboard />}>
          <Route path="users" element={<Users />}/>
            <Route path="captains" element={<Captains />} />
            <Route path="captains/:id" element={<UpdateCaptain />} />
            <Route path="addcaptain" element={<AddCaptain />} />
            <Route path="vessels" element={<Vessels />} />
            <Route path="addvessel" element={<AddShip />} />
            <Route path="vessels/:id" element={<UpdateVessel />} />
            <Route path="trips" element={<Trips />} />
            <Route path="addtrip" element={<AddTrip />} />
            <Route path="trips/:id" element={<UpdateTrip />} />
            <Route path="trips/protoflio/:id" element={<Protoflio />} />
          </Route>
        </Route>
      </Routes>
    </div>
  );
};

export default App;
