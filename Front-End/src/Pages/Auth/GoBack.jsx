import React from "react";
import { Outlet } from "react-router-dom";
import Cookies from "universal-cookie";

const GoBack = () => {
  const cookies = new Cookies();
  const token = cookies.get("token");
  return token ? window.history.back() : <Outlet />;
};

export default GoBack;
