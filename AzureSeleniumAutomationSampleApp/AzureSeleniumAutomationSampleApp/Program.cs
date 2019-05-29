using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Reflection;

namespace AzureSeleniumAutomationSampleApp
{
    class Program
    {
        private static string InformationMessagePrefix = "ASA (Azure Selenium Automation):\r\n" + "Version: 1.0 \r\n" + "Syntax:\r\n" +
    "ASA --login <login> --password <password> [--path <ChromeDriver path>]\r\n" +
    "ASA --help\r\n";
        static bool ParseCommandLine(string[] args, ref string path, ref string login, ref string password, ref string errorMessage)
        {
            int i = 0;
            path = string.Empty;
            login = string.Empty;
            password = string.Empty;
            errorMessage = string.Empty;
            while ((i < args.Length) && (string.IsNullOrEmpty(errorMessage)))
            {
                switch (args[i++])
                {

                    case "--help":
                        errorMessage = InformationMessagePrefix;
                        break;

                    case "--path":
                        if ((i < args.Length) && (!string.IsNullOrEmpty(args[i])))
                            path = args[i++];
                        else
                        {
                            errorMessage = "Path to ChromeDriver not set";
                            return false;
                        }
                        break;

                    case "--login":
                        if ((i < args.Length) && (!string.IsNullOrEmpty(args[i])))
                            login = args[i++];
                        else
                        {
                            errorMessage = "login not set";
                            return false;
                        }
                        break;
                    case "--password":
                        if ((i < args.Length) && (!string.IsNullOrEmpty(args[i])))
                            password = args[i++];
                        else
                        {
                            errorMessage = "password not set";
                            return false;
                        }
                        break;

                }
            }
            return true;
        }
        static bool ExecuteCommandLine(string path, string login, string password, ref string errorMessage)
        {
            errorMessage = string.Empty;
            ChromeDriver driver = null;
            var options = new ChromeOptions();
            options.AddArguments("--incognito");
            try
            {
                if (string.IsNullOrEmpty(path))
                    driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
                else
                    driver = new ChromeDriver(path, options);
            }
            catch (Exception ex)
            {
                errorMessage = "ChromeDrive not loaded, exception: " + ex.Message;
                return false;
            }
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Navigate().GoToUrl("https://ea.azure.com");
                driver.FindElement(By.Id("i0116")).Clear();
                driver.FindElement(By.Id("i0116")).SendKeys(login);
                driver.FindElement(By.Id("i0116")).SendKeys(Keys.Enter);
                System.Threading.Tasks.Task.Delay(1000).Wait();
                driver.FindElement(By.Id("i0118")).Clear();
                driver.FindElement(By.Id("i0118")).SendKeys(password);
                System.Threading.Tasks.Task.Delay(1000).Wait();
                driver.FindElement(By.Id("i0118")).SendKeys(Keys.Enter);
                driver.FindElement(By.Id("idSIButton9")).Click();
            }
            catch (Exception ex)
            {
                errorMessage = "Exception while opening the session: " + ex.Message;
                return false;
            }

            try
            {
                driver.Close();
                driver.Quit();
            }
            catch (Exception ex)
            {
                errorMessage = "Exception while closing the session: " + ex.Message;
            }
            return true;
        }
        static void Main(string[] args)
        {
            string path = string.Empty;
            string login = string.Empty;
            string password = string.Empty;
            string errorMessage = string.Empty;
            if (ParseCommandLine(args, ref path, ref login, ref password, ref errorMessage))
            {
                if (string.IsNullOrEmpty(errorMessage)&&
                    !string.IsNullOrEmpty(login)&&
                    !string.IsNullOrEmpty(password)
                    )
                {
                    if (!ExecuteCommandLine(path, login, password, ref errorMessage))
                        Console.Write(errorMessage);
                    else
                        Console.Write("Test Successful!");

                }
                else
                    Console.Write(errorMessage);
            }
            else
                Console.Write(errorMessage);
        }
    }
}
