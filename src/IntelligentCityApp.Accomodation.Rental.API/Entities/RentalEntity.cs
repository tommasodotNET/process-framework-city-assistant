namespace IntelligentCityApp.Accomodation.Rental.API.Entities;

public enum VehicleType
{
    Car,
    Van,
    Motorcycle,
    Bus
}

public class RentalEntity
{
    public Guid Id { get; set; }
    public List<Vehicle> Vehicles { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }

    public bool RentVehicle(VehicleType vehicleType, int hours)
    {
        var vehicle = Vehicles.FirstOrDefault(v => v.VehicleType == vehicleType);
        if (vehicle != null && vehicle.AvailableUnits > 0)
        {
            vehicle.AvailableUnits--;
            return true;
        }
        return false;
    }

    public int GetAvailableUnits(VehicleType vehicleType)
    {
        var vehicle = Vehicles.FirstOrDefault(v => v.VehicleType == vehicleType);
        return vehicle?.AvailableUnits ?? 0;
    }

    public decimal GetCostPerHour(VehicleType vehicleType)
    {
        var vehicle = Vehicles.FirstOrDefault(v => v.VehicleType == vehicleType);
        return vehicle?.CostPerHour ?? 0;
    }
}

public class Vehicle
{
    public Guid Id { get; set; }
    public int AvailableUnits { get; set; }
    public decimal CostPerHour { get; set; }
    public VehicleType VehicleType { get; set; }
    public DateTime InsuranceExpiryDate { get; set; }
    public int EngineCapacity { get; set; }
}
public enum FuelType
{
    Gasoline,
    Diesel,
    Electric,
    Hybrid,
    HybridPlugin
}
