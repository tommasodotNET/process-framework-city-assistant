using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace IntelligentCityApp.Events.Agent.Plugins;

[Description("Plugin to retrieve events.")]
public class EventPlugin
{
    private readonly FestivalAPIHttpClient _festivalApiClient;
    private readonly CinemaAPIHttpClient _cinemaApiClient;

    public EventPlugin([FromServices]FestivalAPIHttpClient festivalApiClient, [FromServices]CinemaAPIHttpClient cinemaApiClient)
    {
        _festivalApiClient = festivalApiClient;
        _cinemaApiClient = cinemaApiClient;
    }

    [KernelFunction("GetFestivals")]
    [Description("Get all festivals.")]
    public async Task<string> GetFestivalsAsync()
    {
        return await _festivalApiClient.GetFestivalAsync();
    }

    [KernelFunction("GetCinemas")]
    [Description("Get all cinemas.")]
    public async Task<string> GetCinemasAsync()
    {
        return await _cinemaApiClient.GetCinemaAsync();
    }
}
