using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestingExProject
{
    class SingleTest
    {
        static void Main(string[] args)
        {
            // Execute the tests
            execute();
        }

        [Test]
        public static void execute()
        {
            // Update your LambdaTest credentials
            String LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME");
            String LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");

            if (LT_USERNAME == null || LT_ACCESS_KEY == null)
            {
                throw new Exception("LambdaTest credentials are not set in the environment variables.");
            }

            IWebDriver driver;
            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = "latest";
            Dictionary<string, object> ltOptions = new Dictionary<string, object>
            {
                { "username", LT_USERNAME },
                { "accessKey", LT_ACCESS_KEY },
                { "platformName", "Windows 10" },
                { "project", "Demo LT" },
                { "build", "C# Build" },
                { "sessionName", "C# Single Test" },
                { "w3c", true },
                { "plugin", "c#-c#" }
            };
            capabilities.AddAdditionalOption("LT:Options", ltOptions);

            driver = new RemoteWebDriver(new Uri("https://hub.lambdatest.com/wd/hub/"), capabilities);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                Console.WriteLine("Navigating to todos app.");
                driver.Navigate().GoToUrl("https://lambdatest.github.io/sample-todo-app/");

                driver.FindElement(By.Name("li4")).Click();
                Console.WriteLine("Clicking Checkbox");
                driver.FindElement(By.Name("li5")).Click();

                IList<IWebElement> elems = driver.FindElements(By.ClassName("done-true"));
                if (elems.Count != 2)
                    throw new Exception();

                Console.WriteLine("Entering Text");
                driver.FindElement(By.Id("sampletodotext")).SendKeys("Yey, Let's add it to list");
                driver.FindElement(By.Id("addbutton")).Click();

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
