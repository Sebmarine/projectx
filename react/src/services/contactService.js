import axios from "axios";
import { onGlobalSuccess, onGlobalError } from "./serviceHelpers";

const addMessage = (payload) => {
  const config = {
    method: "POST",
    url: "https://getform.io/f/d1292ea1-7dc9-44fd-be0d-d8226037658b",
    data: payload,
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const contactService = {
  addMessage,
};

export default contactService;
