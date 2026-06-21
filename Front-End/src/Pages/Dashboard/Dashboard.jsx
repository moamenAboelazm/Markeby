import React from "react";
import SideBar from "../../Components/Dashboard/SideBar";
import Topbar from "../../Components/Dashboard/Topbar";
import { Outlet } from "react-router-dom";

const Dashboard = () => {
  return (
    <div className="w-full h-screen">
      <Topbar />
      <div className="flex">
        <SideBar />
        <div className="flex-1 p-4 mt-[80px] ml-[270px]">
          <Outlet />
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
