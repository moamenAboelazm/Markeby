export default function Switch  ({ enabled, formik })  {
  return (
    <button
         onClick={() => formik.setFieldValue("IsAvailable", !enabled)}
      className={`relative w-14 h-8 rounded-full transition mt-5 ${
        enabled===formik.values?.IsAvailable ? "bg-blue-500" : "bg-gray-300"
      }`}
    >
      <div
        className={`absolute top-1 left-1 w-6 h-6 bg-white rounded-full transition ${
          enabled ? "translate-x-6" : ""
        }`}
      />
    </button>
  );
};