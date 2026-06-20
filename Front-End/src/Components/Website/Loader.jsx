import React from "react";
import "./Loader.css";
const Loader = () => {
  return (
    <div className="bg-[#0101014f] fixed inset-0 z-50 h-screen w-full flex items-center justify-center">
      <div className="spinner">
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
      </div>
    </div>
  );
};

export default Loader;
