export interface User {
  id: number;
  username: string;
  loggedOn: string;
  profilePicturePath: string;
}

export interface RequestUserDTO {
  id: number;
  username: string;
  time: string;
  profilePicturePath: string;
}
