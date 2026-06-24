import React, { useEffect, useState } from "react";
import { api } from "../../Api/Axios";
import { FaUsers } from "react-icons/fa6";
import { VscWorkspaceTrusted } from "react-icons/vsc";
import { IoBedOutline } from "react-icons/io5";
import { PiBoatFill } from "react-icons/pi";
import { CgUnavailable } from "react-icons/cg";
import { MdEventAvailable, MdOutlineWater } from "react-icons/md";
const BoatCards = () => {
  const [info, setInfo] = useState([]);
  useEffect(() => {
    async function information() {
      try {
        const res = await api.get("/Boats/dashboard-stats");
        setInfo(res.data);
        console.log(res.data);
      } catch (err) {
        console.log(err);
      }
    }
    information();
  }, []);
  return (
    <div className="my-[60px] mx-auto grid gap-2 grid-cols-1 md:grid-cols-2">
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <PiBoatFill className="text-[40px]  text-background rounded-full bg-secondary" />
        <div>
          <h3 className="text-[23px] text-primary uppercase font-light">
            Total Vessels
          </h3>
          <span className="text-primary text-[23px] font-bold">
            {info.totalBoats}
          </span>
        </div>
      </div>
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <MdEventAvailable className="text-[40px]  text-background rounded-full bg-[#154c7b]" />

        <div>
          <h3 className="text-[23px] text-[#154c7b] uppercase font-light">
            Availabel
          </h3>
          <span className="text-[#154c7b] text-[23px] font-bold">
            {info.available}
          </span>
        </div>
      </div>
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <MdOutlineWater className="text-[40px]  text-background rounded-full bg-[#0e5f6f]" />
        <div>
          <h3 className="text-[23px] text-[#0e5f6f] uppercase font-light">
            At Sea
          </h3>
          <span className="text-[#0e5f6f] text-[23px] font-bold">
            {info.atSea}
          </span>
        </div>
      </div>
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <CgUnavailable className="text-[40px]  text-background rounded-full bg-[#0e5f6f]" />
        <div>
          <h3 className="text-[23px] text-[#0e5f6f] uppercase font-light">
            Out of Services
          </h3>
          <span className="text-[#0e5f6f] text-[23px] font-bold">
            {info.outOfService}
          </span>
        </div>
      </div>
    </div>
  );
};

export default BoatCards;
