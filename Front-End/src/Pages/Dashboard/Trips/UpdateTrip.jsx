import React, { useEffect, useMemo, useRef, useState } from "react";
import Btn from "../../../Components/Utils/Btn";
import { IoIosInformationCircleOutline } from "react-icons/io";
import { MdDetails, MdOutlineSchedule } from "react-icons/md";
import * as Yup from "yup";
import {
  FaCloudUploadAlt,
  FaServicestack,
  FaUtensils,
  FaWifi,
} from "react-icons/fa";
import { PiBowlFoodBold } from "react-icons/pi";
import { FaUsers, FaXmark } from "react-icons/fa6";
import { useFormik } from "formik";
import { api } from "../../../Api/Axios";
import { useNavigate, useParams } from "react-router-dom";
import { BOATS, IMGES_URL } from "../../../Api/Api";
import { IoDocumentTextOutline } from "react-icons/io5";
import { VscSettings } from "react-icons/vsc";

const UpdateTrip = () => {
  //   const type = ["Available", "AtSea", "OutOfService"];
  const [captains, setCaptains] = useState([]);
  const [vessels, setVessels] = useState([]);
  const [images, setImages] = useState([]);
  const imagesRef = useRef(null);
  const nav = useNavigate();
  const { id } = useParams();
  const types = [
    { name: "Snorkeling", value: "Snorkeling" },
    { name: "Diving", value: "Diving" },
    { name: "Fishing", value: "Fishing" },
    { name: "Party", value: "Party" },
    { name: "Private ", value: "Private " },
  ];
  const Status = [
    { name: "Scheduled", value: "Scheduled" },
    { name: "Ongoing", value: "Ongoing" },
    { name: "Completed", value: "Completed" },
    { name: "Cancelled", value: "Cancelled" },
  ];
  const formSchema = Yup.object().shape({
    title: Yup.string().required("Title is required"),
    Description: Yup.string().required("Description is required"),
    Type: Yup.string().required("Type is required"),
    StartLocation: Yup.string().required("Start Location is required"),
    StartTime: Yup.date().required("Start Time is required"),
    EndTime: Yup.date().required("End Time is required"),
    BoatId: Yup.string().required("Boat ID is required"),
    CaptainId: Yup.string().required("Captain ID is required"),
    Price: Yup.number()
      .required("Price is required")
      .positive("Price must be a positive number"),
    Status: Yup.string()
      .oneOf(["Scheduled", "Ongoing", "Completed", "Cancelled"])
      .required("Status is required"),
    AvailableSeats: Yup.number()
      .required("Available Seats is required")
      .positive("Available Seats must be a positive number"),
  });
  const formik = useFormik({
    initialValues: {
      title: "",
      Description: "",
      Type: "",
      StartLocation: "",
      StartTime: "",
      EndTime: "",
      BoatId: "",
      CaptainId: "",
      Price: "",
      Status: "Scheduled",
      AvailableSeats: "",
      Images: [],
    },
    validateOnBlur: true,
    validateOnChange: true,
    validationSchema: formSchema,
    validateOnChange: true,
    onSubmit: async (values) => {
      const data = new FormData();
      data.append("Id", id);
      data.append("Title", values.title);
      data.append("Description", values.Description);
      data.append("Type", values.Type);
      data.append("StartLocation", values.StartLocation);
      data.append("StartTime", new Date(values.StartTime).toISOString());
      data.append("EndTime", new Date(values.EndTime).toISOString());
      data.append("BoatId", values.BoatId);
      data.append("CaptainId", values.CaptainId);
      data.append("Price", values.Price);
      data.append("Status", values.Status);
      data.append("AvailableSeats", values.AvailableSeats);

      values.Images.forEach((image) => {
        if (image instanceof File) {
          data.append("Images", image);
        }
      });
      try {
        const res = await api.put(`/Trips/${id}`, data, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
        nav("/dashboard/trips");
      } catch (err) {
        console.log(err.response);
      }
    },
  });

  useEffect(() => {
    async function getTrip() {
      try {
        const res = await api.get(`Trips/${id}`);
        formik.setFieldValue("title", res.data.title);
        formik.setFieldValue("Type", res.data.type);
        formik.setFieldValue("Description", res.data.description);
        formik.setFieldValue("StartLocation", res.data.startLocation);
        formik.setFieldValue("StartTime", res.data.startTime);
        formik.setFieldValue("EndTime", res.data.endTime);
        formik.setFieldValue("BoatId", res.data.boatId);
        formik.setFieldValue("CaptainId", res.data.captainId);
        formik.setFieldValue("Price", res.data.price);
        formik.setFieldValue("AvailableSeats", res.data.availableSeats);
        formik.setFieldValue("Status", res.data.status);
        formik.setFieldValue("Images", res.data.imagesURLs);
        setImages(res.data.imagesURLs);
      } catch (err) {
        console.log(err.data);
      }
    }
    getTrip();
  }, []);
  useEffect(() => {
    if (!formik.values.StartTime || !formik.values.EndTime) return;

    async function availabel() {
      try {
        const res = await api.get(
          `/Trips/available-resources?startTime=${new Date(formik.values.StartTime).toISOString()}&endTime=${new Date(formik.values.EndTime).toISOString()}`,
        );
        setVessels(res.data.boats);
        setCaptains(res.data.captains);
      } catch (err) {
        console.log(err.response);
      }
    }

    availabel();
  }, [formik.values.StartTime, formik.values.EndTime]);
  function handelimage(e) {
    const files = Array.from(e.target.files);

    setImages((prev) => [...prev, ...files]);

    formik.setFieldValue("Images", [...formik.values.Images, ...files]);
  }
  const ImagesShow = useMemo(
    () =>
      images.map((image, k) => (
        <div key={k} className="relative">
          <img
            src={
              typeof image === "string"
                ? `${IMGES_URL}${image}`
                : URL.createObjectURL(image)
            }
            className="rounded-md w-[95%] mx-auto h-[200px]"
          />
        </div>
      )),
    [images],
  );

  return (
    <form onSubmit={formik.handleSubmit}>
      <div className="top flex justify-between items-center my-[30px]">
        <div>
          <h2 className="text-[50px] text-primary font-bold">Update Trip</h2>
          <p className="text-[16px] text-gray-600 mt-2">
            Configure details, logistic, and Captain assignment
          </p>
        </div>
        <button type="submit">
          <Btn text={"Publish Trip"} className={"mr-12"} />
        </button>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 ">
        <div className="flex flex-col gap-y-10">
          <div className=" bg-white shadow-md p-4 rounded-md">
            <h3 className="flex items-center gap-x-2 my-[5px] text-[25px] text-secondary font-medium">
              <IoDocumentTextOutline />
              core information
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-2 ">
              <div>
                <label
                  htmlFor="name"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Trip Title :
                </label>
                <input
                  type="text"
                  value={formik.values.title}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="title"
                  placeholder="e.g., Aegean Sea Luxury Sail"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.title && formik.errors.title && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.title}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="name"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Trip Type :
                </label>
                <select
                  name="Type"
                  value={formik.values.Type}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                >
                  {formik.values.Type === "" && (
                    <option value="">Select Type...</option>
                  )}

                  {types.map((type, i) => (
                    <option key={i} value={type.value}>
                      {type.name}
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
                Detailed Description :
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
              <MdOutlineSchedule />
              Logistics & Schedule
            </h3>
            <div>
              <label
                htmlFor="desc"
                className="text-[15px] my-2 font-light text-gray-800 block"
              >
                Start Location :
              </label>
              <input
                type="text"
                name=""
                id=""
                value={formik.values.StartLocation}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                name="StartLocation"
                placeholder="Hurghada"
                className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
              />
              {formik.touched.StartLocation && formik.errors.StartLocation && (
                <span className="text-[14px] text-red-600 my-2">
                  {formik.errors.StartLocation}
                </span>
              )}
            </div>
            <div className="grid md:grid-cols-2 grid-cols-1 gap-x-2">
              <div>
                <label
                  htmlFor="desc"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Start Date & Time :
                </label>
                <input
                  type="datetime-local"
                  name="StartTime"
                  value={formik.values.StartTime}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  id=""
                  placeholder="Hurghada"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.StartTime && formik.errors.StartTime && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.StartTime}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="desc"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  End Date & Time :
                </label>
                <input
                  type="datetime-local"
                  name="EndTime"
                  value={formik.values.EndTime}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  id=""
                  placeholder="Hurghada"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.EndTime && formik.errors.EndTime && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.EndTime}
                  </span>
                )}
              </div>
            </div>
          </div>
          <div className=" bg-white shadow-md p-4 rounded-md">
            <h3 className="flex items-center gap-x-2 my-[20px] text-[25px] text-secondary font-medium">
              <FaUsers />
              Assignments
            </h3>
            <div className="grid md:grid-cols-2 grid-cols-1 gap-x-2">
              <div>
                <label
                  htmlFor=""
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Vessel Assignment :
                </label>
                <select
                  name="BoatId"
                  value={formik.values.BoatId}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  disabled={
                    formik.values.StartTime === "" &&
                    formik.values.EndTime === ""
                  }
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                >
                  {formik.values.BoatId === "" && (
                    <option value="">Select Vessel...</option>
                  )}

                  {vessels.map((vessel, i) => (
                    <option key={vessel.id} value={vessel.id}>
                      {vessel.name}
                    </option>
                  ))}
                </select>
                {formik.touched.BoatId && formik.errors.BoatId && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.BoatId}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor=""
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Commanding Captain :
                </label>
                <select
                  name="CaptainId"
                  value={formik.values.CaptainId}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  disabled={
                    formik.values.StartTime === "" &&
                    formik.values.EndTime === ""
                  }
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                >
                  {formik.values.CaptainId === "" && (
                    <option value="">Select Captain Name...</option>
                  )}

                  {captains.map((captain, i) => (
                    <option key={captain.id} value={captain.id}>
                      {captain.fullName}
                    </option>
                  ))}
                </select>
                {formik.touched.CaptainId && formik.errors.CaptainId && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.CaptainId}
                  </span>
                )}
              </div>
            </div>
          </div>
        </div>
        <div className="flex gap-y-5 flex-col">
          <div>
            <div className=" bg-white shadow-md p-2 rounded-md mx-2 ">
              <h3 className="flex items-center gap-x-2 my-[5px] text-[25px] text-secondary font-medium">
                <VscSettings />
                Parameters
              </h3>
              <div>
                <label
                  htmlFor="name"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Base Price (USD) :
                </label>
                <input
                  type="number"
                  value={formik.values.Price}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="Price"
                  placeholder="$ 0.00"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.Price && formik.errors.Price && (
                  <span className="text-[14px] text-red-600 my-2">
                    {formik.errors.Price}
                  </span>
                )}
              </div>
              <div>
                <label
                  htmlFor="name"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Available Seats :
                </label>
                <input
                  type="number"
                  value={formik.values.AvailableSeats}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="AvailableSeats"
                  placeholder="0"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.AvailableSeats &&
                  formik.errors.AvailableSeats && (
                    <span className="text-[14px] text-red-600 my-2">
                      {formik.errors.AvailableSeats}
                    </span>
                  )}
              </div>
              <div>
                <label
                  htmlFor="name"
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Status :
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
                  {Status.map((status) => (
                    <option key={status.value} value={status.value}>
                      {status.name}
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
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-1">
            {ImagesShow}
          </div>
        </div>
      </div>
    </form>
  );
};

export default UpdateTrip;
