Feature: UpdateRental

Scenario: Updates a rental
	Given that exists a rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 2                     |
	And that exists a booking with
		| id | rentalId | start | nights |
		| 1  | 5        | D+1   | 3      |
		| 2  | 5        | D+4   | 3      |
		| 3  | 5        | D+7   | 3      |
	When trying to update a rental with
		| rentalId | units | preparationTimeInDays |
		| 5        | 3     | 3                     |
	Then should return the rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 3                     |

Scenario: Attempt to updates a rental beyond units limit
	Given that exists a rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 2                     |
	And that exists a booking with
		| id | rentalId | start | nights |
		| 1  | 5        | D+1   | 3      |
		| 2  | 5        | D+4   | 3      |
		| 3  | 5        | D+7   | 3      |
		| 4  | 5        | D+10  | 3      |
	When trying to update a rental with
		| rentalId | units | preparationTimeInDays |
		| 5        | 3     | 5                     |
	Then should throw 'RentalOverbookingException'
	
Scenario Outline: Invalid input
	When trying to update a rental with
		| rentalId   | units   | preparationTimeInDays   |
		| <rentalId> | <units> | <preparationTimeInDays> |
	Then should throw 'DataContractValidationException'

Examples:
	| rentalId | units | preparationTimeInDays |
	|          | 1     | 1                     |
	| 0        | 1     | 1                     |
	| 1        |       | 1                     |
	| 1        | 0     | 1                     |
	| 1        | 1     | -1                    |

		
Scenario: Invalid rentalId input
	When trying to update a rental with
		| rentalId | units | preparationTimeInDays |
		| 5        | 3     | 1                     |
	Then should throw 'RentalNotFoundException'
