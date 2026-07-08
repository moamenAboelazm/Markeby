import React, { useEffect } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import Cookies from "universal-cookie";

const GoBack = () => {
  const cookies = new Cookies();
  const token = cookies.get("token");
  const navigate = useNavigate();

  useEffect(() => {
    if (token) {
    window.history.back();
    }
  }, [token]);

  return !token ? <Outlet /> : null;
};

export default GoBack;