export interface Message {
  id: number;
  body: string;
  senderId: number;
  receiverId: number;
  time: Date;
}

export interface CreateMessageDto {
  body: string;
  senderId: number;
  receiverId: number;
}

export interface EditMessageDto {
  id: number;
  body: string;
}
