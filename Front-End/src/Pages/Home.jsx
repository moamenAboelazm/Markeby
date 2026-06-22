import React from "react";
import { Link } from "react-router-dom";
import { useRole } from "../Hooks/UseRole";

const Home = () => {
  const { token, role } = useRole();
  return (
    <div>
      <Link to="/register" className="text-blue-500 hover:underline">
        Go to Register Page
      </Link>
      <Link to="/login" className="text-blue-500 hover:underline ml-4">
        Go to Login Page
      </Link>
      {role === "Admin" ? (
        <div className="mt-4">
          <p className="text-green-500">You are logged in as an Admin.</p>
        </div>
      ) : (
        role
      )}
      <Link to="/dashboard" className="text-blue-500 hover:underline">
        Dashboard
      </Link>
    </div>
  );
};

export default Home;
