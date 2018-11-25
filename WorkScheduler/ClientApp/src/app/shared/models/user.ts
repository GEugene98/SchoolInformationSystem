
export class User {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  surName: string;
  roles: string[];
  fullName: string;
}

export function isUserInRole(user: User, role: string) {
  if (user.roles.find(r => r == role)) {
    return true;
  }
  return false;
}
