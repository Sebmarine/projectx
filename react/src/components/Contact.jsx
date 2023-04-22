import React, { useState } from "react";
import { Formik, Form, Field } from "formik";
import { contactSchema } from "../schemas/contactSchema";
import toastr from "toastr";
import contactService from "../services/contactService";

const Contact = () => {
  const [initialValues] = useState({
    name: "",
    email: "",
    message: "",
  });

  const handleSubmit = (values) => {
    console.log(values);
    contactService
      .addMessage(values)
      .then(onMessageAddSuccess)
      .catch(onMessageAddError);
  };

  const onMessageAddSuccess = (response) => {
    console.log("success", response);
    toastr.success("I got your message!");
  };

  const onMessageAddError = (err) => {
    console.log("error", err);
    toastr.success("Message didn't send, try again");
  };

  return (
    <div
      name="contact"
      className="w-full h-screen bg-gradient-to-b from-black to-gray-800 p-4 text-white"
    >
      <div className="flex flex-col p-4 justify-center max-w-screen-lg mx-auto h-full">
        <div className="pb-8">
          <p className="text-4xl font-bold inline border-b-4 border-gray-500">
            Contact
          </p>
          <p className="py-4">Submit the form below to get in touch with me</p>
        </div>

        <div className="flex justify-center items-center">
          <Formik
            initialValues={initialValues}
            validationSchema={contactSchema}
            handleSubmit={handleSubmit}
            action="https://getform.io/f/d1292ea1-7dc9-44fd-be0d-d8226037658b"
            method="POST"
            // className="flex flex-col w-full md:w-1/2"
          >
            {({ errors, touched }) => (
              <Form className="flex flex-col w-full md:w-1/2">
                <Field
                  name="name"
                  placeholder="Enter your name"
                  className="p-2 bg-transparent border-2 rounded-md text-white focus:outline-none"
                />
                {errors.name && touched.name ? <div>{errors.name}</div> : null}
                <Field
                  name="email"
                  type="email"
                  placeholder="Enter your email"
                  className="my-4 p-2 bg-transparent border-2 rounded-md text-white focus:outline-none"
                />
                {errors.email && touched.email ? (
                  <div>{errors.email}</div>
                ) : null}
                <Field
                  name="message"
                  placeholder="Enter your message"
                  as="textarea"
                  rows="10"
                  className="p-2 bg-transparent border-2 rounded-md text-white focus:outline-none"
                />
                {errors.message && touched.message ? (
                  <div>{errors.message}</div>
                ) : null}

                <button
                  className="text-white bg-gradient-to-b from-cyan-500 to-blue-500 px-6 py-3 my-8 mx-auto flex items-center rounded-md hover:scale-110 duration-300"
                  type="submit"
                >
                  Submit
                </button>
              </Form>
            )}
            {/* <input
              type="text"
              name="name"
              placeholder="Enter your name"
              className="p-2 bg-transparent border-2 rounded-md text-white focus:outline-none"
            />
            <input
              type="text"
              name="email"
              placeholder="Enter your email"
              className="my-4 p-2 bg-transparent border-2 rounded-md text-white focus:outline-none"
            />
            <textarea
              name="message"
              placeholder="Enter your message"
              rows="10"
              className="p-2 bg-transparent border-2 rounded-md text-white focus:outline-none"
            ></textarea> */}
            {/* <button className="text-white bg-gradient-to-b from-cyan-500 to-blue-500 px-6 py-3 my-8 mx-auto flex items-center rounded-md hover:scale-110 duration-300">
              Let's Talk
            </button> */}
          </Formik>
        </div>
      </div>
    </div>
  );
};

export default Contact;
