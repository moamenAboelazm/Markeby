import { jwtDecode } from "jwt-decode";
import Cookies from "universal-cookie";
export const useRole = () => {
  const cookies = new Cookies();
  const token = cookies.get("token");
  const role = cookies.get("role");
  if (token) {
    const decodedToken = jwtDecode(token);

    const roleFromToken =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    const id =
      decodedToken[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ];
    const fullNameFromToken = decodedToken.fullName;
    cookies.set("role", roleFromToken);
    return { token, role: roleFromToken, name: fullNameFromToken,id };
  }
  return { token: null, role: null };
};
