import { Injectable } from '@angular/core';
import { UserLookupDto } from '@proxy/shared';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class TokenClaimsService {
  private readonly tokenKey = 'access_token'; // Key used to store the token in local storage

  getDecodedToken(): any {
    const token = localStorage.getItem(this.tokenKey);
    return token ? jwtDecode(token) : null;
  }

  getUserInfo(): UserLookupDto {
    const decodedToken = this.getDecodedToken();
    if (decodedToken) {
      return {
        id: decodedToken.sub,
        fullName: `${decodedToken.given_name}, ${decodedToken.family_name}`,
        email: decodedToken.email,
        phoneNumber: '',
        userName: decodedToken.preferred_username,
      } as UserLookupDto;
    }
    return {} as UserLookupDto;
  }
}
enum ApplicationRoles {
  SalesAdmin = 'SalesAdmin',
}
