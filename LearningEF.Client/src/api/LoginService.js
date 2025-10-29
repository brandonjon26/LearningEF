import { BASE_API_URL } from "./apiConfig";

/**
 * Handles the login API call and stores the resulting JWT token.
 * @param {string} username - The user's username.
 * @param {string} password - The user's password.
 * @returns {string} The raw JWT token string.
 */
export const loginUser = async (username, password) => {
  const response = await fetch(`${BASE_API_URL}/user/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password }),
  });

  if (!response.ok) {
    // Throw an error that the component can catch (e.g., 401, 400)
    throw new Error("Invalid username or password.");
  }

  const data = await response.json();
  const token = data.token; // Retrieve the token from { token: "..." }

  if (token) {
    // Store the token immediately upon successful acquisition
    localStorage.setItem("authToken", token);
  } else {
    throw new Error("Login successful but no token received.");
  }

  return token;
};

/**
 * Retrieves the stored token.
 * @returns {string | null} The JWT token or null.
 */
export const getToken = () => {
  return localStorage.getItem("authToken");
};

/**
 * Removes the stored token to log the user out.
 */
export const logoutUser = () => {
  localStorage.removeItem("authToken");
};
