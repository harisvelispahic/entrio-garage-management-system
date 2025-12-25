import { api, ApiError } from "./api";
import { LoginResponse } from "@/config/api";

export interface LoginCredentials {
  username: string;
  password: string;
}

export const authService = {
  async login(credentials: LoginCredentials): Promise<LoginResponse> {
    try {
      const response = await api.post<LoginResponse>("/auth/login", {
        Email: credentials.username,
        Pin: credentials.password,
      });

      return response;
    } catch (error) {
      if (error instanceof ApiError) {
        if (error.status === 401) {
          throw new Error("Invalid username or password");
        }
        throw new Error(error.message);
      }
      throw new Error("Network error. Please check your connection.");
    }
  },
};
