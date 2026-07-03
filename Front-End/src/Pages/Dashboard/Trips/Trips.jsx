import React, { useEffect, useState } from "react";
import { api } from "../../../Api/Axios";
import CaptainsCards from "../../../Components/Dashboard/CaptainsCards";
import { Link } from "react-router-dom";
import { LuUserPlus } from "react-icons/lu";
import Table from "../../../Components/Dashboard/Table";
import Loader from "../../../Components/Website/Loader";
import { CAPTAIN, CAPTAINS_PAGED } from "../../../Api/Api";
import { FaPlus } from "react-icons/fa";
import TripsCards from "../../../Components/Dashboard/TripsCards";
import Btn from "../../../Components/Utils/Btn";

const Trips = () => {
  const head = [
    "title",
    "type",
    "status",
    "price",
    "startLocation",
    "startTime",
    "endTime",
    "captainName",
    "boatName",
    "availableSeats",
  ];
  const [trips, setTrips] = useState([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(false);
  const [isDelete, setIsDelete] = useState(0);
  const [part, setPart] = useState(true);
  const [navgate, setNavgate] = useState({
    pageNumber: 1,
    pageSize: 3,
    totalCount: 0,
    totalPages: 0,
  });
  useEffect(() => {
    async function getCaptains() {
      try {
        setLoading(true);
        await api
          .get(
            `/Trips/all?PageNumber=${navgate.pageNumber}&PageSize=${navgate.pageSize}`,
          )
          .then((t) => {
            setNavgate((prev) => {
              return {
                ...prev,
                totalCount: t.data.totalCount,
                totalPages: t.data.totalPages,
              };
            });
            setTrips(t.data.items);
          });
      } catch (err) {
        console.log(err);
      } finally {
        setLoading(false);
      }
    }
    getCaptains();
  }, [isDelete, navgate.pageNumber, navgate.limit, navgate.pageSize]);
  async function handelSearch() {
    try {
      const res = await api.get(`Trips/all?title=${search}`);
      setTrips(res.data.items);
    } catch (err) {
      console.log(err);
    }
  }
  async function handelinputSearch(e) {
    setSearch(e.target.value);
    setTimeout(async () => {
      try {
        const res = await api.get(`/Trips/all?title=${e.target.value}`);
        setTrips(res.data.items);
      } catch (err) {
        console.log(err);
      }
    }, 1000);
  }
  async function handelDelete(id) {
    try {
      let res = await api.delete(`/Trips/${id}`);
      setIsDelete((prev) => prev + 1);
    } catch (err) {
      console.log(err);
    }
  }
  if (loading) {
    return <Loader />;
  }
  return (
    <div>
      <div className="top flex justify-between items-center">
        <div>
          <h2 className="text-[50px] text-secondary font-bold">Trips Management</h2>
          <p className="text-gray-700">
            Oversee your fleet leaders and crew assignments.
          </p>
        </div>
        <Link
          to={"/dashboard/addtrip"}
          className="flex items-center gap-x-2 bg-secondary py-2 px-3 text-[18px] text-white rounded-md hover:bg-primary transition-all duration-300"
        >
          <FaPlus /> Add New Trip
        </Link>
      </div>
      <div className="cards">
        <TripsCards />
      </div>
      <div className="bg-gray-100 shadow-md rounded-md">
        <div className="flex justify-between items-center py-[50px] w-[95%] mx-auto">
          <h3 className="text-[30px] font-medium text-secondary">
            Trips Registry
          </h3>
          <div className="flex gap-x-2 items-center">
            <div className=" border border-neutral-700 rounded-md overflow-hidden relative">
              <input
                type="text"
                value={search}
                onChange={handelinputSearch}
                placeholder="Seach By Name Of The Trip!"
                className="border-none outline-none px-2 my-2 w-[350px]"
              />
              <button
                onClick={handelSearch}
                className="bg-primary w-20 h-[120%] text-background absolute top-0 right-0 pb-2 flex items-center justify-center cursor-pointer hover:bg-secondary"
              >
                Search
              </button>
            </div>
            <Btn
              text="Swap"
              onClick={() => {
               setPart((prev) => !prev);
              }}
            />
          </div>
        </div>
        <Table
          head={part?head.slice(0, 5):head.slice(5, head.length)}
          data={trips}
          pageNumber={navgate.pageNumber}
          pageSize={navgate.pageSize}
          delete={handelDelete}
          setNavgate={setNavgate}
          navgate={navgate}
        />
      </div>
    </div>
  );
};

export default Trips;
