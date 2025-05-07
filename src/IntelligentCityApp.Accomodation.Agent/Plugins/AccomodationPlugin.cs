using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace IntelligentCityApp.Accomodation.Agent.Plugins;

[Description("Plugin to retrieve accomodations.")]
public class AccomodationPlugin
{
    private readonly HotelAPIHttpClient _hotelApiClient;
    private readonly RentalAPIHttpClient _rentalApiClient;
    private readonly ParkingAPIHttpClient _parkingApiClient;

    public AccomodationPlugin([FromServices]HotelAPIHttpClient hotelApiClient, [FromServices]RentalAPIHttpClient rentalApiClient, [FromServices]ParkingAPIHttpClient parkingApiClient)
    {
        _hotelApiClient = hotelApiClient;
        _rentalApiClient = rentalApiClient;
        _parkingApiClient = parkingApiClient;
    }

    [KernelFunction("GetHotels")]
    [Description("Get all hotels.")]
    public async Task<string> GetAccomodationsAsync()
    {
        return await _hotelApiClient.GetHotelsAsync();
    }

    [KernelFunction("GetRentals")]
    [Description("Get all rentals.")]
    public async Task<string> GetRentalsAsync()
    {
        return await _rentalApiClient.GetRentalsAsync();
    }

    [KernelFunction("GetParkings")]
    [Description("Get all parkings.")]
    public async Task<string> GetParkingsAsync()
    {
        return await _parkingApiClient.GetParkingAsync();
    }
}
