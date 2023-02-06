Feature: AddRentals

Scenario: Adds new rental
	When trying to add a rental with
		| units | preparationTimeInDays |
		| 3     | 2                     |
	Then should return the rental with
		| id | units | preparationTimeInDays |
		| 1  | 3     | 2                     |

	
Scenario Outline: Invalid input
	When trying to add a rental with
		| units   | preparationTimeInDays   |
		| <units> | <preparationTimeInDays> |
	Then should throw 'DataContractValidationException'

Examples:
	| units | preparationTimeInDays |
	|       |                       |
	| 0     | 0                     |
	| -1    | 0                     |
	| 1     | -1                    |
