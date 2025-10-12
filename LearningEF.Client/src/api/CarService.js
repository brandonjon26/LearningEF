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
