using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace FAWebTest.Specs.PageObjects
{
    /// <summary>
    /// Calculator Page Object
    /// </summary>
    public class HomePageObject
    {
        //The URL of the calculator to be opened in the browser
        private const string FAWebURL = "https://www.footasylum.com";

        //The Selenium web driver to automate the browser
        private readonly IWebDriver _webDriver;
        
        //The default wait time in seconds for wait.Until
        public const int DefaultWaitInSeconds = 10;

        public HomePageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void NavigateToHomePage()
        {
            if (_webDriver.Url != FAWebURL)
            {
                _webDriver.Url = FAWebURL;
            }
        }


        public void AddItemsToBasket(int numItems)
        {
            //TODO make this more intelligent, probably with an external list of test products
            List<string> testProductIds = new List<string>(new string[] { "101752", "083280", "4062549" });

            //add the requested number of products to the basket
            for (int i = 0; i < numItems; i++)
            {
                AddItemToBasket(testProductIds[i]);
            }
        }

        public void AddItemToBasket(string productId)
        {
            _webDriver.Url = "https://www.footasylum.com/page/search/?term=" + productId;

            _webDriver.FindElement(By.ClassName("listing-image")).Click();

            //select the first available size and add to basket
            _webDriver.FindElement(By.ClassName("option2dropElm")).Click();
            _webDriver.FindElement(By.Id("kaddtobasketbtn")).Click();

            //wait for the basket to update
            WaitForBasketToUpdate();
        }

        public Boolean ElementExists(By Locator)
        {
            IReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(Locator);
            if (elements.Count > 0) return true;
            else return false;
        }

        public string WaitForBasketToUpdate()
        {
            IWebElement ResultElement = _webDriver.FindElement(By.Id("itemAddedbasketSummaryMenu"));
            //Wait for the result to be empty
            return WaitUntil(
                () => ResultElement.GetAttribute("style"),
                result => result == string.Empty);
        }

        /// <summary>
        /// Helper method to wait until the expected result is available on the UI
        /// </summary>
        /// <typeparam name="T">The type of result to retrieve</typeparam>
        /// <param name="getResult">The function to poll the result from the UI</param>
        /// <param name="isResultAccepted">The function to decide if the polled result is accepted</param>
        /// <returns>An accepted result returned from the UI. If the UI does not return an accepted result within the timeout an exception is thrown.</returns>
        private T WaitUntil<T>(Func<T> getResult, Func<T, bool> isResultAccepted) where T: class
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            return wait.Until(driver =>
            {
                var result = getResult();
                if (!isResultAccepted(result))
                    return default;

                return result;
            });

        }
    }
}
