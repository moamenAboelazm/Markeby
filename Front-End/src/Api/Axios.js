import axios from "axios";
import Cookies from "universal-cookie";
const cookie = new Cookies();
export const api = axios.create({
  baseURL: "https://markeby.runasp.net/api/",
});
api.interceptors.request.use((config) => {
  const token = cookie.get("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const refreshToken = cookie.get("reftoken");

        const res = await api.post("http://localhost:5000/api/auth/refresh", {
          refreshToken,
        });

        const newAccessToken = res.data.accessToken;

        cookie.set("accessToken", newAccessToken);

        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;

        return api(originalRequest);
      } catch (err) {
        cookie.remove("token");
        cookie.remove("reftoken");
        window.location.href = "/err404"; // Redirect to login page or any other page
      }
    }

    return Promise.reject(error);
  },
);
