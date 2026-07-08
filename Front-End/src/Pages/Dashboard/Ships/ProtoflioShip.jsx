import React, { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { api } from "../../../Api/Axios";
import { IMGES_URL } from "../../../Api/Api";
import { FaPen, FaUsers } from "react-icons/fa";
import { IoBoatSharp, IoSettingsOutline } from "react-icons/io5";
import Loader from "../../../Components/Website/Loader";
import Gallery from "../../../Components/Utils/Gallery";
import { PiGooglePhotosLogoThin } from "react-icons/pi";

const ProtoflioShip = () => {
  const { id } = useParams();
  const [vessel, setVessel] = useState([]);
  const [loading, setLoading] = useState(false);
  const nav = useNavigate();
  const technical = ["yearBuilt", "maxSpeed", "capacity", "type"];
  useEffect(() => {
    async function fetchVessel() {
      try {
        setLoading(true);
        const res = await api.get(`/Boats/${id}`);
        setVessel(res.data);
      } catch (err) {
        nav("/err404");
      } finally {
        setLoading(false);
      }
    }
    fetchVessel();
  }, [id]);
  console.log(vessel);
  if (loading) return <Loader />;
  return (
    <div>
      <div className="main-img relative w-full h-[70vh] rounded-md overflow-hidden">
        <img
          src={`${IMGES_URL}${vessel.mainImageUrl}`}
          className=" absolute inset-0 w-full h-full object-cover"
        />
        <div className="bg-black/15 absolute inset-0 w-full h-full z-30"></div>
        <div className="heading z-40 absolute left-8 bottom-3 text-white flex justify-between w-[95%] items-center">
          <div className="flex flex-col">
            <span className="bg-white/25 text-white px-5 py-2 rounded-[20px] w-fit">
              {vessel.status}
            </span>
            <h2 className="text-[70px] font-bold">{vessel.name}</h2>
          </div>
          <div className="btns flex gap-x-4 items-center mt-10">
            <Link
              to={`/dashboard/vessels`}
              className="bg-primary text-white py-2 px-4 rounded-[25px] hover:bg-secondary transition-colors flex items-center gap-x-2"
            >
              <IoBoatSharp /> Back to Vessels
            </Link>
            <Link
              to={`/dashboard/vessels/${id}`}
              className="bg-primary text-white py-2 px-4 rounded-[25px] hover:bg-secondary transition-colors flex items-center gap-x-2"
            >
              <FaPen /> Edit Profile
            </Link>
          </div>
        </div>
      </div>
      <div className="images my-[70px]">
        <h2 className="text-[30px] text-secondary font-bold mb-4 flex items-center gap-x-2">
          <PiGooglePhotosLogoThin /> Vessel Gallery
        </h2>
        <Gallery images={vessel.images} />
      </div>
      <div className="my-[60px]">
        <h2 className="text-[30px] text-secondary font-bold mb-4 flex items-center gap-x-2">
          <IoSettingsOutline /> Technical Specifications
        </h2>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5">
          {technical.map((tech, key) => (
            <div
              key={key}
              class="group w-52 h-[70px] mx-auto [perspective:1000px] cursor-pointer"
            >
              <div class="relative w-full h-full transition-transform duration-500 [transform-style:preserve-3d] group-hover:[transform:rotateY(180deg)]">
                <div class="absolute w-full h-full [backface-visibility:hidden] flex items-center justify-center rounded-md bg-white border border-primary">
                  <h2 className=" uppercase text-[20px] font-bold text-secondary">
                    {tech}
                  </h2>
                </div>

                <div class="absolute w-full h-full [backface-visibility:hidden] flex items-center justify-center rounded-md bg-secondary text-white [transform:rotateY(180deg)]">
                  <p className="text-[18px] capitalize font-bold">{vessel[tech]?.toLocaleString()}</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ProtoflioShip;
