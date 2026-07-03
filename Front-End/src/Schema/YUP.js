import * as Yup from "yup";
export const RgisterSchema = Yup.object().shape({
  firstname: Yup.string()
    .min(2, "First Name must be at least 2 characters")
    .max(100, "First Name must be less than 100 characters")
    .required("First Name is required"),
  lastname: Yup.string()
    .min(2, "Last Name must be at least 2 characters")
    .max(100, "Last Name must be less than 100 characters")
    .required("Last Name is required"),
  email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters")
    .required("Password is required"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("password"), null], "Passwords must match")
    .required("Confirm Password is required"),
});
export const LoginSchema = Yup.object().shape({
  email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters")
    .required("Password is required"),
});
export const AddCaptainSchema = Yup.object().shape({
  FullName: Yup.string()
    .min(2, "Full Name must be at least 2 characters")
    .max(100, "Full Name must be less than 100 characters")
    .required("Full Name is required"),
  Email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  PhoneNumber: Yup.string()
    .required("Please Enter A Phone Number To Call You")
    .required("Phone Number is Required")
    .max(11, "Phone number Must have 11 number"),
  YearsOfExperience: Yup.number().required(
    "Please Enter A Years Of Experience",
  ),
  Languages: Yup.string().required("Please Enter A Languages"),
  ProfilePhoto: Yup.mixed().required("Photo is required"),
  Bio: Yup.string(),
  Rank: Yup.string().required("Captain must Have A Rank"),
});
export const EditiCaptainSchema = Yup.object().shape({
  FullName: Yup.string()
    .min(2, "Full Name must be at least 2 characters")
    .max(100, "Full Name must be less than 100 characters"),

  Email: Yup.string().email("Invalid email address"),

  PhoneNumber: Yup.string().max(11, "Phone number Must have 11 number"),
  YearsOfExperience: Yup.number(),
  Languages: Yup.string(),
  ProfilePhoto: Yup.mixed(),
  Bio: Yup.string(),
  Rank: Yup.string(),
});
export const AddShipSchema = Yup.object({
  Name: Yup.string()
    .min(3, "Name must be at least 3 characters")
    .max(50, "Name is too long")
    .required("Name of vessel is required"),

  Description: Yup.string()
    .min(10, "Description must be at least 10 characters")
    .required("Description is required"),

  Capacity: Yup.number()
    .typeError("Capacity must be a number")
    .positive("Capacity must be greater than 0")
    .integer("Capacity must be integer")
    .required("Capacity is required"),

  YearBuilt: Yup.number()
    .typeError("Year Built must be a number")
    .min(1900, "Year is too old")
    .max(new Date().getFullYear(), "Invalid year")
    .required("Year Built is required"),

  MaxSpeed: Yup.number()
    .typeError("Max Speed must be a number")
    .positive("Speed must be greater than 0")
    .required("Max Speed is required"),

  HasWifi: Yup.boolean(),

  HasFoodFacility: Yup.boolean(),
});
export const EditiShipSchema = Yup.object({
  Name: Yup.string()
    .min(3, "Name must be at least 3 characters")
    .max(50, "Name is too long")
    .required("Name of vessel is required"),

  Description: Yup.string()
    .min(10, "Description must be at least 10 characters")
    .required("Description is required"),

  Status: Yup.string()
    .oneOf(["Available", "AtSea", "OutOfService"])
    .required("Type of Boat is Required"),

  YearBuilt: Yup.number()
    .typeError("Year Built must be a number")
    .min(1900, "Year is too old")
    .max(new Date().getFullYear(), "Invalid year")
    .required("Year Built is required"),

  MaxSpeed: Yup.number()
    .typeError("Max Speed must be a number")
    .positive("Speed must be greater than 0")
    .required("Max Speed is required"),

  HasWifi: Yup.boolean(),

  HasFoodFacility: Yup.boolean(),
});

export const AddTripSchema = Yup.object({
  Titel: Yup.string()
    .min(5, "Trip title too short")
    .required("Trip title is required"),

  Description: Yup.string()
    .min(20, "Description must be at least 20 chars")
    .required("Description is required"),

  Type: Yup.string().required("Trip type is required"),

  StartLocation: Yup.string().required("Start location is required"),

  StartTime: Yup.date().required("Start time is required"),

  EndTime: Yup.date()
    .required("End time is required")
    .min(Yup.ref("StartTime"), "End time must be after start time"),

  BoatId: Yup.string().required("Vessel is required"),

  CaptainId: Yup.string().required("Captain is required"),

  Price: Yup.number()
    .typeError("Price must be number")
    .positive("Price must be positive")
    .required("Price is required"),


  AvailableSeats: Yup.number()
    .typeError("Seats must be number")
    .min(1)
    .required("Available seats required"),

  Images: Yup.array().min(1, "At least one image required"),
});