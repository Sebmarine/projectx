// AnimatedSebastian.js
import React, { useState, useEffect, useRef } from "react";

const AnimatedSebastian = () => {
  const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  const [text, setText] = useState("SEBASTIAN");
  const intervalRef = useRef(null); // Use useRef to store the interval value

  const handleMouseOver = () => {
    let iteration = 0;

    clearInterval(intervalRef.current);

    intervalRef.current = setInterval(() => {
      setText((currentText) =>
        currentText
          .split("")
          .map((letter, index) => {
            if (index < iteration) {
              return "SEBASTIAN"[index];
            }

            return letters[Math.floor(Math.random() * 26)];
          })
          .join("")
      );

      if (iteration >= "SEBASTIAN".length) {
        clearInterval(intervalRef.current);
      }

      iteration += 1 / 3;
    }, 30);
  };

  useEffect(() => {
    return () => {
      clearInterval(intervalRef.current);
    };
  }, []); // No dependencies required here

  return <span onMouseOver={handleMouseOver}>{text}</span>;
};

export default AnimatedSebastian;
