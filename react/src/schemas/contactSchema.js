import * as Yup from "yup";

const contactSchema = Yup.object().shape({
  name: Yup.string()
    .min(2, "Too Short!")
    .max(50, "Too Long ;")
    .required("I want to know your name"),
  email: Yup.string().email("Invalid email").required("I need an email please"),
  message: Yup.string().min(2, "Too Short!").required("Required"),
});

export { contactSchema };
