using System;
using System.Collections.Generic; // Add this for Dictionary
using System.Collections; // Add this for IList
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestingExProject
{
    class program
    {

        static void Main(string[] args)
        {
            // Execute the tests
            execute();
        }


        [Test]
        public static void execute()
        {
            // Update your lambdatest credentials
            String LT_USERNAME = Environment.GetEnvironmentVariable("athikrehman65.ar");
            String LT_ACCESS_KEY = Environment.GetEnvironmentVariable("JGwzyWC7RRqGqfW5YypyIMHGGSb6HZwfvAIBRPXoFNELxww7qJ");
            IWebDriver driver;
            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = "latest";
            Dictionary<string, object> ltOptions = new Dictionary<string, object>();
            ltOptions.Add("username", LT_USERNAME);
            ltOptions.Add("accessKey", LT_ACCESS_KEY);
            ltOptions.Add("platformName", "Windows 10");
            ltOptions.Add("project", "Demo LT");
            ltOptions.Add("build", "C# Build");
            ltOptions.Add("sessionName", "C# Single Test");
            ltOptions.Add("w3c", true);
            ltOptions.Add("plugin", "c#-c#");
            capabilities.AddAdditionalOption("LT:Options", ltOptions);

            driver = new RemoteWebDriver(new Uri("https://athikrehman65.ar:JGwzyWC7RRqGqfW5YypyIMHGGSb6HZwfvAIBRPXoFNELxww7qJ@hub.lambdatest.com/wd/hub"), capabilities);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                Console.WriteLine("Navigating to todos app.");
                driver.Navigate().GoToUrl("https://lambdatest.github.io/sample-todo-app/");

                driver.FindElement(By.Name("li4")).Click();
                Console.WriteLine("Clicking Checkbox");
                driver.FindElement(By.Name("li5")).Click();

                // If both clicks worked, then the following list should have length 2
                IList<IWebElement> elems = driver.FindElements(By.ClassName("done-true"));
                // so we'll assert that this is correct.
                if (elems.Count != 2)
                    throw new Exception();

                Console.WriteLine("Entering Text");
                driver.FindElement(By.Id("sampletodotext")).SendKeys("Yey, Let's add it to list");
                driver.FindElement(By.Id("addbutton")).Click();

                // Let's also assert that the new todo we added is in the list
                string spanText = driver.FindElement(By.XPath("/html/body/div/div/div/ul/li[6]/span")).Text;
                if (!"Yey, Let's add it to list".Equals(spanText))
                    throw new Exception();

                ((IJavaScriptExecutor)driver).ExecuteScript("lambda-status=passed");
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("lambda-status=failed");
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
