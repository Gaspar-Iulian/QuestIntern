import { AuthUser } from './auth-user';

export interface AuthResponse {
  user: AuthUser;
  message: string;
}
