import { BASE_API_URL } from "./apiConfig";
import { getToken } from "./LoginService";

const CARS_URL = `${BASE_API_URL}/Car`;

/**
 * 🛠️ Internal Helper: Creates the necessary headers, including the JWT token.
 * It's called for every API request below.
 * @param {object} options - Optional fetch configuration object.
 * @returns {object} The fetch configuration object with Authorization headers added.
 */

const authorizedFetch = (options = {}) => {
  const token = getToken(); // Get the token from localStorage via LoginService

  if (!token) {
    // If we're hitting a secured endpoint, and no token exists, throw
    // This will be caught by the calling component, allowing it to redirect to login
    throw new Error("Unauthorized access: Authentication token not found.");
  }

  // Define the base headers
  const authHeaders = {
    "Content-Type": "application/json",
    // 🔑 Inject the token here: "Authorization: Bearer [token]"
    Authorization: `Bearer ${token}`,
  };

  // Merge provided options with authorization headers
  const config = {
    ...options,
    headers: {
      ...authHeaders,
      ...options.headers, // Allow consumers to override headers if needed
    },
  };

  return config;
};

/**
 * Fetches the list of all cars from the ASP.NET Core API.
 * @returns {Promise<Array>} A promise that resolves to an array of car objects.
 */

export const getCars = async () => {
  try {
    // Use authorizedFetch to get the headers
    const config = authorizedFetch({ method: "GET" });
    const response = await fetch(CARS_URL, config);

    if (!response.ok) {
      if (response.status === 401 || response.status === 403) {
        throw new Error("Session expired or unauthorized. Please log in.");
      }
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    // The response body is the JSON array of cars from your API
    return await response.json();
  } catch (error) {
    // Log the error for debugging and re-throw to be handled by the component
    console.error("Failed to fetch cars:", error);
    throw error;
  }
};

export const addCar = async (data) => {
  try {
    const config = authorizedFetch({
      method: "POST",
      body: JSON.stringify(data),
    });

    const response = await fetch(CARS_URL, config);

    if (!response.ok) {
      if (response.status === 401 || response.status === 403) {
        throw new Error("Session expired or unauthorized. Please log in.");
      }
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const contentType = response.headers.get("content-type");

    if (contentType && contentType.includes("application/json")) {
      return await response.json();
    }

    return null;
  } catch (error) {
    // Log the error for debugging and re-throw to be handled by the component
    console.error("failed to add car:", error);
    throw error;
  }
};

export const updateCar = async (carId, carData) => {
  // Construct the URL to target the specific object
  const UPDATE_URL = `${CARS_URL}/${carId}`;

  try {
    const config = authorizedFetch({
      method: "PUT",
      body: JSON.stringify(carData),
    });

    const response = await fetch(UPDATE_URL, config);

    if (!response.ok) {
      if (response.status === 401 || response.status === 403) {
        throw new Error("Session expired or unauthorized. Please log in.");
      }
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const contentType = response.headers.get("content-type");

    if (contentType && contentType.includes("application/json")) {
      // If the API sends back JSON
      return await response.json();
    }

    return carData;
  } catch (error) {
    console.error(`Failed to update car ${carId}:`, error);
    throw error;
  }
};

export const deleteCar = async (carId) => {
  const DELETE_URL = `${CARS_URL}/${carId}`;

  try {
    const config = authorizedFetch({ method: "DELETE" });

    const response = await fetch(DELETE_URL, config);

    if (!response.ok) {
      if (response.status === 401 || response.status === 403) {
        throw new Error("Session expired or unauthorized. Please log in.");
      }
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return carId;
  } catch (error) {
    console.error(`Failed to delete car ${carId}:`, error);
    throw error;
  }
};
