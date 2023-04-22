import React from "react";

const About = () => {
  return (
    <div
      name="about"
      className="w-full min-h-screen bg-gradient-to-b from-gray-800 to-black text-white"
    >
      <div className="max-w-screen-lg p-4 mx-auto flex flex-col justify-center w-full h-full">
        <div className="">
          <p className="text-4xl font-bold inline border-b-4 border-gray-500">
            About
          </p>
        </div>
        <p className="text-xl mt-5">
          Hello! I'm Sebastian Lorenzo Hernandez, a passionate Full Stack
          Software Engineer with a diverse background in Front-End and Back-End
          development, as well as Database Management. After serving in the Army
          and spending 11 years in various industries, including engineering, I
          pursued my dream of working in the tech industry. With the unwavering
          support of my family, I embarked on this journey and honed my skills
          in software engineering at MiVet, where I gained invaluable experience
          developing web applications using React, JavaScript, .NET MVC, and
          SQL.
        </p>
        <br />
        <p className="text-xl">
          My technical expertise includes Front-End technologies such as React,
          JavaScript, HTML5, CSS3, Redux, and Bootstrap, as well as Middle-Tier
          technologies like C#, .Net, .Net Core, and ASP.Net. I am also
          proficient in Database Management with SQL, TSQL, and Microsoft SQL
          Server Management Studio. My experience in the Army and the
          engineering industry has taught me the importance of adaptability,
          problem-solving, and collaboration, which I believe are essential for
          success in the tech industry. I am excited to explore new
          opportunities in software engineering and would love to connect with
          you!
        </p>
      </div>
    </div>
  );
};

export default About;
