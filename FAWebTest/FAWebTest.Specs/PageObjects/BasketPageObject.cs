using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FAWebTest.Specs.PageObjects
{
    /// <summary>
    /// Calculator Page Object
    /// </summary>
    public class BasketPageObject
    {
        //The URL of the calculator to be opened in the browser
        private const string FAWebBasketURL = "https://www.footasylum.com/page/basket/";

        //The Selenium web driver to automate the browser
        private readonly IWebDriver _webDriver;
        
        //The default wait time in seconds for wait.Until
        public const int DefaultWaitInSeconds = 10;

        public BasketPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void NavigateToBasket()
        {
            if (_webDriver.Url != FAWebBasketURL)
            {
                _webDriver.Url = FAWebBasketURL;
            }
        }


        public void RemoveAllItems()
        {
            IReadOnlyCollection<IWebElement> removeLinks = _webDriver.FindElements(By.LinkText("Remove"));
            foreach (IWebElement removeLink in removeLinks)
            {
                removeLink.Click();
            }
        }

        public int GetBasketItemCount()
        {
            //no basket items if the basket view doesn't load
            if (!ElementExists(By.ClassName("bsktitemcontainer")))
            {
                return 0;
            }
            else
            {
                int ItemCount = 0;
                IReadOnlyCollection<IWebElement> Quantities = _webDriver.FindElements(By.XPath("//*[contains(@id,'qty_')]"));
                foreach (IWebElement Quantity in Quantities)
                {
                    string quantity = Quantity.GetAttribute("value");
                    if (quantity != null) ItemCount += Int32.Parse(quantity);
                }
                return ItemCount;
            }
        }

        public Boolean ElementExists(By Locator)
        {
            IReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(Locator);
            if (elements.Count > 0) return true;
            else return false;
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
