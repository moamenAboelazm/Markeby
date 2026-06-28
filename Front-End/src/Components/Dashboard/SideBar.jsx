import React from "react";
import { LuLayoutDashboard } from "react-icons/lu";
import { MdAddLocationAlt, MdOutlineAdminPanelSettings } from "react-icons/md";
import { FaUsers } from "react-icons/fa6";
import { FaRegCompass } from "react-icons/fa";
import { IoSettingsSharp } from "react-icons/io5";
import { GiCaptainHatProfile, GiSpeedBoat } from "react-icons/gi";
import { GiPirateCaptain } from "react-icons/gi";
import { FaShip } from "react-icons/fa";
import { Link, NavLink } from "react-router-dom";
import LogOut from "../../Pages/Auth/LogOut";
const SideBar = () => {
  const menuItems = [
    {
      name: "Dashboard",
      icon: <LuLayoutDashboard className="text-[18px]" />,
    },
    {
      name: "Users",
      icon: <FaUsers className="text-[18px]" />,
    },
    {
      name: "Trips",
      icon: <FaRegCompass className="text-[18px]" />,
    },
    {
      name:"Add Trip",
       icon: <MdAddLocationAlt className="text-[18px]" />,
    },
    {
      name: "Captains",
      icon: <GiCaptainHatProfile className="text-[18px]" />,
    },
    {
      name: "Add Captain",
      icon: <GiPirateCaptain className="text-[18px]" />,
    },
    {
      name: "Vessels",
      icon: <GiSpeedBoat />,
    },
    {
      name: "Add Vessel",
      icon: <FaShip className="text-[18px]" />,
    },
    {
      name: "Settings",
      icon: <IoSettingsSharp className="text-[18px]" />,
    },
  ];
  return (
    <div className="side-bar w-64 z-10 h-full fixed inset-0 bg-primary text-white px-[20px] py-4 border-r-[1px] border-gray-300 shadow-2xl">
      <div className="flex items-center gap-2 p-4 mt-[80px]">
        <MdOutlineAdminPanelSettings className="text-[20px]" />
        <h2 className="text-[25px]">Control Panel</h2>
      </div>
      <div className="flex flex-col mt-11 border-y-[1px] border-secondary pt-4 overflow-y-auto h-[calc(100vh-300px)] scrollbar-thumb-secondary">
        {menuItems.map((item, index) => (
          <NavLink
            to={`${item.name.toLowerCase().replace(/\s/g, "")}`}
            key={index}
            className="flex items-center gap-2 p-2 rounded-[20px] hover:bg-secondary my-2 cursor-pointer"
          >
            {item.icon}
            <span>{item.name}</span>
          </NavLink>
        ))}
      </div>

      <div className="absolute bottom-4 left-0 w-full">
        <LogOut />
      </div>
    </div>
  );
};

export default SideBar;
