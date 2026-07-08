import { useFormik } from "formik";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { FaEye } from "react-icons/fa";
import { FaEyeSlash } from "react-icons/fa";
import Cookies from "universal-cookie";
import Loader from "../../Components/Website/Loader";
import { jwtDecode } from "jwt-decode";
import { api } from "../../Api/Axios";
import { LOGIN } from "../../Api/Api";
import { LoginSchema } from "../../Schema/YUP";
const Login = () => {
  const [loading, setLoading] = useState(false);
  const nav = useNavigate();
  const cookies = new Cookies();

  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
    },
    validationSchema: LoginSchema,
    validateOnBlur: true,
    validateOnChange: true,
    onSubmit: async (values, formikHelpers) => {
      try {
        setLoading(true);
        const response = await api.post(LOGIN, {
          email: values.email,
          password: values.password,
        });
        cookies.set("token", response.data.token);
        const decodedToken = jwtDecode(response.data.token);
        const roleFromToken =
          decodedToken[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
          ];
        cookies.set("reftoken", response.data.refreshToken);
        if (roleFromToken === "Admin") {
          cookies.set("role", roleFromToken);
          nav("/dashboard/users");
        } else {
          nav("/");
        }
      } catch (error) {
        formikHelpers.setFieldError("password", "Invalid email or password");
      } finally {
        setLoading(false);
      }
    },
  });
  const [eyes, setEyes] = useState({
    eye1: false,
  });
  if (loading === true) {
    return <Loader />;
  }
  return (
    <div className="w-4/5 mx-auto">
      <div className="flex flex-col md:flex-row items-center justify-between min-h-screen">
        <div className="w-full md:w-1/2">
          <h2 className="text-[37px] text-center md:text-start lg:text-[50px] font-bold mb-4">
            Discover The Magic Beneath The Waves with{" "}
            <span className="text-secondary">Markby</span>.
          </h2>
          <p className="text-md mb-4">
            Experience the beauty of Hurghada’s Red Sea, where vibrant fish,
            stunning coral reefs, and endless blue waters await you.
          </p>
          <p className="text-md mb-4">
            Set sail with Markby and create unforgettable moments with family
            and friends.
          </p>
          <p className="text-md mb-4">Your Next Sea Adventure Starts Here.</p>

          <div className="flex flex-col md:flex-row items-center gap-4">
            <div className="flex flex-wrap justify-center -space-x-3">
              <img
                className="size-12 rounded-full object-cover"
                src="/assets/Bony.jpeg"
                alt="userImage1"
              />
              <img
                className="size-12 rounded-full"
                src="https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?q=80&w=200"
                alt="userImage2"
              />
              <img
                className="size-12 rounded-full"
                src="https://images.unsplash.com/photo-1438761681033-6461ffad8d80?q=80&w=200&h=200&auto=format&fit=crop"
                alt="userImage3"
              />
            </div>
            <p className="text-sm italic">+25,000 Adventures</p>
          </div>
        </div>
        <form
          onSubmit={formik.handleSubmit}
          className="w-full md:w-1/2 bg-white md:px-[50px] px-[20px] py-5 rounded-lg shadow-md mt-8 md:mt-3 lg:ml-5"
        >
          <div className="text-center mt-8">
            <h3 className="text-[40px]  font-bold mb-4">Welcome Back</h3>
            <p className="text-gray-600 mb-4">
              Login to manage your sea expeditions, track your bookings, and
              access exclusive offers.
            </p>
          </div>
          <div className="mb-4">
            <label htmlFor="email" className="block text-gray-700  mb-2">
              Email Address :
            </label>
            <input
              type="email"
              id="email"
              value={formik.values.email}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              name="email"
              placeholder="Enter your email address"
              className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
            />
            {formik.errors.email && formik.touched.email ? (
              <p className="text-red-500 text-sm mt-2">{formik.errors.email}</p>
            ) : null}
          </div>
          <div className="mb-4">
            <label htmlFor="password" className="block text-gray-700 mb-2">
              Password :
            </label>
            <div className="flex items-center gap-2">
              <input
                type={eyes.eye1 ? "text" : "password"}
                id="password"
                placeholder="Enter your password"
                className="border border-gray-400 rounded-[20px] py-2 px-3 focus:outline-none focus:ring-2 focus:border-secondary valid:ring-secondary w-full"
                value={formik.values.password}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                name="password"
              />
              {eyes.eye1 ? (
                <FaEye
                  className="text-tertiary cursor-pointer text-[25px]"
                  onClick={() => setEyes((prev) => ({ ...prev, eye1: false }))}
                />
              ) : (
                <FaEyeSlash
                  className="text-tertiary cursor-pointer text-[25px]"
                  onClick={() => setEyes((prev) => ({ ...prev, eye1: true }))}
                />
              )}
            </div>
            {formik.errors.password && formik.touched.password ? (
              <p className="text-red-500 text-sm mt-2">
                {formik.errors.password}
              </p>
            ) : null}
          </div>

          <button
            type="submit"
            className="bg-secondary hover:bg-transparent  text-white font-bold py-2 px-4 rounded-[20px] transition-colors w-full border-2 border-secondary hover:text-secondary"
          >
            Login
          </button>
          <p className="text-center text-gray-600 mt-4">
            I don't have an account?
            <Link to="/register" className="text-secondary hover:underline">
              Register here
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
};

export default Login;
