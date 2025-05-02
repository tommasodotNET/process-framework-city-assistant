using System;

namespace IntelligentCityApp.ProcessOrchestration.Events;

public static class IntelligentCityEvents
{
    public static readonly string NewRequestReceived = nameof(NewRequestReceived);
    public static readonly string InformationRetrieved = nameof(InformationRetrieved);
    public static readonly string RetrieveAccomodation = nameof(RetrieveAccomodation);
    public static readonly string RetrieveEvents = nameof(RetrieveEvents);
    public static readonly string FinalizeProcess = nameof(FinalizeProcess);
}
