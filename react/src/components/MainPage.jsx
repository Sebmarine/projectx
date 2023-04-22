// components/MainPage.js
import React from "react";
import About from "./About";
import Contact from "./Contact";
import Experience from "./Experience";
import Home from "./Home";
import Portfolio from "./Portfolio";
import SocialLinks from "./SocialLinks";
import NavBar from "./NavBar";

const MainPage = () => {
  return (
    <>
      <NavBar />
      <Home/>
      <About />
      <Portfolio />
      <Experience />
      <Contact />
      <SocialLinks />
    </>
  );
};

export default MainPage;
