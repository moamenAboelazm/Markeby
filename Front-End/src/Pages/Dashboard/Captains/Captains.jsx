import React, { useEffect, useState } from "react";
import { api } from "../../../Api/Axios";
import CaptainsCards from "../../../Components/Dashboard/CaptainsCards";
import { Link } from "react-router-dom";
import { LuUserPlus } from "react-icons/lu";
import Table from "../../../Components/Dashboard/Table";
import Loader from "../../../Components/Website/Loader";
import { CAPTAIN, CAPTAINS_PAGED } from "../../../Api/Api";

const Captains = () => {
  const head = ["fullName", "rank", "vessel", "status", "phoneNumber"];
  const [captains, setCaptains] = useState([]);
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
            `/Captains/paged?PageNumber=${navgate.pageNumber}&PageSize=${navgate.pageSize}`,
          )
          .then((t) => {
            setNavgate((prev) => {
              return {
                ...prev,
                totalCount: t.data.totalCount,
                totalPages: t.data.totalPages,
              };
            });
            setCaptains(t.data.items);
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
      const res = await api.get("/Captains/paged", {
        params: {
          SearchTerm: search,
        },
      });
      setCaptains(res.data.items);
    } catch (err) {
      console.log(err);
    }
  }
  async function handelinputSearch(e) {
    setSearch(e.target.value);
    setTimeout(async () => {
      try {
        const res = await api.get(CAPTAINS_PAGED, {
          params: {
            fullName: e.target.value,
          },
        });
        setCaptains(res.data.items);
      } catch (err) {
        console.log(err);
      }
    }, 3000);
  }
  async function handelDelete(id) {
    try {
      let res = await api.delete(CAPTAIN(id));
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
          <h2 className="text-[50px] text-secondary font-bold">Captain Management</h2>
          <p className="text-gray-700">
            Oversee your fleet leaders and crew assignments.
          </p>
        </div>
        <Link
          to={"/dashboard/addcaptain"}
          className="flex items-center gap-x-2 bg-secondary py-2 px-3 text-[18px] text-white rounded-md hover:bg-primary transition-all duration-300"
        >
          <LuUserPlus /> Add New Captain
        </Link>
      </div>
      <div className="cards">
        <CaptainsCards />
      </div>
      <div className="bg-gray-100 shadow-md rounded-md">
        <div className="flex justify-between items-center py-[50px] w-[95%] mx-auto">
          <h3 className="text-[30px] font-medium text-secondary">
            Captain Registry
          </h3>
          <div className=" border border-neutral-700 rounded-md overflow-hidden relative">
            <input
              type="text"
              value={search}
              onChange={handelinputSearch}
              placeholder="Seach By Name Of The Captain!"
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
          data={captains}
          delete={handelDelete}
          setNavgate={setNavgate}
          navgate={navgate}
        />
      </div>
    </div>
  );
};

export default Captains;
