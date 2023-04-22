import axios from "axios";
axios.defaults.withCredentials = true;

/**
 * Will unpack the response body from reponse object
 * @param {*} response
 *
 */
const onGlobalSuccess = (response) => {
  /// Should not use if you need access to anything other than the data
  return response.data;
};

const onGlobalError = (err) => {
  return Promise.reject(err);
};

const API_HOST_PREFIX = process.env.REACT_APP_API_HOST_PREFIX;
const API_NODE_HOST_PREFIX = process.env.REACT_APP_API_NODE_HOST_PREFIX;

export {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX,
  API_NODE_HOST_PREFIX,
};
