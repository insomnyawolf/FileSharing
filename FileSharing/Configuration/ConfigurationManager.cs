﻿using Microsoft.Extensions.Configuration;

namespace FileSharing.Configuration
{
    public static class ConfigurationManager
    {
        public static IConfiguration Configuration { get; private set; }
        public static AppSettings AppSettings { get; private set; }

        public static void Initialize(IConfiguration Configuration)
        {
            ConfigurationManager.Configuration = Configuration;

            ConfigurationReloadManager(null);
        }

        private static void ConfigurationReloadManager(object state)
        {
            // Reload The Config
            AppSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            // Reregister the change listener
            Configuration.GetReloadToken().RegisterChangeCallback(ConfigurationReloadManager, null);
        }
    }
}
