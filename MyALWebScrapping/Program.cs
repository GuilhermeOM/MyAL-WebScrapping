using MyALWebScrapping.Configs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace MyALWebScrapping
{
    public class Program
    {
        static void Main(string[] args)
        {
            ChromeDriverConfig driverConfig = new ChromeDriverConfig();
            IWebDriver driver = new ChromeDriver(driverConfig.service, driverConfig.options);

            driver.Navigate().GoToUrl("https://myanimelist.net/topanime.php");

            if (ElementExists(driver, "pb12", 10))
            {
                Console.WriteLine("Exists");
            }
        }

        private static bool ElementExists(IWebDriver currentDriver, string elementId, int timeout)
        {
            WebDriverWait wait = new WebDriverWait(currentDriver, TimeSpan.FromSeconds(timeout));

            bool elementExists = wait.Until(condition =>
            {
                try
                {
                    IWebElement elementToBeDisplayed = currentDriver.FindElement(By.ClassName(elementId));
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            return elementExists;
        }
    }
}
