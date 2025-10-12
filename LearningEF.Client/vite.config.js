import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import path from "path";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],

  server: {
    port: 3000, // This forces the client to use port 3000
  },

  resolve: {
    alias: {
      // The alias '@' now points to the 'src' directory
      "@": path.resolve(__dirname, "./src"),
    },
  },
});
