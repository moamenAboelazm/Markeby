import React, { useEffect, useState } from "react";
import { useRole } from "../../Hooks/UseRole";
import { api } from "../../Api/Axios";
import { Navigate, Outlet, useNavigate } from "react-router-dom";

const ProtectedRoutes = () => {
  const { token, role, id } = useRole();
  const [user, setUser] = useState("");
  const nav = useNavigate("");
  useEffect(() => {
    async function getCurrentUser() {
      try {
        const res = await api.get(`Users/${id}`);
        setUser(res.data.lastName);
      } catch (err) {
        nav("/");
      }
    }
    getCurrentUser();
  }, []);
  return token ? (
    role === "Admin" ? (
      user !== "" ? (
        <Outlet />
      ) : (
        ""
      )
    ) : (
      <Navigate to={"/login"} replace={true} />
    )
  ) : (
    <Navigate to={"/login"} replace={true} />
  );
};

export default ProtectedRoutes;
