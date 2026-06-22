using Microsoft.Maui.Devices;

namespace MobileApp.Services;

public static class ApiConfig
{
    // The base URL of the ASP.NET Core backend API
    public static string BaseUrl
    {
        get
        {
            // Android emulator maps 10.0.2.2 to the host loopback (localhost)
            if (DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DevicePlatform.Android)
            {
                return "http://10.0.2.2:5263/";
            }
            
            // Windows, iOS Simulator, etc. can use localhost directly
            return "http://localhost:5263/";
        }
    }
}
