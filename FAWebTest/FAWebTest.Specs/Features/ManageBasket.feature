@basket
Feature: Manage the contents of the basket

Link to a feature: [basket](FAWebTest.Specs/Features/ManageBasket.feature)

Scenario: Remove an item from the basket
	Given I have '1' item(s) in my basket
	When I remove all items from the basket
	Then my basket should be empty
