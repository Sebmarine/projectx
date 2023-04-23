import React, { useState } from 'react';
import "./SignUpForm.css";

const SignUpForm = () => {
  const [formType, setFormType] = useState('user');

  const handleButtonClick = (type) => {
    setFormType(type);
  };

  return (
    <div
    name="parentContainer"
    >
    <div
      name="secondDiv"
    >
      <h1 name="signUpLabel">Sign Up Form</h1>
      <button onClick={() => handleButtonClick('user')}>User</button>
      <button onClick={() => handleButtonClick('vendor')}>Vendor</button>

      <form>
        <div>
          <label htmlFor="name">Name:</label>
          <input type="text" id="name" />
        </div>
        <div>
          <label htmlFor="email">Email:</label>
          <input type="email" id="email" />
        </div>

        {formType === 'vendor' && (
          <>
            <div>
              <label htmlFor="company">Company:</label>
              <input type="text" id="company" />
            </div>
            <div>
              <label htmlFor="website">Website:</label>
              <input type="url" id="website" />
            </div>
            <div>
              <label htmlFor="phone">Phone:</label>
              <input type="tel" id="phone" />
            </div>
            <div>
              <label htmlFor="address">Address:</label>
              <input type="text" id="address" />
            </div>
          </>
        )}

        <button type="submit">Submit</button>
      </form>
    </div>
    </div>
  );
};

export default SignUpForm;
