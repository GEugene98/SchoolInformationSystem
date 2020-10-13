
export class User {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  surName: string;
  roles: string[];
  fullName: string;
  activity: string[];
  role: string;
  email: string;
  position: string;

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
