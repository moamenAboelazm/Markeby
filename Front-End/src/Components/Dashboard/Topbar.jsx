import React from "react";
import { NavLink } from "react-router-dom";
import { FaRegUserCircle } from "react-icons/fa";
import { useRole } from "../../Hooks/UseRole";
const Topbar = () => {
  const { name } = useRole();
  const Links = [
    {
      name: "Home",
      to: "/",
    },
    {
      name: "Dashboard",
      to: "/dashboard",
    },
    {
      name: "Expeditions",
      to: "/dashboard/expeditions",
    },
    {
      name: "About",
      to: "/about",
    },
  ];
  return (
    <div className="fixed inset-0 w-full h-16 bg-background z-50 flex items-center justify-between px-[100px] shadow-md py-[40px]">
      <div className="logo">
        <h1 className="text-[35px] text-primary font-light uppercase letter-spacing-[2px]">
          Markeby
        </h1>
      </div>
      {
        <div className="links flex items-center gap-4">
          {Links.map((link, index) => (
            <NavLink
              key={index}
              to={link.to}
              className="text-[18px] text-primary hover:text-secondary"
            >
              {link.name}
            </NavLink>
          ))}
        </div>
      }
      <div className="user flex items-center gap-2">
        <FaRegUserCircle className="text-[30px] text-primary" />
        <span className="text-[18px] text-primary">{name}</span>
      </div>
    </div>
  );
};

export default Topbar;
