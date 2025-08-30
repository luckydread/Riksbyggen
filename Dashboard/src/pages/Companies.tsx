import { Building2 } from "lucide-react";
import { useEffect, useState } from "react";
import axios from "axios";
import { CompanyCard } from "@/components/companies/CompanyCard";
import { CreateCompanyDialog } from "@/components/companies/CreateCompanyDialog";

export default function Companies() {

  const [companies, setCompanies] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await axios.get(" https://localhost:7237/api/Companies");
        setCompanies(response.data);
      } catch (error) {
        console.error("Error fetching companies:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchCompanies();
  }, []);

  if (loading) {
    return <div className="text-center py-12">Laddar företag...</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <Building2 className="h-8 w-8 text-primary" />
          <div>
            <h1 className="text-3xl font-bold">Företag</h1>
            <p className="text-muted-foreground">
              Hantera företag och deras fastigheter
            </p>
          </div>
        </div>
        <CreateCompanyDialog />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {companies.map((company) => (
          <CompanyCard
            key={company.id}
            company={company}
            apartmentCount={company.apartments.length}
          />
        ))}
      </div>

      {companies.length === 0 && (
        <div className="text-center py-12">
          <Building2 className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
          <h3 className="text-lg font-medium text-muted-foreground mb-2">
            Inga företag ännu
          </h3>
          <p className="text-muted-foreground mb-4">
            Börja med att skapa ditt första företag.
          </p>
          <CreateCompanyDialog />
        </div>
      )}
    </div>
  );
}