import { Home, MapPin, User, Phone, Calendar } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Apartment } from "@/types";

interface ApartmentCardProps {
  apartment: Apartment;
  companyId?: string;
}

export function ApartmentCard({ apartment, companyId }: ApartmentCardProps) {
  const isLeaseExpiringSoon = () => {
    const threeMonthsFromNow = new Date();
    threeMonthsFromNow.setMonth(threeMonthsFromNow.getMonth() + 3);
    return new Date(apartment.leaseExpiryDate) <= threeMonthsFromNow;
  };

  const leaseStatusBadge = () => {
    switch (apartment.status) {
      case "Busy":
        return <Badge variant="destructive">Busy</Badge>;
      case "Available":
        return <Badge variant="default">Available</Badge>;
      default:
        return <Badge variant="secondary">{apartment.status}</Badge>;
    }
  };

  const formattedLeaseDate = new Date(apartment.leaseExpiryDate).toLocaleDateString();

  return (
    <Card className="h-full transition-all duration-200 hover:shadow-lg hover:-translate-y-1">
      <CardHeader>
        <CardTitle className="flex items-center justify-between">
          <div className="flex items-center space-x-2">
            <Home className="h-5 w-5 text-primary" />
            <span className="truncate">{apartment.street}</span>
          </div>
          {leaseStatusBadge()}
        </CardTitle>
     
      </CardHeader>

      <CardContent className="space-y-3">
        <div className="grid grid-cols-2 gap-4 text-sm">
          <div>
            <span className="text-muted-foreground">Rooms:</span>
            <div className="font-medium">{apartment.numberOfRooms}</div>
          </div>
          <div>
            <span className="text-muted-foreground">Zip Code:</span>
            <div className="font-medium">{apartment.zipCode}</div>
          </div>
          <div className="col-span-2 flex items-center space-x-2 text-sm">
            <MapPin className="h-4 w-4 text-muted-foreground" />
            <span className="text-muted-foreground">
              {apartment.city}, {apartment.street}
            </span>
          </div>
        </div>

        <div className="space-y-2 pt-2 border-t">
          <div className="flex items-center space-x-2 text-sm">
            <Calendar className="h-4 w-4 text-muted-foreground" />
            <span className="text-muted-foreground">
              Lease expires: {formattedLeaseDate} {isLeaseExpiringSoon() && "⚠️"}
            </span>
          </div>
          {/* Tenant info placeholders */}
          <div className="flex items-center space-x-2 text-sm">
            <User className="h-4 w-4 text-muted-foreground" />
            <span className="text-muted-foreground">Tenant: No tenant</span>
          </div>
          <div className="flex items-center space-x-2 text-sm">
            <Phone className="h-4 w-4 text-muted-foreground" />
            <span className="text-muted-foreground">Phone: +46-xxx-xxx-xxx</span>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
