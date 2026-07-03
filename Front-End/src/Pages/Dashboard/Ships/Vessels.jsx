import React, { useEffect, useState } from "react";
import { api } from "../../../Api/Axios";
import CaptainsCards from "../../../Components/Dashboard/CaptainsCards";
import { Link } from "react-router-dom";
import { LuUserPlus } from "react-icons/lu";
import Table from "../../../Components/Dashboard/Table";
import Loader from "../../../Components/Website/Loader";
import { FaPlus } from "react-icons/fa";
import BoatCards from "../../../Components/Dashboard/BoatCards";
import { ALL_BOATS, BOAT } from "../../../Api/Api";

const Vessels = () => {
  const head = ["name", "type", "capacity", "status", "amenities"];
  const [boats, setBoats] = useState([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(false);
  const [isDelete, setIsDelete] = useState(0);
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
            `/Boats/all?PageNumber=${navgate.pageNumber}&PageSize=${navgate.pageSize}&limit=${navgate.limit}`,
          )
          .then((t) => {
            setNavgate((prev) => {
              return {
                ...prev,
                totalCount: t.data.totalCount,
                totalPages: t.data.totalPages,
              };
            });
            setBoats(t.data.items);
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
      const res = await api.get(ALL_BOATS, {
        params: {
          SearchTerm: search,
        },
      });
      setBoats(res.data.items);
    } catch (err) {
      console.log(err);
    }
  }
  async function handelinputSearch(e) {
    setSearch(e.target.value);
    setTimeout(async () => {
      try {
        const res = await api.get(ALL_BOATS, {
          params: {
            SearchTerm: e.target.value,
          },
        });
        setBoats(res.data.items);
      } catch (err) {
        console.log(err);
      }
    }, 3000);
  }
  async function handelDelete(id) {
    try {
      let res = await api.delete(BOAT(id));
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
          <h2 className="text-[50px] text-secondary font-bold">Vessel Management</h2>
          <p className="text-gray-700">
            Oversee your fleet leaders and crew assignments.
          </p>
        </div>
        <Link
          to={"/dashboard/addvessel"}
          className="flex items-center gap-x-2 bg-secondary py-2 px-3 text-[18px] text-white rounded-md hover:bg-primary transition-all duration-300"
        >
          <FaPlus /> Add New Vessel
        </Link>
      </div>
      <div className="cards">
        <BoatCards />
      </div>
      <div className="bg-gray-100 shadow-md rounded-md">
        <div className="flex justify-between items-center py-[50px] w-[95%] mx-auto">
          <h3 className="text-[30px] font-medium text-secondary">
            Vessel Registry
          </h3>
          <div className=" border border-neutral-700 rounded-md overflow-hidden relative">
            <input
              type="text"
              value={search}
              onChange={handelinputSearch}
              placeholder="...Seach Vessel By Name!"
              className="border-none outline-none px-2 my-2 w-[350px]"
            />
            <button
              onClick={handelSearch}
              className="bg-primary w-20 h-[120%] text-background absolute top-0 right-0 pb-2 flex items-center justify-center cursor-pointer hover:bg-secondary"
            >
              Search
            </button>
          </div>
        </div>
        <Table
          head={head}
          data={boats}
          delete={handelDelete}
          setNavgate={setNavgate}
          navgate={navgate}
        />
      </div>
    </div>
  );
};

export default Vessels;
