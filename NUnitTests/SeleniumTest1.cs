using NuGet.Frameworks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace NUnitTests
{
    public class QA_Tests
    {
        private EdgeDriver driver;
        private IJavaScriptExecutor js;

        private QA_POM qa_pom;

        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            qa_pom = new QA_POM(driver);
            js = (IJavaScriptExecutor)driver;
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        [Test]
        public void Test_To_Post_Message_To_tx100()
        {
            qa_pom.NavigateToUrl("https://tx100.onrender.com/chat/student");
            IWebElement nameField = qa_pom.getElementById("name");
            nameField.SendKeys("James W");
            IWebElement messageField = qa_pom.getElementById("message");
            messageField.SendKeys("Hello from my Selenium Test!");
            messageField.SendKeys(Keys.Enter);
        }
    }


    public class QA_POM
    {
        private IWebDriver driver;
        public int elementWaitTime = 10;
        
        public QA_POM(IWebDriver driver)
        {
            this.driver = driver;
        }   

        public IWebDriver NavigateToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(elementWaitTime));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            
            return driver;
        }


        public void dismissCookies(IWebElement cookieButton)
        {
            if(cookieButton.Displayed)
            {
                cookieButton.Click();
            }           
        }

        public void performSearch(IWebElement searchBox, IWebElement searchButton, string query)
        {
            searchBox.Clear();
            searchBox.SendKeys(query);
            searchButton.Click();
        }

        public IWebElement getElement(string css)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(elementWaitTime));
            IWebElement element = wait.Until(driver =>
            {
                var el = driver.FindElement(By.CssSelector(css));
                return el.Displayed ? el : null;
            });
            return element;
        }

        public IWebElement getElementByName(string name)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(elementWaitTime));
            IWebElement element = wait.Until(driver =>
            {
                var el = driver.FindElement(By.Name(name));
                return el.Displayed ? el : null;
            });
            return element;
        }

        public IWebElement getElementById(string id)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(elementWaitTime));
            IWebElement element = wait.Until(driver =>
            {
                var el = driver.FindElement(By.Id(id));
                return el.Displayed ? el : null;
            });
            return element;
        }

    }

}
