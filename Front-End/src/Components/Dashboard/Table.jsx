import React from "react";
import { PiDotOutlineFill } from "react-icons/pi";
import { MdDelete, MdOutlineWifi, MdOutlineWifiOff } from "react-icons/md";
import { FiEdit } from "react-icons/fi";
import Pagenate from "./Pagenate";
import { Link } from "react-router-dom";
import { FaUtensils } from "react-icons/fa";
const Table = (props) => {
  const headShow = props.head.map((h, k) => {
    return (
      <th
        key={k}
        scope="col"
        class="px-6 py-2 text-[18px] font-medium uppercase"
      >
        {h === "phoneNumber" ? "phone" : h === "fullName" ? "Full Name" : h}
      </th>
    );
  });
  const bodyShow = props.data.map((d, i) => {
    return (
      <tr className=" border-b border-gray-300 bg-background hover:bg-[#f8fbff] transition-all duration-300 ease-in">
        {props.head.map((h, l) => {
          return (
            <td key={l} className="px-6 py-4 capitalize">
              {
                h === "fullName" ? (
                  <div className="flex gap-x-2 items-center">
                    {d["profilePhotoUrl"] !== null ? (
                      <img
                        className="w-[50px] h-[50px] rounded-full object-cover"
                        src={`https://markeby.runasp.net${d["profilePhotoUrl"]}`}
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
                    ${d[h].toUpperCase() === "ACTIVE" || d[h].toUpperCase() === "AVAILABLE" ? " text-[#a63a14]" : d[h].toUpperCase() === "INACTIVE" || d[h].toLowerCase() === "atsea" ? " text-secondary" : " text-tertiary "} text-[14px] rounded-[30px] text-center flex  items-center gap-x-[-25px] `}
                  >
                    <PiDotOutlineFill className="text-[50px]" /> {d[h]}
                  </p>
                ) : h === "name" && h !== "fullName" ? (
                  <div className="flex gap-x-2">
                    <img
                      src={`https://markeby.runasp.net${d["mainImageUrl"]}`}
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
                      {d["hasFoodFacility"]===true?<FaUtensils className="text-cyan-700"/>:<FaUtensils className="text-gray-300"/>} 
                    </span>
                  </div>
                ) : (
                  <p className="text-[16px]">{d[h]}</p>
                )
                // null
                // <p className="text-[16px]">{d[h]}</p>
              }
            </td>
          );
        })}
        <td className="flex justify-between items-center px-4 py-6">
          <Link
            to={d["id"]}
            className="text-secondary hover:text-primary hover:scale-110 text-[25px] transition-all ease-in duration-300"
          >
            <FiEdit />
          </Link>
          <MdDelete
            onClick={() => props.delete(d["id"])}
            className="text-red-500 hover:scale-110 cursor-pointer text-[30px] duration-300 "
          />
        </td>
      </tr>
    );
  });
  return (
    <div class="relative overflow-x-auto bg-neutral-primary-soft shadow-xs">
      <table class="w-full text-sm text-left rtl:text-right text-body">
        <thead class="text-sm text-body bg-neutral-secondary-soft">
          <tr>
            {headShow}
            <th class="px-6 py-2 text-[18px] font-medium uppercase">Actions</th>
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
