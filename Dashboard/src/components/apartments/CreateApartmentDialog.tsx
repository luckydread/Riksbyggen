import { useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useToast } from "@/hooks/use-toast";
import axios from "axios";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

export function CreateApartmentDialog() {
  const [open, setOpen] = useState(false);
  const { toast } = useToast();

  const [formData, setFormData] = useState({
    street: "",
    city: "",
    zipCode: "",
    numberOfRooms: 0,
    status: "",
    companyId: 0,
    leaseExpiryDate: new Date().toISOString(),
  });

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({
      ...prev,
      [field]:
        field === "companyId" || field === "numberOfRooms"
          ? Number(value)
          : field === "leaseExpiryDate"
            ? new Date(value).toISOString()
            : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await axios.post("http://138.68.79.101:5000/api/Apartment", formData, {
        headers: { "Content-Type": "application/json" },
      });

      toast({
        title: "Lägenhet skapad",
        description: `Lägenheten har lagts till i systemet.`,
      });

      // Reset form and close modal
      setFormData({
        street: "",
        city: "",
        zipCode: "",
        numberOfRooms: 0,
        status: "",
        companyId: 0,
        leaseExpiryDate: new Date().toISOString(),
      });

      setOpen(false);
    } catch (err) {
      console.error("Error creating apartment:", err);
      toast({
        title: "Fel",
        description: "Kunde inte skapa lägenheten. Försök igen.",
        variant: "destructive",
      });
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button className="flex items-center space-x-2">
          <Plus className="h-4 w-4" />
          <span>Ny Lägenhet</span>
        </Button>
      </DialogTrigger>

      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Skapa Lägenhet</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Company ID */}
          <div className="space-y-2">
            <Label htmlFor="companyId">Company ID</Label>
            <Input
              id="companyId"
              type="number"
              value={formData.companyId}
              onChange={(e) => handleInputChange("companyId", e.target.value)}
              required
            />
          </div>

          {/* Lease Expiry Date */}
          <div className="space-y-2">
            <Label htmlFor="leaseExpiryDate">Lease Expiry Date</Label>
            <Input
              id="leaseExpiryDate"
              type="date"
              value={formData.leaseExpiryDate.split("T")[0]}
              onChange={(e) => handleInputChange("leaseExpiryDate", e.target.value)}
              required
            />
          </div>

          {/* Street */}
          <div className="space-y-2">
            <Label htmlFor="street">Street</Label>
            <Input
              id="street"
              value={formData.street}
              onChange={(e) => handleInputChange("street", e.target.value)}
              required
            />
          </div>

          {/* City */}
          <div className="space-y-2">
            <Label htmlFor="city">City</Label>
            <Input
              id="city"
              value={formData.city}
              onChange={(e) => handleInputChange("city", e.target.value)}
              required
            />
          </div>

          {/* Zip Code */}
          <div className="space-y-2">
            <Label htmlFor="zipCode">Zip Code</Label>
            <Input
              id="zipCode"
              value={formData.zipCode}
              onChange={(e) => handleInputChange("zipCode", e.target.value)}
              required
            />
          </div>

          {/* Status */}
          <div className="space-y-2">
            <Label htmlFor="status">Status</Label>
            <Input
              id="status"
              value={formData.status}
              onChange={(e) => handleInputChange("status", e.target.value)}
              required
            />
          </div>

          {/* Number of Rooms */}
          <div className="space-y-2">
            <Label htmlFor="numberOfRooms">Number of Rooms</Label>
            <Input
              id="numberOfRooms"
              type="number"
              value={formData.numberOfRooms}
              onChange={(e) => handleInputChange("numberOfRooms", e.target.value)}
              required
            />
          </div>

          {/* Buttons */}
          <div className="flex justify-end space-x-2 pt-4">
            <Button type="button" variant="outline" onClick={() => setOpen(false)}>
              Avbryt
            </Button>
            <Button type="submit">Skapa Lägenhet</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
