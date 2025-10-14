import { BASE_API_URL } from "./apiConfig";

const CARS_URL = `${BASE_API_URL}/Car`;

/**
 * Fetches the list of all cars from the ASP.NET Core API.
 * @returns {Promise<Array>} A promise that resolves to an array of car objects.
 */

export const getCars = async () => {
  try {
    const response = await fetch(CARS_URL);

    if (!response.ok) {
      // Throw an error if the HTTP status code indicates failure (4xx or 5xx)
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
    const response = await fetch(CARS_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      // Throw an error if the HTTP status code indicates failure (4xx or 5xx)
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
    const response = await fetch(UPDATE_URL, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },

      body: JSON.stringify(carData),
    });

    if (!response.ok) {
      // Check for 4xx or 5xx errors
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
    const response = await fetch(DELETE_URL, {
      method: "DELETE",
    });

    if (!response.ok) {
      // Check for 4xx or 5xx errors
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return carId;
  } catch (error) {
    console.error(`Failed to delete car ${carId}:`, error);
    throw error;
  }
};
