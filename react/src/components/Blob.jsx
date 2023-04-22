import React, { useState, useEffect } from "react";
import "./Blob.css";

const Blob = () => {
  const [position, setPosition] = useState({ left: 0, top: 0 });

  const handlePointerMove = (event) => {
    const { clientX, clientY } = event;

    setPosition({
      left: clientX,
      top: clientY,
    });
  };

  useEffect(() => {
    window.addEventListener("pointermove", handlePointerMove);

    return () => {
      window.removeEventListener("pointermove", handlePointerMove);
    };
  }, []);

  const blobStyle = {
    left: position.left,
    top: position.top,
    position: "absolute",
    transition: "all 3s",
  };

  return (
    <>
      <div style={blobStyle} id="blob"></div>
      <div id="blur"></div>
    </>
  );
};

export default Blob;
