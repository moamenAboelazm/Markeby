import React, { useEffect, useMemo, useRef, useState } from "react";
import Btn from "../../../Components/Utils/Btn";
import { IoIosInformationCircleOutline } from "react-icons/io";
import { MdDetails } from "react-icons/md";
import * as Yup from "yup";
import {
  FaCloudUploadAlt,
  FaServicestack,
  FaUtensils,
  FaWifi,
} from "react-icons/fa";
import { PiBowlFoodBold } from "react-icons/pi";
import { FaXmark } from "react-icons/fa6";
import { useFormik } from "formik";
import { api } from "../../../Api/Axios";
import { useNavigate, useParams } from "react-router-dom";
import { BOAT } from "../../../Api/Api";
import { EditiShipSchema } from "../../../Schema/YUP";

const UpdateVessel = () => {
  const status = ["Available", "AtSea", "OutOfService"];
  const [images, setImages] = useState([]);
  const [mainImage, setMainImage] = useState("");
  const { id } = useParams();
  const imagesRef = useRef(null);
  const nav = useNavigate();

  const formik = useFormik({
    initialValues: {
      Name: "",
      Description: "",
      Type: "",
      Capacity: 0,
      YearBuilt: 2026,
      MaxSpeed: 1000,
      HasWifi: true,
      HasFoodFacility: true,
      Status: "",
      Images: [],
    },
    validationSchema: EditiShipSchema,
    validateOnBlur: true,
    validateOnChange: true,
    onSubmit: async (values) => {
      const data = new FormData();
      data.append("Id", id);
      data.append("Name", values.Name);
      data.append("Description", values.Description);
      data.append("Type", values.Type);
      data.append("Status", values.Status);
      data.append("Capacity", values.Capacity);
      data.append("YearBuilt", values.YearBuilt);
      data.append("MaxSpeed", values.MaxSpeed);
      data.append("HasWifi", values.HasWifi);
      data.append("HasFoodFacility", values.HasFoodFacility);
      values.Images.forEach((image) => {
        data.append("Images", image);
      });
      try {
        const res = await api.put(BOAT(id), data);
        nav("/dashboard/vessels");
      } catch (err) {
        console.log(err.message);
      }
    },
  });
  useEffect(() => {
    async function getVessel() {
      try {
        const res = await api.get(BOAT(id));
        console.log(res.data);
        formik.setFieldValue("Name", res.data.name);
        formik.setFieldValue("Description", res.data.description);
        formik.setFieldValue("Status", res.data.status);
        formik.setFieldValue("Capacity", res.data.capacity);
        formik.setFieldValue("YearBuilt", res.data.yearBuilt);
        formik.setFieldValue("MaxSpeed", res.data.maxSpeed);
        formik.setFieldValue("HasWifi", res.data.hasWifi);
        formik.setFieldValue("HasFoodFacility", res.data.hasFoodFacility);
        formik.setFieldValue("Type", res.data.type);
        formik.setFieldValue("Images", res.data.images);
        setMainImage(res.data.mainImageUrl);
      } catch {}
    }
    getVessel();
  }, [id]);
  console.log(formik.values);
  function handelimage(e) {
    setImages((prev) => [...prev, ...e.target.files]);
    formik.setFieldValue("Images", [...e.target.files]);
  }
  function deleteImage(name) {
    setImages((prev) => prev.filter((img) => img.name !== name));
  }
  return (
    <form onSubmit={formik.handleSubmit}>
      <div className="top flex justify-between items-center my-[30px]">
        <h2 className="text-[50px] text-primary font-bold">Update Vessel</h2>
        <button type="submit">
          <Btn text={"Update"} class={"mr-12"} />
        </button>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 ">
        <div className="flex flex-col gap-y-10">
          <div className=" bg-white shadow-md p-4 rounded-md">
            <h3 className="flex items-center gap-x-2 my-[5px] text-[25px] text-secondary font-medium">
              <IoIosInformationCircleOutline />
              Vessel Specifications
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-2">
              <div>
                <label
                  htmlFor="name"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Name :{" "}
                </label>
                <input
                  type="text"
                  value={formik.values.Name}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="Name"
                  placeholder="e.g. Oceanic Explorer"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.Name && formik.errors.Name && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.Name}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="type"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Status :{" "}
                </label>
                <select
                  name="Status"
                  value={formik.values.Status}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                >
                  {formik.values.Status === "" && (
                    <option value="">Select Status...</option>
                  )}

                  {status.map((s, i) => (
                    <option key={i} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
                {formik.touched.Status && formik.errors.Status && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.Status}
                  </span>
                )}
              </div>
            </div>
            <div>
              <label
                htmlFor="desc"
                className="text-[15px] my-2 font-light text-gray-800 block"
              >
                Description :{" "}
              </label>
              <textarea
                value={formik.values.Description}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                name="Description"
                cols={60}
                rows={10}
                id=""
                className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
              ></textarea>
              {formik.touched.Description && formik.errors.Description && (
                <span className="text-[14px] text-red-600 my-2">
                  {formik.errors.Description}
                </span>
              )}
            </div>
          </div>
          <div className=" bg-white shadow-md p-4 rounded-md">
            <h3 className="flex items-center gap-x-2 my-[5px] text-[25px] text-secondary font-medium">
              <MdDetails />
              Technical Details
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-2">
              <div>
                <label
                  htmlFor="capacity"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Capacity :
                </label>
                <input
                  value={formik.values.Capacity}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="Capacity"
                  type="number"
                  placeholder="0"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.Capacity && formik.errors.Capacity && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.Capacity}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="built"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Year Built :
                </label>
                <input
                  value={formik.values.YearBuilt}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="YearBuilt"
                  type="number"
                  placeholder="2024"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.YearBuilt && formik.errors.YearBuilt && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.YearBuilt}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="speed"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Max Speed :
                </label>
                <input
                  value={formik.values.MaxSpeed}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="MaxSpeed"
                  type="number"
                  placeholder="0.00"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.MaxSpeed && formik.errors.MaxSpeed && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.MaxSpeed}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="type"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Type :{" "}
                </label>
                <input
                  type="text"
                  name="Type"
                  value={formik.values.Type}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                  id=""
                />
                {formik.touched.Type && formik.errors.Type && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.Type}
                  </span>
                )}
              </div>
            </div>
          </div>
          <div className=" bg-white shadow-md p-4 rounded-md">
            <h3 className="flex items-center gap-x-2 my-[20px] text-[25px] text-secondary font-medium">
              <FaServicestack />
              Facilities
            </h3>
            <div className="flex justify-between items-center">
              <div className="wifi flex items-center text-[30px] gap-x-5 bg-gray-100 px-5 py-2 rounded-lg">
                <FaWifi className="text-blue-600" />
                <input
                  type="checkbox"
                  name="HasWifi"
                  checked={formik.values.HasWifi}
                  onChange={formik.handleChange}
                  id=""
                  className="w-[50px]"
                />
              </div>
              <div className="food flex items-center text-[30px]  gap-x-5 bg-gray-100 px-5 py-2 rounded-lg">
                <FaUtensils className="text-cyan-800" />
                <input
                  type="checkbox"
                  name="HasFoodFacility"
                  checked={formik.values.HasFoodFacility}
                  onChange={formik.handleChange}
                  id=""
                />
              </div>
            </div>
          </div>
        </div>
        <div>
          <div className="main-img">
            <img
              className="w-[95%] mx-auto h-[200px] object-cover rounded-md"
              src={`https://markeby.runasp.net${mainImage}`}
            />
          </div>
          <div className="small-imgs grid grid-cols-1 md:grid-cols-2 my-5 ml-2 mx-auto gap-2">
            {formik.values.Images.map((img, k) => (
              <div key={k}>
                <img
                  src={`https://markeby.runasp.net${img.imageUrl}`}
                  className="w-[200px] h-[100px] rounded-md object-cover"
                />
              </div>
            ))}
          </div>
        </div>
      </div>
    </form>
  );
};

export default UpdateVessel;
