import { Building2, Phone, Mail, MapPin } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Company } from "@/types";

interface CompanyCardProps {
  company: Company;
  apartmentCount?: number;
}

export function CompanyCard({ company, apartmentCount = 0 }: CompanyCardProps) {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/apartments?company=${company.id}`);
  };

  return (
    <Card
      className="h-full transition-all duration-200 hover:shadow-lg hover:-translate-y-1 cursor-pointer"
      onClick={handleClick}
    >
      <CardHeader>
        <CardTitle className="flex items-center space-x-2">
          <Building2 className="h-5 w-5 text-primary" />
          <span className="truncate">{company.name}</span>
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-3">
        <div className="flex items-start space-x-2 text-sm">
          <MapPin className="h-4 w-4 text-muted-foreground mt-0.5 flex-shrink-0" />
          <span className="text-muted-foreground">{company.street}</span>
        </div>

        <div className="pt-2 border-t">
          <div className="flex justify-between items-center text-sm">
            <span className="text-muted-foreground">Lägenheter:</span>
            <span className="font-medium text-primary">{apartmentCount}</span>
          </div>

        </div>
      </CardContent>
    </Card>
  );
}