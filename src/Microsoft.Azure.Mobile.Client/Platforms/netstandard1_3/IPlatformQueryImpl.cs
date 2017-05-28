using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.WindowsAzure.MobileServices
{
    /// <summary>
    /// These three methods should be implemented
    /// by the target platform. See PlatformInformation and MobileServiceHttpClient.GetUserAgentHeader()
    /// for insight.
    /// </summary>
    public interface IPlatformQueryImpl
    {
        string GetOperatingSystemVersion();
        string GetOperatingSystemArchitecture();
        string GetOperatingSystemName();
    }
}
