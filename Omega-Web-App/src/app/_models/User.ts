export class User {
  id: string;
  userName: string;
  email: string;
  password: string;
  isAdmin: boolean;
  role: string;

  getUserRole(): void {
    if (this.isAdmin) {
      this.role = 'مدیر';
    } else {
      this.role = 'کاربر';
    }
  }
}
