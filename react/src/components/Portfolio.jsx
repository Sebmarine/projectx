import React from "react";
import cardSplosion from "../assets/portfolio/cardSplosion.gif";
import findUR from "../assets/portfolio/findUR.gif";
import miVet from "../assets/portfolio/MiVet.gif";
// import reactParallax from "../assets/portfolio/reactParallax.jpg";
// import reactSmooth from "../assets/portfolio/reactSmooth.jpg";
// import reactWeather from "../assets/portfolio/reactWeather.jpg";

const Portfolio = () => {
  const portfolios = [
    {
      id: 1,
      src: cardSplosion,
      link: "https://github.com/Sebmarine/findur/blob/main/src/components/Cardsplosion.jsx",
    },
    {
      id: 2,
      src: miVet,
      link: "https://github.com/Sebmarine/MiVet",
    },
    {
      id: 3,
      src: findUR,
      link: "https://github.com/Sebmarine/findur",
    },
  ];

  return (
    <div
      name="portfolio"
      className="bg-gradient-to-b from-black to-gray-800 w-full text-white md:h-screen"
    >
      <div className="max-w-screen-lg p-4 mx-auto flex flex-col justify-center w-full h-full">
        <div className="pb-8">
          <p className="text-4xl font-bold inline border-b-4 border-gray-500">
            Portfolio
          </p>
          <p className="py-4">Check out some of my work right here</p>
        </div>

        <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-8 px-12 sm:px-0">
          {portfolios.map(({ id, src, link }) => (
            <div key={id} className="shadow-md shadow-gray-600 rounded-lg">
              <img
                src={src}
                alt=""
                className="rounded-md duration-200 hover:scale-105"
              />
              <div className="flex items-center justify-center">
                {/* <button className="w-1/2 border border-gray-500 rounded-md px-6 py-3 m-4 duration-200 hover:scale-105">
                  Demo
                </button> */}
                <button
                  onClick={() => window.open(link, "_blank")}
                  className="w-1/2 border border-cyan-500 rounded-md px-6 py-3 m-4 duration-200 hover:scale-105"
                >
                  Code
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Portfolio;
