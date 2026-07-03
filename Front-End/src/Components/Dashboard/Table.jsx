import React from "react";
import { PiDotOutlineFill } from "react-icons/pi";
import { MdDelete, MdOutlineWifi, MdOutlineWifiOff } from "react-icons/md";
import { FiEdit } from "react-icons/fi";
import Pagenate from "./Pagenate";
import { Link } from "react-router-dom";
import { FaUtensils } from "react-icons/fa";
import { FaEye } from "react-icons/fa6";
import { IMGES_URL } from "../../Api/Api";
const Table = (props) => {
  const headShow = props.head.map((h, k) => {
    return (
      <th
        key={k}
        scope="col"
        className="px-2 py-2 text-[18px] font-medium uppercase"
      >
        {h === "phoneNumber" ? "phone" : h === "fullName" ? "Full Name" : h}
      </th>
    );
  });
  const bodyShow = props.data.map((d, i) => {
    return (
      <tr
        key={i}
        className=" border-b border-gray-300 bg-background hover:bg-[#f8fbff] transition-all duration-300 ease-in"
      >
        {props.head.map((h, l) => {
          return (
            <td key={l} className="px-2 py-3 capitalize">
              {
                h === "fullName" ? (
                  <div className="flex gap-x-2 items-center">
                    {d["profilePhotoUrl"] !== null ? (
                      <img
                        className="w-[50px] h-[50px] rounded-full object-cover"
                        src={`${IMGES_URL}${d["profilePhotoUrl"]}`}
                      />
                    ) : (
                      <p className="w-[70px] h-[70px] rounded-full bg-secondary flex items-center justify-center text-background text-[25px]">
                        {d["fullName"][0]}
                      </p>
                    )}
                    <div className="flex flex-col">
                      <p className="text-[18px] text-primary capitalize">
                        {d[h]}
                      </p>
                      <p className="text-[16px] text-tertiary lowercase">
                        {d["email"]}
                      </p>
                    </div>
                  </div>
                ) : h === "status" ? (
                  <p
                    className={`
                    ${d[h].toUpperCase() === "ACTIVE" || d[h].toUpperCase() === "AVAILABLE" || d[h].toLowerCase() === "scheduled" ? " text-[#a63a14]" : d[h].toUpperCase() === "INACTIVE" || d[h].toLowerCase() === "atsea" || d[h].toLowerCase() === "ongoing" ? " text-secondary" : " text-tertiary "} text-[14px] rounded-[30px] text-center flex  items-center gap-x-[-25px] `}
                  >
                    <PiDotOutlineFill className="text-[50px]" /> {d[h]}
                  </p>
                ) : h === "name" && h !== "fullName" ? (
                  <div className="flex gap-x-2 items-center">
                    <img
                      src={`${IMGES_URL}${d["mainImageUrl"]}`}
                      className="w-[65px] h-[65px] rounded-md"
                    />
                    <h3 className="text-[23px] text-secondary">{d[h]}</h3>
                  </div>
                ) : h === "capacity" ? (
                  <p className="text-[18px] font-medium">{d[h]} Pax</p>
                ) : h === "amenities" ? (
                  <div className="flex justify-between items-center text-[25px]">
                    <span className="text-secondary">
                      {d["hasWifi"] === true ? (
                        <MdOutlineWifi />
                      ) : (
                        <MdOutlineWifiOff />
                      )}
                    </span>
                    <span>
                      {d["hasFoodFacility"] === true ? (
                        <FaUtensils className="text-cyan-700" />
                      ) : (
                        <FaUtensils className="text-gray-300" />
                      )}
                    </span>
                  </div>
                ) : h === "startTime" || h === "endTime" ? (
                  <p className="text-[16px]">
                    {new Date(d[h]).toLocaleString("en-EG")}
                  </p>
                ) : h === "profile" ? (
                  d["profileImgUrl"] !== null ? (
                    <Link to={`protoflio/${d["id"]}`}>
                      <img
                        src={`${IMGES_URL}${d["profilePhotoUrl"]}`}
                        className="w-[50px] h-[50px] rounded-full object-cover"
                      />
                    </Link>
                  ) : (
                    <Link
                      to={`/protoflio/${d["id"]}`}
                      className="w-[50px] h-[50px] rounded-full bg-gray-300 flex items-center justify-center text-[20px] font-bold text-tertiary"
                    >
                      {d["userName"][0].toUpperCase()}
                    </Link>
                  )
                ) : h === "roles" ? (
                  d[h][0] === "Admin" ? (
                    <p className="text-[20px] bg-primary text-white py-1 px-2 rounded-[25px] flex justify-center items-center  uppercase hover:scale-110 transition-all duration-300">
                      {d[h][0]}
                    </p>
                  ) : (
                    <p className="text-[20px] bg-tertiary text-white py-1 px-2 rounded-[25px] flex justify-center items-center lowercase hover:scale-110 transition-all duration-300 ">
                      {d[h][0]}
                    </p>
                  )
                ) : (
                  <p className="text-[16px]">{d[h]}</p>
                )
                // null
                // <p className="text-[16px]">{d[h]}</p>
              }
            </td>
          );
        })}
        <td className="flex items-center px-3 py-6 gap-x-3 text-[20px]">
          <Link
            to={`protoflio/${d["id"]}`}
            className="text-tertiary hover:text-secondary hover:scale-110 text-[25px] transition-all ease-in duration-300"
          >
            <FaEye />
          </Link>
          <Link
            to={d["id"]}
            className="text-secondary hover:text-primary hover:scale-110 text-[25px] transition-all ease-in duration-300"
          >
            <FiEdit />
          </Link>
          {props.isHasNoDelete ? null : (
            <MdDelete
              onClick={() => props.delete(d["id"])}
              className="text-red-500 hover:scale-110 cursor-pointer text-[30px] duration-300 "
            />
          )}
        </td>
      </tr>
    );
  });
  return (
    <div className="relative overflow-x-scroll bg-neutral-primary-soft shadow-xs">
      <table className="w-full text-sm text-left rtl:text-right text-body">
        <thead className="text-sm text-body bg-neutral-secondary-soft">
          <tr>
            {headShow}
            <th className="px-6 py-2 text-[18px] font-medium uppercase">
              Actions
            </th>
          </tr>
        </thead>
        <tbody>{bodyShow}</tbody>
      </table>
      <div className="flex justify-between items-center my-[30px] mr-[20px]">
        <Pagenate navgate={props.navgate} setNavgate={props.setNavgate} />
        <div className="flex items-center gap-x-2">
          <p>
            {window.location.pathname.replace("/dashboard/", "").toUpperCase()}{" "}
            For Page :{" "}
          </p>
          <select
            name=""
            id=""
            value={props.navgate.pageSize}
            onChange={(e) =>
              props.setNavgate((prev) => {
                return { ...prev, pageSize: +e.target.value };
              })
            }
          >
            {[3, 5, 7, 10].map((i) => (
              <option key={i} value={i}>
                {i}
              </option>
            ))}
          </select>
        </div>
      </div>
    </div>
  );
};

export default Table;
