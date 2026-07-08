import React, { useEffect, useMemo, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { api } from "../../../Api/Axios";
import { FaPen, FaUser, FaUsers } from "react-icons/fa6";
import Loader from "../../../Components/Website/Loader";
import Table from "../../../Components/Dashboard/Table";
import { RiCalendar2Line } from "react-icons/ri";
import { VscWorkspaceTrusted } from "react-icons/vsc";
import { IMGES_URL } from "../../../Api/Api";

const Profile = () => {
  const { id } = useParams();
  const [user, setUser] = useState({});
  const nav = useNavigate();
  const [loading, setLoading] = useState(true);
  const userInfo = [
    {
      label: "First Name",
      value: user.firstName,
    },
    {
      label: "Last Name",
      value: user.lastName,
    },
    {
      label: "Email",
      value: user.email,
    },
    {
      label: "User Name",
      value: user.userName,
    },
    {
      label: "Age",
      value: user.age,
    },
    {
      label: "Phone Number",
      value: user.phoneNumber || "Not Provided",
    },
  ];
  useEffect(() => {
    async function getUser() {
      try {
        setLoading(true);
        const res = await api.get(`Users/${id}`);
        setUser(res.data);
      } catch (err) {
        if (err.response && err.response.status === 404) {
          nav("/err404");
        }
      } finally {
        setLoading(false);
      }
    }
    getUser();
  }, [id]);
  const head = ["id", "tripTitle", "startDate", "tripStatus", "totalPrice"];
  const tabelHead = useMemo(() => {
    return head.map((h) => {
      return h === "tripTitle"
        ? "Trip name"
        : h === "startDate"
          ? "Date"
          : h === "tripStatus"
            ? "Status"
            : h === "totalPrice"
              ? "Total Price"
              : h === "id"
                ? "Booking ID"
                : h;
    });
  }, [id]);
  if (loading) {
    return <Loader />;
  }
  return (
    <div>
      <div className="heading flex justify-between items-start">
        <div className="flex items-center gap-x-4">
          {user?.profileImgUrl === null ? (
            <img
              src="https://cdn-icons-png.flaticon.com/512/149/149071.png"
              alt=""
              className="w-[100px] h-[100px] rounded-full"
            />
          ) : (
            <img
              src={`${IMGES_URL}${user?.profileImgUrl}`}
              alt="image profile"
              className="w-[100px] h-[100px] rounded-full object-cover"
            />
          )}
          <div>
            <h2 className="text-[50px] text-primary font-bold capitalize">
              {user.firstName} {user.lastName}
            </h2>
            <div className="flex items-center gap-x-4">
              <p
                className={`text-[20px] text-gray-600 ${user.roles?.includes("Admin") ? " bg-primary text-white capitalize" : " lowercase bg-tertiary text-white px-5"} w-fit py-1 px-4 rounded-[25px] flex justify-center items-center rounded-2xl`}
              >
                {user.roles}
              </p>
              {user.roles?.includes("Admin") ? (
                <p className="flex items-center gap-x-2 text-[20px] text-gray-600">
                  <VscWorkspaceTrusted /> Verified Personal
                </p>
              ) : null}
            </div>
          </div>
        </div>
        <div className="btns flex gap-x-4 items-center mt-4">
          <Link
            to={`/dashboard/users`}
            className="bg-primary text-white py-2 px-4 rounded-[25px] hover:bg-secondary transition-colors flex items-center gap-x-2"
          >
            <FaUsers /> Back to Users
          </Link>
          <Link
            to={`/dashboard/users/${id}`}
            className="bg-primary text-white py-2 px-4 rounded-[25px] hover:bg-secondary transition-colors flex items-center gap-x-2"
          >
            <FaPen /> Edit Profile
          </Link>
        </div>
      </div>
      <div className="profile-info mt-[50px] bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[30px] text-secondary font-semibold mb-4 flex items-center gap-x-2">
          <FaUser /> User Information
        </h3>
        <div className="profile-details mt-10 grid grid-cols-1 md:grid-cols-2 gap-x-10 gap-y-4">
          {userInfo.map((info, index) => (
            <div className="flex flex-col gap-y-2" key={index}>
              <p className="text-[20px] text-gray-600">{info.label}</p>
              <p className="text-[20px] text-primary font-semibold border-b border-gray-300 rounded-md px-3 py-2">
                {info.value}
              </p>
            </div>
          ))}
        </div>
      </div>
      <div className="user-trips mt-[50px] bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[30px] text-secondary font-semibold mb-4 flex items-center gap-x-2">
          <RiCalendar2Line /> User Bookings
        </h3>
        <div className="trips-table mt-10 overflow-x-auto ">
          <table className="w-full min-w-[800px] border-collapse border border-gray-300">
            <thead className="bg-gray-100 w-full  w-[150%]">
              <tr>
                {tabelHead.map((header, index) => (
                  <th
                    key={index}
                    className="text-[20px] uppercase text-tertiary font-semibold border-b border-gray-300 py-2 px-3 w-auto"
                  >
                    {header}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {user.bookings?.length > 0 ? (
                user.bookings.map((booking, index) => (
                  <tr key={index} className="border-b border-gray-300 w-full">
                    {head.map((h, index) => (
                      <td
                        key={index}
                        className="text-[18px] text-gray-800 py-2  text-center"
                      >
                        {h === "startDate"
                          ? new Date(booking[h]).toLocaleDateString()
                          : h === "totalPrice"
                            ? `$${booking[h].toLocaleString()}`
                            : h === "id"
                              ? `#${booking[h]}`
                              : booking[h]}
                      </td>
                    ))}
                  </tr>
                ))
              ) : (
                <tr>
                  <td
                    colSpan="12"
                    className="text-[20px] text-gray-600 py-2 px-3 text-center"
                  >
                    No Bookings Found.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Profile;
