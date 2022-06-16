using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyALWebScrapping.Configs
{
    public class ChromeDriverConfig
    {
        public readonly ChromeOptions options;
        public readonly ChromeDriverService service;

        public ChromeDriverConfig()
        {
            options = new ChromeOptions();
            service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            HandleDriverConfiguration();
        }

        private void HandleDriverConfiguration()
        {
            options.AddArgument("headless");
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");

            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
        }
    }
}
