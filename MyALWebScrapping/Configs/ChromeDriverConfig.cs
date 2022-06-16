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
        public ChromeOptions options;
        public ChromeDriverService service;

        public ChromeDriverConfig()
        {
            HandleDriverConfiguration();
        }

        private void HandleDriverConfiguration()
        {
            options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");

            service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
        }
    }
}
