// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAzure.MobileServices
{
    /// <summary>
    /// Implements the <see cref="IPlatform"/> interface for the Windows
    /// Phone platform.
    /// </summary>
    internal class PlatformInformation : IPlatformInformation
    {
        /// <summary>
        /// The version string of the operating system, apparently for an HTTP user-agent string.
        /// </summary>
        public static string OSVersion { get; private set; }

        /// <summary>
        /// Windows net45 runtime reports this as "Win32NT"; apparently this is for an HTTP user-agent string.
        /// That's all I can really provide.
        /// </summary>
        public static string OSPlatformArch { get; private set; }

        /// <summary>
        /// The name of the operating system.
        /// </summary>
        public static string OSName { get; private set; }

        /// <summary>
        /// A singleton instance of the <see cref="ApplicationStorage"/>.
        /// </summary>
        private static IPlatformInformation instance = new PlatformInformation();

        /// <summary>
        /// A singleton instance of the <see cref="ApplicationStorage"/>.
        /// </summary>
        public static IPlatformInformation Instance
        {
            get
            {
                return instance;
            }
        }

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

        static PlatformInformation()
        {
            string defaultName = PlatformOrdinal.ToString();

            if (defaultName == "OSX")
            {
                defaultName = "MacOS";
            }

            string[] desc = RuntimeInformation.OSDescription.Split(' ');

            OSVersion = desc[2];
            OSPlatformArch = defaultName;
            OSName = defaultName;
        }

        /// <summary>
        /// The architecture of the platform.
        /// </summary>
        public string OperatingSystemArchitecture
        {
            get
            {
                return OSPlatformArch;
            }
        }

        /// <summary>
        /// The name of the operating system of the platform.
        /// </summary>
        public string OperatingSystemName
        {
            get
            {
                return OSName;
            }
        }

        /// <summary>
        /// The version of the operating system of the platform.
        /// </summary>
        public string OperatingSystemVersion
        {
            get
            {
                /*
                Version version = Environment.OSVersion.Version;

                return string.Format(CultureInfo.InvariantCulture,
                    "{0}.{1}.{2}.{3}",
                    version.Major,
                    version.Minor,
                    version.Revision,
                    version.Build
                );
                */

                return OSVersion;
            }
        }

        /// <summary>
        /// Indicated whether the device is an emulator or not
        /// </summary>
        public bool IsEmulator
        {
            get
            {
                return false;
            }
        }

        public string Version
        {
            get
            {
                return this.GetVersionFromAssemblyFileVersion();
            }
        }
    }
}
