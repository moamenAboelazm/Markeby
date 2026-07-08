import React from "react";
import { IoIosLogOut } from "react-icons/io";
import Cookie from "universal-cookie";
const LogOut = () => {
  const cookies = new Cookie();
  const handleLogout = () => {
    cookies.remove("token", { path: "/" });
    cookies.remove("role", { path: "/" });
    cookies.remove("reftoken", { path: "/" });
    window.location.href = "/login"; // Redirect to login page or any other page
  };
  return (
    <button
      className=" bg-tertiary text-white py-2 px-4 rounded-md hover:bg-secondary hover:text-white
     mt-4 flex items-center w-[80%] mx-auto cursor-pointer"
      onClick={handleLogout}
    >
      <IoIosLogOut className="text-[20px] mr-2" />
      Logout
    </button>
  );
};

export default LogOut;
