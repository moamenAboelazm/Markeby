import React from "react";
import { Routes, Route } from "react-router-dom";
import Register from "./Pages/Auth/Register";
import Home from "./Pages/Home";
import Login from "./Pages/Auth/Login";
import GoBack from "./Pages/Auth/GoBack";
import Dashboard from "./Pages/Dashboard/Dashboard";

const App = () => {
  return (
    <div className="bg-background min-h-screen">
      <Routes>
        <Route path="/" element={<Home />} />
        {/* <Route element={<GoBack />}> */}
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        {/* </Route> */}
        <Route path="/dashboard" element={<Dashboard />}></Route>
      </Routes>
    </div>
  );
};

export default App;
