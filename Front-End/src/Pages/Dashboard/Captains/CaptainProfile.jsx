import React, { useEffect, useMemo, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { api } from "../../../Api/Axios";
import { IMGES_URL } from "../../../Api/Api";
import { FaPen, FaUsers } from "react-icons/fa";
import { GiCaptainHatProfile } from "react-icons/gi";
import { BiTrip } from "react-icons/bi";

const CaptainProfile = () => {
  const { id } = useParams();
  const [captain, setCaptain] = useState([]);
  const nav = useNavigate();
  const userInfo = [
    {
      label: "Full Name",
      value: captain.fullName,
    },
    {
      label: "Email",
      value: captain.email,
    },
    {
      label: "Language",
      value: captain.languages,
    },
    {
      label: "Phone Number",
      value: captain.phoneNumber,
    },
    {
      label: "Rank",
      value: captain.rank,
    },
    {
      label: "years of Experience",
      value: captain.yearsOfExperience,
    },
  ];
  const head = [
    "boatImg",
    "title",
    "type",
    "boatName",
    "startLocation",
    "startTime",
    "endTime",
    "status",
  ];
  const tabelHead = useMemo(() => {
    return head.map((h) => {
      return h === "boatImg"
        ? "protoflio"
        : h === "startTime"
          ? "Start Date"
          : h === "boatName"
            ? "Name"
            : h === "totalPrice"
              ? "Total Price"
              : h === "id"
                ? "Booking ID"
                : h === "startLocation"
                  ? "Location"
                  : h === "endTime"
                    ? "End Time"
                    : h;
    });
  }, [id]);
  useEffect(() => {
    async function getCaptain() {
      try {
        const res = await api.get(`/Captains/${id}`);
        setCaptain(res.data);
      } catch (err) {
        if (err.response.status === 400) {
          nav("/err404");
        }
      }
    }
    getCaptain();
  }, []);
  return (
    <div>
      <div className="heading flex justify-between items-start">
        <div className="flex items-center gap-x-4">
          {captain?.profileImgUrl === null ? (
            <img
              src="https://cdn-icons-png.flaticon.com/512/149/149071.png"
              alt=""
              className="w-[100px] h-[100px] rounded-full"
            />
          ) : (
            <img
              src={`${IMGES_URL}${captain?.profilePhotoUrl}`}
              alt="image profile"
              className="w-[100px] h-[100px] rounded-full object-cover"
            />
          )}
          <div>
            <h2 className="text-[50px] text-primary font-bold capitalize">
              {captain.fullName}
            </h2>
            <div className="flex items-center gap-x-4">
              <p
                className={`text-[20px] text-gray-600  lowercase bg-tertiary text-white px-5 w-fit py-1 px-4 rounded-[25px] flex justify-center items-center rounded-2xl`}
              >
                {captain.rank}
              </p>
            </div>
          </div>
        </div>
        <div className="btns flex gap-x-4 items-center mt-4">
          <Link
            to={`/dashboard/captains`}
            className="bg-primary text-white py-2 px-4 rounded-[25px] hover:bg-secondary transition-colors flex items-center gap-x-2"
          >
            <FaUsers /> Back to Captains
          </Link>
          <Link
            to={`/dashboard/captains/${id}`}
            className="bg-primary text-white py-2 px-4 rounded-[25px] hover:bg-secondary transition-colors flex items-center gap-x-2"
          >
            <FaPen /> Edit Profile
          </Link>
        </div>
      </div>
      <div className="profile-info mt-[50px] bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[30px] text-secondary font-semibold mb-4 flex items-center gap-x-2">
          <GiCaptainHatProfile /> Captain Information
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
        <div className="my-5">
          <p className="text-[20px] text-gray-600">Bio</p>
          <p className="text-[20px] text-primary font-semibold border-b border-gray-300 rounded-md px-3 py-2">
            {captain.bio}
          </p>
        </div>
      </div>
      <div className="profile-info mt-[50px] bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[30px] text-secondary font-semibold mb-4 flex items-center gap-x-2">
          <BiTrip /> Trip History
        </h3>
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
            {captain.tripsTable?.length > 0 ? (
              captain.tripsTable.map((trip, index) => (
                <tr key={index} className="border-b border-gray-300 w-full">
                  {head.map((h, index) => (
                    <td
                      key={index}
                      className="text-[18px] text-gray-800 py-2  text-center"
                    >
                      {h === "startTime" || h === "endTime" ? (
                        new Date(trip[h]).toLocaleDateString()
                      ) : h === "boatImg" ? (
                        <Link to={`/dashboard/trips/protoflio/${trip["id"]}`}>
                          <img src={`${IMGES_URL}${trip[h]}`} alt="trip protoflio" className="w-[60px] h-[60px] m-2 rounded-full border" />
                        </Link>
                      ) : h === "id" ? (
                        `#${trip[h]}`
                      ) : (
                        trip[h]
                      )}
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
                  No Trips Found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default CaptainProfile;
