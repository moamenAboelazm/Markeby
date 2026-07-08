import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../../../Api/Axios";
import { CiLocationOn } from "react-icons/ci";
import { LuDot } from "react-icons/lu";
import { PiDotOutlineFill } from "react-icons/pi";
import Loader from "../../../Components/Website/Loader";
import { MdSchedule } from "react-icons/md";
import { AiTwotoneSchedule } from "react-icons/ai";
import Gallery from "../../../Components/Utils/Gallery";

const Protoflio = () => {
  const { id } = useParams();
  const [trip, setTrip] = useState({});
  const [loading, setLoading] = useState(false);
  useEffect(() => {
    async function getTrip() {
      try {
        setLoading(true);
        const res = await api.get(`/Trips/${id}`);
        setTrip(res.data);
      } catch (err) {
        console.log(err);
        setLoading(false);
      } finally {
        setLoading(false);
      }
    }
    getTrip();
  }, [id]);
  console.log(trip);
  if (loading) {
    return <Loader />;
  }
  return (
    <div>
      <div className="heading flex justify-between items-center">
        <div>
          <h2 className="text-[70px] text-primary font-bold capitalize">
            {trip.title}
          </h2>
          <p className="flex items-center gap-x-2 text-[18px] text-gray-600">
            <CiLocationOn />
            {trip.startLocation}
          </p>
        </div>
        <p
          className={`flex items-center  ${trip.status?.toLowerCase() === "scheduled" ? " text-[#a63a14] bg-[#a63b1472] border border-[#a63b14]" : trip.status?.toLowerCase() === "completed" ? " text-blue-800 border border-blue-500 bg-blue-200" : " text-tertiary "} text-[20px] rounded-[30px] text-center flex  items-center pr-3.5 `}
        >
          <PiDotOutlineFill className="text-[50px]" />
          {trip.status}
        </p>
      </div>
      <div className="flex items-start gap-x-2">
        <div className="trip-overview my-[60px] w-[65%] bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
          <h3 className="text-[27px] capitalize text-secondary font-semibold">
            Trip overview
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-y-4">
            <div className="vessel">
              <p className="text-tertiary text-[23px] my-2">Vessel</p>
              <div className="flex gap-x-2 items-center">
                <img
                  src={`https://markeby.runasp.net${trip.boatImg}`}
                  alt="vessel"
                  className="w-[40px] h-[40px] rounded-full"
                />
                <p className="text-[20px] text-gray-800">{trip.boatName}</p>
              </div>
            </div>
            <div className="Captain">
              <p className="text-tertiary text-[23px] my-2">Captain</p>
              <div className="flex gap-x-2 items-center">
                <img
                  src={`https://markeby.runasp.net${trip.captainImg}`}
                  alt="vessel"
                  className="w-[40px] h-[40px] rounded-full"
                />
                <p className="text-[20px] text-gray-800">{trip.captainName}</p>
              </div>
            </div>
            <div className="Start Time">
              <p className="text-tertiary text-[23px] my-2">Departure</p>
              <div className="flex gap-x-2 items-center">
                <MdSchedule className="text-[30px]" />
                <p className="text-[20px] text-gray-800">
                  {new Date(trip.startTime).toLocaleString("en-EG")}
                </p>
              </div>
            </div>
            <div className="Start Time">
              <p className="text-tertiary text-[23px] my-2">Return</p>
              <div className="flex gap-x-2 items-center">
                <AiTwotoneSchedule className="text-[30px]" />
                <p className="text-[20px] text-gray-800">
                  {new Date(trip.endTime).toLocaleString("en-EG")}
                </p>
              </div>
            </div>
          </div>
        </div>
        <div className="Booking bg-primary w-[35%] my-[60px] rounded-md shadow-md px-4 py-3">
          <h4 className="text-gray-300 uppercase text-[17px]">
            booking status
          </h4>
          <div className="flex items-end justify-between">
            <h5>
              <span className="text-[70px] font-bold text-white">
                {trip.availableSeats}
              </span>
              <sub className="text-[35px] text-gray-300">
                / {trip.boatSeats}
              </sub>
            </h5>
            <p className="text-[16px] text-gray-300 mb-[10px]">
              Seats availabled
            </p>
          </div>
          <div className="progrees w-full bg-gray-500 h-[10px] my-[10px] rounded-md relative">
            <span
              style={{
                width: (trip.availableSeats / trip.boatSeats) * 100 + "%",
              }}
              className={` absolute top-0 left-0 h-full bg-[#2b4eaecc]  rounded-md`}
            ></span>
          </div>
          <p className="text-[16px] text-gray-300 my-[10px]">
            Base Price (Per Person)
          </p>
          <h4 className="text-white text-[35px] font-semibold">
            ${trip.price?.toLocaleString()}
          </h4>
        </div>
      </div>
      <div className="trip-description mb-[60px] w-full bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[27px] capitalize text-secondary font-semibold">
          Expedition Description
        </h3>
        <p className="text-[17px] text-gray-700 mt-5">{trip.description}</p>
      </div>
      <div className="trip-description mb-[60px] w-full bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[27px] capitalize text-secondary font-semibold mb-[30px]">
          Gallery
        </h3>
        {/* <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-2">
          {trip.imagesURLs?.map((img, key) => {
            return (
              <img
                key={key}
                src={`https://markeby.runasp.net${img}`}
                alt="trip-img"
                className="w-full h-[200px] rounded-md hover:scale-105 transition-all duration-300"
              />
            );
          })}
        </div> */}
        <Gallery images={trip.imagesURLs}/>
      </div>
    </div>
  );
};

export default Protoflio;
