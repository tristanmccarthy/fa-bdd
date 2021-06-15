using FAWebTest.Specs.Drivers;
using FAWebTest.Specs.PageObjects;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace FAWebTest.Specs.Features
{
    [Binding]
    public class ManageBasketSteps
    {
        //Page Object for basket
        private readonly BasketPageObject _basketPageObject;
        private readonly HomePageObject _homePageObject;

        public ManageBasketSteps(BrowserDriver browserDriver)
        {
            _basketPageObject = new BasketPageObject(browserDriver.Current);
            _homePageObject = new HomePageObject(browserDriver.Current);
        }

        [Given(@"I have '(.*)' item\(s\) in my basket")]
        public void GivenIHaveItemSInMyBasket(int NumBasketItems)
        {
            _homePageObject.AddItemsToBasket(NumBasketItems);
            _basketPageObject.NavigateToBasket();
            _basketPageObject.GetBasketItemCount().Should().Be(NumBasketItems);
        }
        
        [When(@"I remove all items from the basket")]
        public void WhenIRemoveAllItemsFromTheBasket()
        {
            _basketPageObject.NavigateToBasket();
            _basketPageObject.RemoveAllItems();
            
        }
        
        [Then(@"my basket should be empty")]
        public void ThenMyBasketShouldBeEmpty()
        {
            //delegate to Page Object
            int actualResult = _basketPageObject.GetBasketItemCount();

            actualResult.Should().Be(0);
        }
    }
}
