// Need to move these to "Main Page"
// import Home from "./components/Home";
// import HomePage from "./components/HomePage";
// import NavBar from "./components/NavBar";

import MainPage from "./components/MainPage";
import SignUpForm from "./components/SignUpForm";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/signup" element={<SignUpForm />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
