export class Man {
  fullName: string;
  position: string;
}

export class User extends Man {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  surName: string;
  roles: string[];
  activity: string[];
  role: string;
  email: string;

  canAccept: boolean; 
  canConfirm: boolean; 
  canUseChecklists: boolean; 
  canSeeAllChecklists: boolean; 
  canSeeAllProtocols: boolean; 
  canSeeAllSchedules: boolean;  
}

export function isUserInRole(user: User, role: string) {
  if (!user) {
    return false;
  }
  if (user.roles.find(r => r == role)) {
    return true;
  }
  return false;
}
