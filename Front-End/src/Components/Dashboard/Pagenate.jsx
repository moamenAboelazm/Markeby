import { useState } from "react";
import { Pagination } from "antd";

function Pagenate(props) {

  const onChange = (pageNumber) => {
    props.setNavgate((prev)=>{return {...prev,pageNumber:pageNumber}});
  };
  return (
    <Pagination
      current={+props.navgate.pageNumber}
      total={+props.navgate.totalCount}
pageSize={props.navgate.pageSize}      onChange={onChange}
    />
  );
}

export default Pagenate;