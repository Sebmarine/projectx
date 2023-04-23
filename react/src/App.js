// import About from "./components/About";
// import Contact from "./components/Contact";
// import Experience from "./components/Experience";
// import Portfolio from "./components/Portfolio";
import Home from "./components/Home";
import HomePage from "./components/HomePage";
import NavBar from "./components/NavBar";

import SocialLinks from "./components/SocialLinks";

function App() {
  return (
    <div className="App">
      <NavBar />
      <Home />
      <HomePage />
      {/* <About />
      <Portfolio />
      <Experience />
      <Contact /> */}

      <SocialLinks />
    </div>
  );
}

export default App;
