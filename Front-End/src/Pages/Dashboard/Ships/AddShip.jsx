import React, { useMemo, useRef, useState } from "react";
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
import { useNavigate } from "react-router-dom";

const AddShip = () => {
  const type = ["Available", "AtSea", "OutOfService"];
  const [images, setImages] = useState([]);
  const imagesRef = useRef(null);
  const nav = useNavigate()
  const yup = Yup.object({
    Name: Yup.string()
      .min(3, "Name must be at least 3 characters")
      .max(50, "Name is too long")
      .required("Name of vessel is required"),

    Description: Yup.string()
      .min(10, "Description must be at least 10 characters")
      .required("Description is required"),

    Type: Yup.string()
      .oneOf(["Available", "AtSea", "OutOfService"])
      .required("Type of Boat is Required"),

    Capacity: Yup.number()
      .typeError("Capacity must be a number")
      .positive("Capacity must be greater than 0")
      .integer("Capacity must be integer")
      .required("Capacity is required"),

    YearBuilt: Yup.number()
      .typeError("Year Built must be a number")
      .min(1900, "Year is too old")
      .max(new Date().getFullYear(), "Invalid year")
      .required("Year Built is required"),

    MaxSpeed: Yup.number()
      .typeError("Max Speed must be a number")
      .positive("Speed must be greater than 0")
      .required("Max Speed is required"),

    HasWifi: Yup.boolean(),

    HasFoodFacility: Yup.boolean(),
  });
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
      Images: [],
    },
    validationSchema: yup,
    validateOnBlur: true,
    validateOnChange: true,
    onSubmit: async (values) => {
      const data = new FormData();
      data.append("Name", values.Name);
      data.append("Description", values.Description);
      data.append("Type", values.Type);
      data.append("Capacity", values.Capacity);
      data.append("YearBuilt", values.YearBuilt);
      data.append("MaxSpeed", values.MaxSpeed);
      data.append("HasWifi", values.HasWifi);
      data.append("HasFoodFacility", values.HasFoodFacility);
      values.Images.forEach((image) => {
        data.append("Images", image);
      });
      try {
        const res = await api.post("/Boats", data);
        nav("/dashboard/captains")
      } catch (err) {
        console.log(err.message);
      }
    },
  });

  function handelimage(e) {
    setImages((prev) => [...prev, ...e.target.files]);
    formik.setFieldValue("Images", [...e.target.files]);
  }
  function deleteImage(name) {
    setImages((prev) => prev.filter((img) => img.name !== name));
  }
  const ImagesShow = useMemo(
    () =>
      images.map((image, k) => {
        return (
          <div key={k} className=" relative">
            <img src={URL.createObjectURL(image)} className=" rounded-md w-[95%] mx-auto h-[200px]" />
            <FaXmark
              onClick={(e) => deleteImage(image.name)}
              className=" absolute top-0 right-1 bg-red-600 text-[17px] text-background cursor-pointer w-[20px] h-[20px] rounded-full"
            />
          </div>
        );
      }),
    [images],
  );
  return (
    <form onSubmit={formik.handleSubmit}>

      <div className="top flex justify-between items-center my-[30px]">
        <h2 className="text-[50px] text-primary font-bold">Add New Vessel</h2>
        <button type="submit">
          <Btn text={"Add to fleet"} class={"mr-12"} />
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
                  Type :{" "}
                </label>
                <select
                  name="Type"
                  value={formik.values.Type}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                >
                  {formik.values.Type === "" && (
                    <option value="">Select Type</option>
                  )}

                  {type.map((t, i) => (
                    <option key={i} value={t}>
                      {t}
                    </option>
                  ))}
                </select>
                {formik.touched.Type && formik.errors.Type && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.Type}
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
          <input
            ref={imagesRef}
            onChange={handelimage}
            type="file"
            name="image"
            id="image"
            multiple
            hidden
          />
          <div
            onClick={() => imagesRef.current.click()}
            className="box cursor-pointer bg-gray-300 w-[90%] border-gray-600 border-dashed border-2 mx-auto rounded-md my-3 flex flex-col items-center"
          >
            <span className="text-[100px] text-secondary">
              <FaCloudUploadAlt />
            </span>
            <p className="text-gray-700 text-[18px] my-2">
              Drag && Drop Images
            </p>
            <p className="text-gray-500 text-[14px] my-1">
              Recommended 800x800px
            </p>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-1 mx-2">
            {ImagesShow}
          </div>
        </div>
      </div>
    </form>
  );
};

export default AddShip;
