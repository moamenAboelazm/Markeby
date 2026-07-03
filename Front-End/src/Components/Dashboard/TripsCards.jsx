import React, { useEffect, useState } from "react";
import { api } from "../../Api/Axios";
import { FaUsers } from "react-icons/fa6";
import { VscWorkspaceTrusted } from "react-icons/vsc";
import { IoBedOutline } from "react-icons/io5";
import { SiVirustotal } from "react-icons/si";
import { GrCompliance } from "react-icons/gr";
import { MdOutlineCancelPresentation } from "react-icons/md";
import { AiTwotoneSchedule } from "react-icons/ai";
import { GiFishingBoat } from "react-icons/gi";
const TripsCards = () => {
  const [info, setInfo] = useState([]);
  useEffect(() => {
    async function information() {
      try {
        const res = await api.get("/Trips/dashboard-stats");
        setInfo(res.data);
      } catch (err) {
        console.log(err);
      }
    }
    information();
  }, []);
  return (
    <div className="my-[60px] mx-auto grid gap-2 grid-cols-1 md:grid-cols-2 lg:grid-cols-2">
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <SiVirustotal className="text-[40px]  text-background rounded-full bg-secondary" />
        <div>
          <h3 className="text-[23px] text-primary uppercase font-light">
            Total Trips
          </h3>
          <span className="text-primary text-[23px] font-bold">
            {info.totalTrips}
          </span>
        </div>
      </div>
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <AiTwotoneSchedule className="text-[40px]  text-background rounded-full bg-[#154c7b]" />

        <div>
          <h3 className="text-[23px] text-[#154c7b] uppercase font-light">
            Scheduled Trips
          </h3>
          <span className="text-[#154c7b] text-[23px] font-bold">
            {info.scheduledTrips}
          </span>
        </div>
      </div>
      <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <GrCompliance className="text-[40px]  text-background rounded-full bg-[#0e5f6f]" />
        <div>
          <h3 className="text-[23px] text-[#0e5f6f] uppercase font-light">
            Completed Trips
          </h3>
          <span className="text-[#0e5f6f] text-[23px] font-bold">
            {info.completedTrips}
          </span>
        </div>
      </div>
            <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <GiFishingBoat className="text-[40px]  text-background rounded-full bg-[#0e5f6f]" />
        <div>
          <h3 className="text-[23px] text-[#0e5f6f] uppercase font-light">
            Ongoing Trips
          </h3>
          <span className="text-[#0e5f6f] text-[23px] font-bold">
            {info.ongoingTrips}
          </span>
        </div>
      </div>
            <div className="px-2 py-3 rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
        <MdOutlineCancelPresentation className="text-[40px]  text-background rounded-full bg-[#0e5f6f]" />
        <div>
          <h3 className="text-[23px] text-[#0e5f6f] uppercase font-light">
            Cancelled Trips
          </h3>
          <span className="text-[#0e5f6f] text-[23px] font-bold">
            {info.cancelledTrips}
          </span>
        </div>
      </div>
    </div>
  );
};

export default TripsCards;
