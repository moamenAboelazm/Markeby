import React from "react";
import SideBar from "../../Components/Dashboard/SideBar";
import Topbar from "../../Components/Dashboard/Topbar";

const Dashboard = () => {
  return (
    <div className="w-full h-screen">
      <Topbar />
      <SideBar />
    </div>
  );
};

export default Dashboard;
