using MyALWebScrapping.Configs;
using MyALWebScrapping.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

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

                    var sw = new Stopwatch();

                    while (amountOfScrappedData <= 1000)
                    {
                        IList<IWebElement> rows = driver.FindElements(By.ClassName("ranking-list"));
                        IList<IWebElement> ranks = driver.FindElements(By.ClassName("top-anime-rank-text"));
                        IList<IWebElement> names = driver.FindElements(By.ClassName("anime_ranking_h3"));
                        IList<IWebElement> scores = driver.FindElements(By.ClassName("score-label"));

                        sw.Start();
                        foreach (IWebElement row in rows)
                        {
                            int rank = int.Parse(ranks[rows.IndexOf(row)].Text);
                            string name = names[rows.IndexOf(row)].Text;
                            double score = 0;

                            if (scores[rows.IndexOf(row)].Text == "N/A")
                                score = double.Parse(scores[rows.IndexOf(row) + 1].Text, CultureInfo.InvariantCulture);
                            else
                                score = double.Parse(scores[rows.IndexOf(row)].Text, CultureInfo.InvariantCulture);

                            Anime anime = new Anime(rank, name, score);
                        }
                        sw.Stop();

                        Console.WriteLine("TIME: " + sw.ElapsedMilliseconds);
                        sw.Restart();

                        amountOfScrappedData += rows.Count;

                        if (!DidNextPaginationLoaded(driver, Convert.ToInt32(ranks[ranks.Count - 1].Text)))
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
