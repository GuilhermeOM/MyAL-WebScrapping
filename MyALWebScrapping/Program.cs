using MyALWebScrapping.Configs;
using MyALWebScrapping.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MyALWebScrapping
{
    public class Program
    {
        static void Main(string[] args)
        {
            ChromeDriverConfig driverConfig = new ChromeDriverConfig();
            IWebDriver driver = new ChromeDriver(driverConfig.service, driverConfig.options);

            driver.Navigate().GoToUrl("https://myanimelist.net/topanime.php");

            if (ElementExists(driver, "top-ranking-table", 10))
            {
                try
                {
                    int amountOfScrappedData = 0;

                    while (amountOfScrappedData <= 1000)
                    {
                        IList<IWebElement> rows = driver.FindElements(By.ClassName("ranking-list"));

                        int rank = 0;
                        string name = null;
                        double score = 0;

                        foreach (IWebElement row in rows)
                        {
                            rank = Int32.Parse(row.FindElement(By.ClassName("top-anime-rank-text")).Text);
                            name = row.FindElement(By.ClassName("anime_ranking_h3")).Text;
                            score = Double.Parse(row.FindElement(By.ClassName("score-label")).Text, CultureInfo.InvariantCulture);

                            Anime anime = new Anime(rank, name, score);

                            Console.WriteLine("RANK: " + anime.Rank);
                            Console.WriteLine("NAME: " + anime.Name);
                            Console.WriteLine("SCORE: " + anime.Score);
                        }

                        amountOfScrappedData += rows.Count;

                        if (!DidNextPaginationLoaded(driver, rank))
                            break;
                    }
                }
                catch (StaleElementReferenceException error)
                {
                    Console.WriteLine(error.StackTrace);
                }
                catch (NoSuchElementException error)
                {
                    Console.WriteLine(error.StackTrace);
                }
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

        private static bool DidNextPaginationLoaded(IWebDriver currentDriver, int lastCheckedRank)
        {
            try
            {
                currentDriver.FindElement(By.ClassName("next")).Click();

                Thread.Sleep(300);

                int currentTableRank = Convert.ToInt32(currentDriver.FindElement(By.ClassName("top-anime-rank-text")).Text);

                if (currentTableRank > lastCheckedRank)
                    return true;

                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
