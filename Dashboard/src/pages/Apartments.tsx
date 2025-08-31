import { useState, useEffect } from "react";
import { useSearchParams, useLocation } from "react-router-dom";
import { Home } from "lucide-react";
import { ApartmentCard } from "@/components/apartments/ApartmentCard";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import axios from "axios";
import { CreateApartmentDialog } from "@/components/apartments/CreateApartmentDialog";

export default function Apartments() {
  const [searchParams, setSearchParams] = useSearchParams();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const companyIdParam = queryParams.get("company");

  const [companies, setCompanies] = useState<{ id: number; name: string }[]>([]);
  const [apartments, setApartments] = useState<any[]>([]);
  const [selectedCompanyId, setSelectedCompanyId] = useState<string>("all");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch companies
  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const res = await axios.get("http://138.68.79.101:5000/api/Companys");
        setCompanies(res.data);
      } catch (err) {
        console.error("Error fetching companies:", err);
        setError("Kunde inte hämta företag");
      }
    };
    fetchCompanies();
  }, []);

  useEffect(() => {
    const fetchApartments = async () => {
      setLoading(true);
      try {
        let url = `http://138.68.79.101:5000/api/Companys/${companyIdParam}/apartments`;
        const res = await axios.get(url);
        setApartments(res.data);
      } catch (err) {
        console.error("Error fetching apartments:", err);
        setError("Kunde inte hämta lägenheter");
      } finally {
        setLoading(false);
      }
    };
    fetchApartments();
  }, [companyIdParam]);

  useEffect(() => {
    if (companyIdParam && companies.find(c => c.id.toString() === companyIdParam)) {
      setSelectedCompanyId(companyIdParam);
    } else {
      setSelectedCompanyId("all");
    }
  }, [companyIdParam, companies]);

  const handleCompanyChange = (value: string) => {
    setSelectedCompanyId(value);
    if (value === "all") {
      searchParams.delete("company");
    } else {
      searchParams.set("company", value);
    }
    setSearchParams(searchParams);
  };

  const filteredApartments = selectedCompanyId === "all"
    ? apartments
    : apartments.filter(apt => apt.companyId.toString() === selectedCompanyId);

  if (loading) {
    return <div className="p-4">Laddar lägenheter...</div>;
  }

  if (error) {
    return <div className="p-4 text-red-500">{error}</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <Home className="h-8 w-8 text-primary" />
          <div>
            <h1 className="text-3xl font-bold">Lägenheter</h1>
            <p className="text-muted-foreground">
              Hantera lägenheter och hyresgäster
            </p>
            <CreateApartmentDialog />
          </div>
        </div>
      </div>

      {/* Company filter */}
      <div className="flex items-center space-x-4">
        <label htmlFor="company-filter" className="text-sm font-medium">
          Filtrera på företag:
        </label>

        <Select value={selectedCompanyId} onValueChange={handleCompanyChange}>
          <SelectTrigger className="w-64">
            <SelectValue placeholder="Välj företag" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Alla företag</SelectItem>
            {companies.map((company) => (
              <SelectItem key={company.id} value={company.id.toString()}>
                {company.name}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {/* Apartments grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredApartments.map((apt) => (
          <ApartmentCard key={apt.id} apartment={apt} companyId={apt.companyId.toString()} />
        ))}
      </div>

      {filteredApartments.length === 0 && (
        <div className="text-center py-12">
          <Home className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
          <h3 className="text-lg font-medium text-muted-foreground mb-2">
            Inga lägenheter hittades
          </h3>
          <p className="text-muted-foreground">
            {selectedCompanyId === "all"
              ? "Det finns inga lägenheter registrerade ännu."
              : "Det valda företaget har inga lägenheter registrerade."}
          </p>
        </div>
      )}
    </div>
  );
}
