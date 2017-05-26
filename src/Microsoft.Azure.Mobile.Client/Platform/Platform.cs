// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Reflection;

#if NETSTANDARD1_4
using System.Runtime.InteropServices;
#endif 

namespace Microsoft.WindowsAzure.MobileServices
{

    /// <summary>
    /// Set any callbacks and properties here to your
    /// heart's content if you're using NetStandard 1.4; otherwise,
    /// this is useless. Make sure they're
    /// set before Platform.Instance is queried.
    /// </summary>
    public static class PlatformDetail
    {
#if NETSTANDARD1_4
        public interface IApplicationStorageOps
        {
            System.IO.FileStream GetFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access);
        }

        public static IApplicationStorageOps AppStorageOps { get; set; }

        /// <summary>
        /// Path to the user's local application data storage. E.g., on windows this is often %USERPROFILE%\AppData\Local\YOUR_APPLICATION_NAME_HERE
        /// </summary>
        public static string LocalApplicationDataDirectory { get; set; }

        /// <summary>
        /// The version string of the operating system, apparently for an HTTP user-agent string.
        /// </summary>
        public static string OSVersion { get; set; }

        /// <summary>
        /// Windows net45 runtime reports this as "Win32NT"; apparently this is for an HTTP user-agent string.
        /// That's all I can really provide.
        /// </summary>
        public static string OSPlatformArch { get; set; }

        /// <summary>
        /// The name of the operating system.
        /// </summary>
        public static string OSName { get; set; }

        /// <summary>
        /// Matched OSPlatform result according to RuntimeInformation.IsOSPlatform(),
        /// or a thrown exception if no match is found.
        /// </summary>
        public static OSPlatform PlatformOrdinal
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return OSPlatform.Windows;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return OSPlatform.OSX;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return OSPlatform.Linux;
                }

                throw new InvalidOperationException("Could not identify operating system; RuntimeInformation.IsOSPlatform() returned false for all 3 values");
            }
        }

        internal static void Validate()
        {
            if (string.IsNullOrEmpty(LocalApplicationDataDirectory))
            {
                throw new NullReferenceException("PlatformDetail.LocalApplicationDataDirectory must be set");
            }

            if (AppStorageOps == null)
            {
                throw new NullReferenceException("AppStorageOps cannot be null.");
            }

            string defaultName = PlatformOrdinal.ToString();

            if (defaultName == "OSX")
            {
                defaultName = "MacOS";
            }

            string[] desc = RuntimeInformation.OSDescription.Split(' ');

            
            if (string.IsNullOrEmpty(OSVersion))
            {
                OSVersion = desc[2];
            }

            if (string.IsNullOrEmpty(OSPlatformArch))
            {
                OSPlatformArch = defaultName;
            }

            if (string.IsNullOrEmpty(OSName))
            {
                OSName = defaultName;
            }
        }
#else
         /// <summary>
        /// Path to the user's local application data storage. E.g., on windows this is often %USERPROFILE%\AppData\Local\YOUR_APPLICATION_NAME_HERE
        /// </summary>
        public static string LocalApplicationDataDirectory { get { throw new InvalidOperationException("This class is only meant to be used with .NET Core"); } }

        /// <summary>
        /// The version string of the operating system, apparently for an HTTP user-agent string.
        /// </summary>
        public static string OSVersion { get { throw new InvalidOperationException("This class is only meant to be used with .NET Core"); } }

        /// <summary>
        /// Windows net45 runtime reports this as "Win32NT"; apparently this is for an HTTP user-agent string.
        /// That's all I can really provide.
        /// </summary>
        public static string OSPlatformArch { get { throw new InvalidOperationException("This class is only meant to be used with .NET Core"); } }

        /// <summary>
        /// The name of the operating system.
        /// </summary>
        public static string OSName { get { throw new InvalidOperationException("This class is only meant to be used with .NET Core"); } }

        /// <summary>
        /// Dummy stub.
        /// </summary>
        public enum OSPlatform {}

        /// <summary>
        /// Matched OSPlatform result according to RuntimeInformation.IsOSPlatform(),
        /// or a thrown exception if no match is found.
        /// </summary>
        public static OSPlatform PlatformOrdinal
        {
            get { throw new InvalidOperationException("This class is only meant to be used with .NET Core"); }
        }
#endif
    }


    /// <summary>
    /// Provides access to platform-specific framework API's.
    /// </summary>
    internal static class Platform
    {

        /// <summary>
        /// The string value to use for the operating system name, arch, or version if
        /// the value is unknown.
        /// </summary>
        public const string UnknownValueString = "--";

        private static IPlatform current;

        /// <summary>
        /// Gets the current platform. If none is loaded yet, accessing this property triggers platform resolution.
        /// </summary>
        public static IPlatform Instance
        {
            get
            {
                // create if not yet created
                if (current == null)
                {
#if NETSTANDARD1_4
                    PlatformDetail.Validate();
#endif

#if !NETSTANDARD1_1
                    current = new CurrentPlatform();
#else
                    throw new PlatformNotSupportedException("TODO...");
#endif
                }

                return current;
            }

            // keep this public so we can set a Platform for unit testing.
            set
            {
                current = value;
            }
        }
    }
}
