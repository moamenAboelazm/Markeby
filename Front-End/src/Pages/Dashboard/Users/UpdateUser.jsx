import React, { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Btn from "../../../Components/Utils/Btn";
import { useFormik } from "formik";
import { api } from "../../../Api/Axios";
import * as Yup from "yup";
import { IMGES_URL } from "../../../Api/Api";
import { Fa0, FaCamera, FaUpload, FaUser } from "react-icons/fa6";
const UpdateUser = () => {
  const { id } = useParams();
  const nav = useNavigate();
  const imageRef = useRef(null);
  const [role, setRole] = useState("");
  const validationSchema = Yup.object({
    FirstName: Yup.string()
      .required("First name is required")
      .min(2, "First name must be at least 2 characters")
      .max(30, "First name must be less than 50 characters"),

    LastName: Yup.string()
      .required("Last name is required")
      .min(2, "Last name must be at least 2 characters")
      .max(50, "Last name must be less than 50 characters"),

    BirthDate: Yup.date().required("Birth date is required"),

    Gender: Yup.string()
      .oneOf(["Male", "Female"], "Please select a valid gender")
      .required("Gender is required"),

    Nationality: Yup.string()
      .oneOf(
        ["Egyptian", "Saudi", "Moroccan", "Tunisian", "Libyan", "Another"],
        "Please select a valid nationality",
      )
      .required("Nationality is required"),

    PhoneNumber: Yup.string()
      .required("Phone number is required")
      .matches(/^[0-9]{11}$/, "Phone number must be 11 digits"),
    Address: Yup.string()
      .required("Address is required")
      .min(5, "Address must be at least 5 characters")
      .max(200, "Address must be less than 200 characters"),
  });
  const formik = useFormik({
    initialValues: {
      FirstName: "",
      LastName: "",
      BirthDate: "",
      Gender: "",
      Nationality: "",
      PhoneNumber: "",
      Address: "",
      ProfileImgUrl: "",
    },
    validationSchema: validationSchema,
    validateOnBlur: true,
    validateOnChange: true,
    onSubmit: async (values) => {
      try {
        const formData = new FormData();
        formData.append("FirstName", values.FirstName);
        formData.append("LastName", values.LastName);
        formData.append("BirthDate", values.BirthDate);
        formData.append("Gender", values.Gender);
        formData.append("Nationality", values.Nationality);
        formData.append("PhoneNumber", values.PhoneNumber);
        formData.append("Address", values.Address);

        if (values.ProfileImgUrl instanceof File) {
          formData.append("ProfileImgUrl", values.ProfileImgUrl);
        }

        const res = await api.put(`/Users/profile`, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
        nav("/dashboard/users");
      } catch (error) {
        console.error(error);
      }
    },
  });
  useEffect(() => {
    async function getUser() {
      try {
        const res = await api.get(`Users/${id}`);
        formik.setValues({
          FirstName: res.data.firstName,
          LastName: res.data.lastName,
          BirthDate: res.data.birthDate || "",
          Gender: res.data.gender || "",
          Nationality: res.data.nationality || "",
          PhoneNumber: res.data.phoneNumber || "",
          Address: res.data.address || "",
          ProfileImgUrl: res.data.profileImgUrl || "",
        });
        setRole(res.data.roles[0] || "");
      } catch (error) {
        if (error.response && error.response.status === 404) {
          nav("/err404");
        } else {
          console.error("Error fetching user data");
        }
      }
    }
    getUser();
  }, [id]);
  async function handelRoleChange(e) {
    try {
      const res = await api.put("/Users/role", {
        userId: id,
        targetRole: e.target.value,
      });
      setRole(e.target.value);
    } catch (err) {
      console.log(err);
    }
  }
  return (
    <form onSubmit={formik.handleSubmit}>
      <div className="top flex justify-between items-center">
        <div>
          <h2 className="text-[50px] text-primary font-bold">Update User</h2>
          <p className="text-[16px] text-gray-600 mt-2">
            Update user information and manage their account settings.
          </p>
        </div>
        <button type="submit">
          <Btn text={"Save Changes"} className={"mr-12"} />
        </button>
      </div>
      <div className="image w-full relative rounded-xl bg-primary h-[100px] my-[30px]">
        {formik.values.ProfileImgUrl ? (
          <img
            src={
              formik.values.ProfileImgUrl instanceof File
                ? URL.createObjectURL(formik.values.ProfileImgUrl)
                : `${IMGES_URL}${formik.values.ProfileImgUrl}`
            }
            alt="Profile"
            className="w-[100px] h-[100px] object-cover rounded-full absolute bottom-[-50px] left-[30px]"
          />
        ) : (
          <img
            src="https://cdn-icons-png.flaticon.com/512/149/149071.png"
            alt="Default Profile"
            className="w-[100px] h-[100px] rounded-full absolute bottom-[-50px] left-[30px]"
          />
        )}

        <div
          className="absolute bottom-[-50px] left-[100px] bg-white rounded-full p-2 cursor-pointer shadow"
          onClick={() => imageRef.current?.click()}
        >
          <FaCamera className="text-xl text-primary" />
        </div>

        <input
          ref={imageRef}
          type="file"
          accept="image/*"
          className="hidden"
          onChange={(e) => {
            const file = e.target.files[0];
            if (file) {
              formik.setFieldValue("ProfileImgUrl", file);
            }

            // علشان لو اختار نفس الصورة تاني يشتغل
            e.target.value = "";
          }}
        />
      </div>
      <div className="user-info mt-[90px] bg-white border border-gray-100 rounded-md shadow-md px-5 py-6">
        <h3 className="text-[30px] text-secondary font-semibold mb-4 flex items-center gap-x-2">
          <FaUser /> User Information
        </h3>
        <div className="user-details mt-10 grid grid-cols-1 md:grid-cols-2 gap-x-10 gap-y-4">
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              first Name
            </label>
            <input
              type="text"
              name="FirstName"
              value={formik.values.FirstName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            />
            {formik.touched.FirstName && formik.errors.FirstName && (
              <span className="text-[14px] text-red-600 my-2">
                {formik.errors.FirstName}
              </span>
            )}
          </div>
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              Last Name
            </label>
            <input
              type="text"
              name="LastName"
              value={formik.values.LastName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            />
            {formik.touched.LastName && formik.errors.LastName && (
              <span className="text-[14px] text-red-600 my-2">
                {formik.errors.LastName}
              </span>
            )}
          </div>
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              Gender
            </label>
            <select
              value={formik.values.Gender}
              name="Gender"
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            >
              <option value="">Select Gender</option>
              {["Male", "Female"].map((gen, key) => {
                return (
                  <option key={key} value={gen}>
                    {gen}
                  </option>
                );
              })}
            </select>
            {formik.touched.Gender && formik.errors.Gender && (
              <span className="text-[14px] text-red-600 my-2">
                {formik.errors.Gender}
              </span>
            )}
          </div>
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              Birth Date
            </label>
            <input
              type="date"
              name="BirthDate"
              value={formik.values.BirthDate}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            />
            {formik.touched.BirthDate && formik.errors.BirthDate && (
              <span className="text-[14px] text-red-600 my-2">
                {formik.errors.BirthDate}
              </span>
            )}
          </div>
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              Nationality
            </label>
            <select
              value={formik.values.Nationality}
              name="Nationality"
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            >
              <option value="">Select Nationality</option>

              {[
                "Egyptian",
                "Saudi",
                "Moroccan",
                "Tunisian",
                "Libyan",
                "Another",
              ].map((n, key) => {
                return (
                  <option key={key} value={n}>
                    {n}
                  </option>
                );
              })}
            </select>
            {formik.touched.Nationality && formik.errors.Nationality && (
              <span className="text-[14px] text-red-600 my-2">
                {formik.errors.Nationality}
              </span>
            )}
          </div>
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              Role
            </label>
            <select
              value={role}
              onChange={handelRoleChange}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            >
              {["Admin", "User"].map((role, key) => {
                return (
                  <option key={key} value={role}>
                    {role}
                  </option>
                );
              })}
            </select>
          </div>
          <div>
            <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
              Phone Number
            </label>
            <input
              type="tel"
              name="PhoneNumber"
              value={formik.values.PhoneNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            />
            {formik.touched.PhoneNumber && formik.errors.PhoneNumber && (
              <span className="text-[14px] text-red-600 my-2">
                {formik.errors.PhoneNumber}
              </span>
            )}
          </div>
        </div>
        <div>
          <label className="text-[17px] uppercase my-2 font-light text-gray-800 block">
            Address
          </label>
          <textarea
            type="text"
            name="Address"
            rows={5}
            value={formik.values.Address}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
          />
          {formik.touched.Address && formik.errors.Address && (
            <span className="text-[14px] text-red-600 my-2">
              {formik.errors.Address}
            </span>
          )}
        </div>
      </div>
    </form>
  );
};

export default UpdateUser;
