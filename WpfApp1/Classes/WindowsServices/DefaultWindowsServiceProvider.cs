using Serilog;
using System.ServiceProcess;
using WpfApp1.Classes.WindowsServices;
using WpfApp1.Models;

namespace Service;

internal class DefaultWindowsServiceProvider : WindowsServiceProvider {
    public DefaultWindowsServiceProvider(ServiceControllerStatus[] statusesOfInterest, ILogger? logger) : base(statusesOfInterest, logger) {
    }

    internal override WindowsServiceModel[]? GetServices() {
        var allServices = ServiceController.GetServices();
        if (allServices == null)
            return null;

        var servicesState = new WindowsServiceModel[allServices.Length];
        var currentIndex = 0;

        foreach (var service in allServices)
            if(_statusesOfInterest.Contains(service.Status))
                servicesState[currentIndex++] = GetRelevantInformation(service);

        return servicesState[0..currentIndex];
    }

    private static WindowsServiceModel GetRelevantInformation(ServiceController service) => new(
        name: service.ServiceName,
        displayName: service.DisplayName,
        type: service.ServiceType.ToString(),
        status: service.Status.ToString(),
        startType: service.StartType.ToString());
}
