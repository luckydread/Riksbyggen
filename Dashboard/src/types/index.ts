export interface Company {
  id: string;
  name: string;
  street: string;
}

export interface Apartment {
  id: string;
  street: string;
  city: string;
  zipCode: string;
  numberOfRooms: number;
  leaseExpiryDate: Date;
  status: string;
  companyId: string;
}