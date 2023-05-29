import { useState } from "react";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export const useRequest = () => {
  const [data, setData] = useState([]);
  const [error, setError] = useState();

  async function handleRequest(promise) {
    try {
      setError(null);
      const response = await promise;
      setData(response.data);
    } catch (error) {
      console.log(error?.message)
      setError(error);
    }
  }

  return { handleRequest, data, error };
};
