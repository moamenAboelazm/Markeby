import React from "react";

const Btn = (props) => {
  return (
    <div className={`py-2 px-3 border-primary border w-fit rounded-md cursor-pointer hover:bg-transparent hover:text-tertiary bg-primary text-background transition-all duration-300 ease-in ${props.class}`} onClick={props.onClick}>
      {props.text}
    </div>
  );
};

export default Btn;
