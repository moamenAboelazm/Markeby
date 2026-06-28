export const REGISTER = "/Authentication/create";
export const LOGIN = "/Authentication/login";
export const CAPTAINS_PAGED = "/Captains/paged";
export const BOATS = "/Boats"
export const ALL_BOATS = "/Boats/all";
export function CAPTAINS(pageNumber, pageSize) {
  return `/${CAPTAINS_PAGED}?PageNumber=${pageNumber}&PageSize=${pageSize}`;
}
export function CAPTAIN(id) {
  return `/Captains/${id}`;
}
export function BOAT(id) {
  return `/Boats/${id}`;
}
