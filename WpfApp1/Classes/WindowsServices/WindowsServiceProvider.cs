using Serilog;
using System.Collections.ObjectModel;
using System.ServiceProcess;
using WpfApp1.Models;

namespace WpfApp1.Classes.WindowsServices;
internal abstract class WindowsServiceProvider {

    private ILogger? _logger;

    internal ReadOnlyCollection<ServiceControllerStatus> _statusesOfInterest;

    internal WindowsServiceProvider(ServiceControllerStatus[] statusesOfInterest, ILogger? logger) => (_statusesOfInterest, _logger) = (new(statusesOfInterest), logger);

    internal abstract WindowsServiceModel[]? GetServices();
}
