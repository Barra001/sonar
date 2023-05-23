Feature: AddRemoveSnacks

@mytag
Scenario: Add valid Snack
	Given A valid snack with price 100 and description Papas
	When I add the snack
	Then the snack should be added

@mytag
Scenario: Add invalid Snack
	Given A invalid snack with price -100 and description Papas
	When I add the invalid snack
	Then a exception should be thrown saying price was invalid

@mytag
Scenario: Remove valid Snack
	Given A valid snack id 1
	When I remove the valid snack
	Then the snack should be removed

@mytag
Scenario: Remove invalid Snack
	Given A invalid snack id -1
	When I remove the invalid snack
	Then a exception should be thrown that says snackId was invalid



