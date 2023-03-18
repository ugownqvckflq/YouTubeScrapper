using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;


    namespace YouTubeParser
    {

        internal class Program
        {
            static void Main(string[] args)
            {
                
                IWebDriver driver = new ChromeDriver();
                string test_url_1 = "https://www.youtube.com/@upvotemedia/videos";
                driver.Url = test_url_1;
                int vcount = 0;

                var timeout = 10000; /* Maximum wait time of 10 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                Thread.Sleep(5000);

                
                Int64 last_height = (Int64)(((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight"));
                while (true)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
                    /* Wait to load page */
                    Thread.Sleep(2000);
                    /* Calculate new scroll height and compare with last scroll height */
                    Int64 new_height = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight");
                    if (new_height == last_height)
                        /* If heights are the same it will exit the function */
                        break;
                    last_height = new_height;
                }

            try
            {
                By elem_video_link = By.CssSelector("ytd-rich-item-renderer.style-scope.ytd-rich-grid-row");
                ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
                Console.WriteLine("Total number of videos in " + test_url_1 + " are " + videos.Count);

                foreach (IWebElement video in videos)
                {
                    string str_title, str_views, str_rel;
                    IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title-link"));
                    str_title = elem_video_title.Text;

                    IWebElement elem_video_views = video.FindElement(By.XPath("//*[@id='metadata-line']/span[1]"));
                    str_views = elem_video_views.Text;

                    IWebElement elem_video_reldate = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[2]"));
                    str_rel = elem_video_reldate.Text;

                    Console.WriteLine("******* Video " + vcount + " *******");
                    Console.WriteLine("Video Title: " + str_title);
                    Console.WriteLine("Video Views: " + str_views);
                    Console.WriteLine("Video Release Date: " + str_rel);
                    Console.WriteLine("\n");
                    vcount++;
                }

                Console.WriteLine("Scraping Data from NixPravin YouTube channel Passed");

            }
            catch (Exception)
            {

                Console.WriteLine("Exception"); //log
            }

               

                





            }
        }
    }

