import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],

  server: {
    port: 3000, // This forces the client to use port 3000
    // host: true, // You might need this if localhost doesn't work well
  },
});
