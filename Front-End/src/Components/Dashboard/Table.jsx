import React from "react";
import { PiDotOutlineFill } from "react-icons/pi";
import { MdDelete } from "react-icons/md";
import Pagenate from "./Pagenate";
const Table = (props) => {
  const headShow = props.head.map((h, k) => {
    return (
      <th
        key={k}
        scope="col"
        class="px-6 py-2 text-[18px] font-medium uppercase"
      >
        {h === "phoneNumber" ? "phone" : h}
      </th>
    );
  });
  const bodyShow = props.data.map((d, i) => {
    return (
      <tr className=" border-b border-gray-300">
        {props.head.map((h, l) => {
          return (
            <td key={l} className="px-6 py-4 capitalize">
              {h === "fullName" ? (
                <div className="flex gap-x-2 items-center">
                  {d["profilePhotoUrl"] !== null ? (
                    <img
                      className="w-[40px] h-[40px] rounded-full object-cover"
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
                    ${d[h].toUpperCase() === "ACTIVE" ? " text-[#a63a14]" : d[h].toUpperCase() === "INACTIVE" ? " text-[#b6b6c4]" : " text-[#6b6f4b] "} text-[14px] rounded-[30px] text-center flex  items-center gap-x-[-25px] `}
                >
                  <PiDotOutlineFill className="text-[50px]" /> {d[h]}
                </p>
              ) : (
                <p className="text-[16px]">{d[h]}</p>
              )}
            </td>
          );
        })}
        <td className="flex justify-between items-center px-4 py-6">
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
        <tbody className="bg-background">{bodyShow}</tbody>
      </table>
      <div className="flex justify-between items-center my-[30px] mr-[20px]">
        <Pagenate navgate={props.navgate} setNavgate={props.setNavgate} />
        <div className="flex items-center gap-x-2">
          <p>{window.location.pathname.replace("/dashboard/","").toUpperCase()} For Page : </p>
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
