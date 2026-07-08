import React, { useEffect, useState } from "react";
import { api } from "../../../Api/Axios";
import CaptainsCards from "../../../Components/Dashboard/CaptainsCards";
import { Link } from "react-router-dom";
import { LuUserPlus } from "react-icons/lu";
import Table from "../../../Components/Dashboard/Table";
import Loader from "../../../Components/Website/Loader";
import { FaPlus, FaUserPlus } from "react-icons/fa";
import BoatCards from "../../../Components/Dashboard/BoatCards";
import { ALL_BOATS, BOAT } from "../../../Api/Api";
import { PiBoatFill } from "react-icons/pi";
import { FaUser, FaUsers } from "react-icons/fa6";

const Users = () => {
  const head = ["profile", "userName", "email", "age", "roles"];
  const [users, setUsers] = useState([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(false);
  const [isDelete, setIsDelete] = useState(0);
  const [totalUsers, setTotalUsers] = useState(0);
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
            `/Users?PageNumber=${navgate.pageNumber}&PageSize=${navgate.pageSize}&limit=${navgate.limit}`,
          )
          .then((t) => {
            setNavgate((prev) => {
              return {
                ...prev,
                totalCount: t.data.totalCount,
                totalPages: t.data.totalPages,
              };
            });
            setUsers(t.data.items);
            setTotalUsers(t.data.totalCount);
    
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
      const res = await api.get("/Users", {
        params: {
          SearchTerm: search,
        },
      });
      setUsers(res.data.items);
    } catch (err) {
      console.log(err);
    }
  }
  async function handelinputSearch(e) {
    setSearch(e.target.value);
    setTimeout(async () => {
      try {
        const res = await api.get("/Users", {
          params: {
            SearchTerm: e.target.value,
          },
        });
        setUsers(res.data.items);
      } catch (err) {
        console.log(err);
      }
    }, 3000);
  }
  if (loading) {
    return <Loader />;
  }
  return (
    <div>
      <div className="top flex justify-between items-center mb-[30px]">
        <div>
          <h2 className="text-[50px] text-secondary font-bold">
            Users Management
          </h2>
          <p className="text-gray-700 w-[480px] text-[17px]">
            Overview of all users registered in the system, including their
            details and status.
          </p>
        </div>
        <div className="px-2 py-3 w-[85%] mx-auto rounded-md shadow-md flex flex-wrap justify-between items-start hover:-translate-y-1.5 transition-all duration-300">
          <FaUsers className="text-[40px]  text-background rounded-full bg-secondary" />
          <div>
            <h3 className="text-[23px] text-primary uppercase font-light">
              Total Users
            </h3>
            <span className="text-primary text-[23px] font-bold">
              {totalUsers}
            </span>
          </div>
        </div>
      </div>
      <div className="bg-gray-100 shadow-md rounded-md">
        <div className="flex justify-between items-center py-[50px] w-[95%] mx-auto">
          <h3 className="text-[30px] font-medium text-secondary">
            User Registry
          </h3>
          <div className=" border border-neutral-700 rounded-md overflow-hidden relative">
            <input
              type="text"
              value={search}
              onChange={handelinputSearch}
              placeholder="...Search User By Name!"
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
          data={users}
          setNavgate={setNavgate}
          navgate={navgate}
          isHasNoDelete={true}
        />
      </div>
    </div>
  );
};

export default Users;
