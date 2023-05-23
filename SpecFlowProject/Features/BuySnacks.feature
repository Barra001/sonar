Feature: BuySnacks

@mytag
Scenario: Buy ticket with one snack
	Given I buy 2 snacks
	And 2 tickets 
	When I purchase the tickets with the snack
	Then the ticket should have an ammount of 2
	And there should be 1 snack

@mytag
Scenario: Buy ticket with 2 types of snacks
	Given I buy 2 snacks of one type
	And 3 snacks of another type
	And 3 tickets 
	When I purchase the tickets with the snacks
	Then the ticket should have an ammount of 3
	And there should be 2 snack

	
@mytag
Scenario: Buy ticket without snacks
	Given I buy 0 snacks 
	And 3 tickets 
	When I purchase the tickets without the snacks
	Then the ticket should have an ammount of 3
	And there should be 0 snack

