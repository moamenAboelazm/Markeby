import React, { useEffect, useRef, useState } from "react";
import { FaRegUser } from "react-icons/fa";
import { LuContact } from "react-icons/lu";
import { MdAddAPhoto } from "react-icons/md";
import { FaCloudUploadAlt } from "react-icons/fa";
import Btn from "../../../Components/Utils/Btn";
import * as Yup from "yup";
import { useFormik } from "formik";
import { api } from "../../../Api/Axios";
import axios from "axios";
import { useRole } from "../../../Hooks/UseRole";
import { useNavigate, useParams } from "react-router-dom";
import Switch from "../../../Components/Dashboard/Switch";
import Loader from "../../../Components/Website/Loader";
import { CAPTAIN } from "../../../Api/Api";
import { EditiCaptainSchema } from "../../../Schema/YUP";
const AddCaptain = () => {
  const { token } = useRole();
  const { id } = useParams();
  const nav = useNavigate();
  const [loading, setLoading] = useState(false);

  const formik = useFormik({
    initialValues: {
      FullName: "",
      YearsOfExperience: 2,
      Rank: "",
      Languages: "",
      Email: "",
      PhoneNumber: "",
      Bio: "",
      IsAvailable: true,
      ProfilePhoto: "",
    },
    validationSchema: EditiCaptainSchema,
    validateOnBlur: true,
    validateOnChange: true,
    onSubmit: async (values, FormikHelper) => {
      setLoading(true);
      try {
        const data = new FormData();
        data.append("FullName", formik.values.FullName);
        data.append("Email", formik.values.Email);
        data.append("IsAvailable", formik.values.IsAvailable || true);
        data.append("Languages", formik.values.Languages);
        data.append("Bio", formik.values.Bio);
        data.append("PhoneNumber", formik.values.PhoneNumber);
        data.append("Rank", formik.values.Rank);
        data.append("YearsOfExperience", formik.values.YearsOfExperience);
        data.append("ProfilePhoto", formik.values.ProfilePhoto);
        data.append("Id", id);
        const res = await api.put(CAPTAIN(id), data);
        nav("/dashboard/captains");
      } catch (err) {
        console.log(err.response?.data);
      } finally {
        setLoading(false);
      }
    },
  });
  const Rank = ["Master", "Cheif Mate", "Second Officer", "Third Officer"];
  const [image, setImage] = useState();

  useEffect(() => {
    async function fetchCaptain() {
      try {
        const res = api.get(CAPTAIN(id));
        res.then((d) => {
          formik.setFieldValue("FullName", d.data.fullName);
          formik.setFieldValue("YearsOfExperience", d.data.yearsOfExperience);
          formik.setFieldValue("Email", d.data.email);
          formik.setFieldValue("Languages", d.data.languages);
          formik.setFieldValue("Bio", d.data.bio);
          formik.setFieldValue("PhoneNumber", d.data.phoneNumber);
          formik.setFieldValue("Rank", d.data.rank);
          formik.setFieldValue("IsAvailable", d.data.IsAvailable);
          setImage(`https://markeby.runasp.net${d.data["profilePhotoUrl"]}`);
        });
      } catch (err) {
        console.log(err);
      }
    }
    fetchCaptain();
  }, [id]);
  function handelimage(e) {
    const file = e.target.files.item(0);
    if (file) {
      setImage(URL.createObjectURL(file));
      formik.setFieldValue("ProfilePhoto", file);
    }
  }
  const imageRef = useRef("");
  if (loading) {
    return <Loader />;
  }
  return (
    <form onSubmit={formik.handleSubmit} className="p-4">
      <div className="flex justify-between items-end">
        <div>
          <h3 className="text-[50px] text-primary font-bold">
            Edit Captain Profile
          </h3>
          <p className="text-[16px] text-gray-600 mt-2">
            Manage Professional credentials and sea serivce records
          </p>
        </div>
        <div className="flex justify-between items-center flex-col gap-y-2">
          <button type="submit">
            <Btn text={"Update Captain"} className={"ml-12"} />
          </button>
        </div>
      </div>
      <div className="mt-8 flex flex-col md:flex-row-reverse gap-4">
        <div className="flex flex-col gap-y-6">
          <div className="personal-info bg-white shadow-md p-4 rounded-md">
            <h3 className="text-[20px] font-light mb-4 flex items-center text-[25px] gap-2 ">
              {" "}
              <FaRegUser className="text-secondry" /> Captain Information
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
              <div>
                <label
                  htmlFor=""
                  className="text-[15px] my-2 font-light text-gray-800 block"
                >
                  Full legal Name :{" "}
                </label>
                <input
                  value={formik.values.FullName}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="FullName"
                  type="text"
                  placeholder="Full Name"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.FullName && formik.errors.FullName && (
                  <span className="text-[14px] text-red-500">
                    {formik.errors.FullName}
                  </span>
                )}
              </div>
              <div>
                <label
                  className="text-[15px] my-2 font-light text-gray-800 block"
                  htmlFor=""
                >
                  RANK
                </label>
                <select
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  value={formik.values.Rank}
                  name="Rank"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                >
                  {Rank.map((i, k) => (
                    <option key={k} value={i}>
                      {i}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="text-[15px] my-2 font-light text-gray-800 block">
                  Years Of Experiance
                </label>
                <input
                  value={formik.values.YearsOfExperience}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="YearsOfExperience"
                  type="text"
                  placeholder="+20"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.YearsOfExperience &&
                  formik.errors.YearsOfExperience && (
                    <span className="text-[14px] text-red-500">
                      {formik.errors.YearsOfExperience}
                    </span>
                  )}
              </div>
              <div>
                <label className="text-[15px] my-2 font-light text-gray-800 block">
                  Primary Language
                </label>
                <input
                  value={formik.values.Languages}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="Languages"
                  type="text"
                  placeholder="English"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.Languages && formik.errors.Languages && (
                  <span className="text-red-500 text-[14px]">
                    {formik.errors.Languages}
                  </span>
                )}
              </div>
            </div>
          </div>
          <div className="contact-info bg-white shadow-md p-4 rounded-md">
            <h3 className="text-[20px] font-light mb-4 flex items-center text-[25px] gap-2 ">
              {" "}
              <LuContact className="text-secondary" /> Contact Information
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
              <div>
                <label
                  className="text-[15px] my-2 font-light text-gray-800 block"
                  htmlFor=""
                >
                  Email Address :{" "}
                </label>
                <input
                  value={formik.values.Email}
                  onChange={formik.handleChange}
                  name="Email"
                  type="email"
                  placeholder="captain@gmail.com"
                  id=""
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                />
                {formik.touched.Email && formik.errors.Email && (
                  <span className="text-[14px] text-red-500">
                    {formik.errors.Email}
                  </span>
                )}
              </div>
              <div>
                <label
                  className="text-[15px] my-2 font-light text-gray-800 block"
                  htmlFor=""
                >
                  Phone Number :{" "}
                </label>
                <input
                  value={formik.values.PhoneNumber}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  name="PhoneNumber"
                  className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                  type="tel"
                  placeholder="0101010100"
                  id=""
                />
                {formik.touched.PhoneNumber && formik.errors.PhoneNumber && (
                  <span className="text-[14px] text-red-500">
                    {formik.errors.PhoneNumber}
                  </span>
                )}
              </div>
            </div>
            <textarea
              value={formik.values.Bio}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              name="Bio"
              placeholder="Details the captain sailing history, notable expreditions, and leadership..."
              cols="52"
              rows="10"
              className="my-5 border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            ></textarea>
            {formik.touched.Bio && formik.errors.Bio && (
              <span className="text-red-500 text-[14px]">
                {formik.errors.Bio}
              </span>
            )}
          </div>
        </div>
        <div className="flex flex-col gap-y-6">
          <div
            className="image w-4/5 mx-auto bg-background shadow-md"
            onClick={() => imageRef.current.click()}
          >
            <input
              ref={imageRef}
              onChange={handelimage}
              type="file"
              name="image"
              id="image"
              hidden
            />
            <h3 className="text-[23px] text-tertiary flex  items-center gap-x-2.5">
              <MdAddAPhoto /> Captain Photo
            </h3>
            {image ? (
              <img
                src={image}
                className="w-full h-[350px] object-cover rounded-md"
              />
            ) : (
              <div className="box cursor-pointer bg-gray-300 w-[90%] mx-auto rounded-md my-3 flex flex-col items-center">
                <span className="text-[100px] text-secondary">
                  <FaCloudUploadAlt />
                </span>
                <p className="text-gray-700 text-[18px] my-2">
                  Drag && Drop Image
                </p>
                <p className="text-gray-500 text-[14px] my-1">
                  Recommended 800x800px
                </p>
                {formik.touched.ProfilePhoto && formik.errors.ProfilePhoto && (
                  <span>{formik.errors.ProfilePhoto}</span>
                )}
              </div>
            )}
          </div>
          <div className="guideline w-4/5 mx-auto py-5 px-2 bg-primary rounded-md">
            <h4 className="text-background text-[25px] font-bold">GuideLine</h4>
            <p className="text-[16px] text-gray-300">
              Complete Profiles increase expeditions booking rates by 45%.
              Ensure high-quailty photo and details.{" "}
            </p>
          </div>
        </div>
      </div>
    </form>
  );
};

export default AddCaptain;
