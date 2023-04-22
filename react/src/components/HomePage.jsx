import React from "react";
import "./HomePage.css";

const HomePage = () => {
  return (
    <div className="home-page">
      <h1>Discover, Plan, and Create Unforgettable Events with FindUR</h1>
      <h2>
        FindUR simplifies event planning by connecting you with top-notch
        vendors, unique venues, and personalized services.
      </h2>
      <ul>
        <li>Easily browse and book local vendors for any event</li>
        <li>Compare prices and availability in one convenient location</li>
        <li>
          Save time and effort with our user-friendly event management tools
        </li>
        <li>Stay organized with customizable event checklists and calendars</li>
        <li>
          Collaborate with your team, friends, or family to plan the perfect
          event
        </li>
        <li>Access FindUR on any device: desktop, tablet, or mobile</li>
      </ul>
      <button>
        Ready to plan your next event? Sign up for FindUR today and experience
        the future of event planning!
      </button>
      <blockquote>
        FindUR made planning my wedding a breeze. I was able to find and book
        all my vendors in one place, and the event management tools kept me
        organized throughout the process. -Happy Customer
      </blockquote>
    </div>
  );
};

export default HomePage;
