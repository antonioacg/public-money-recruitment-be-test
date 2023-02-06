Feature: AddBookings

Scenario: Adds new booking
	Given that exists a rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 2                     |
	When trying to add a booking with
		| rentalId | start | nights |
		| 5        | D+1   | 3      |
	Then should return the booking with
		| id | rentalId | start | nights |
		| 1  | 5        | D+1   | 3      |

Scenario: Adds new booking when existing others
	Given that exists a rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 2                     |
	And that exists a booking with
		| id | rentalId | start | nights |
		| 1  | 5        | D+1   | 3      |
		| 2  | 5        | D+3   | 3      |
	When trying to add a booking with
		| rentalId | start | nights |
		| 5        | D+4   | 3      |
	Then should return the booking with
		| id | rentalId | start | nights |
		| 3  | 5        | D+4   | 3      |

Scenario: Attempt to add beyond units limit
	Given that exists a rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 2                     |
	And that exists a booking with
		| id | rentalId | start | nights |
		| 1  | 5        | D+1   | 3      |
		| 2  | 5        | D+4   | 3      |
		| 3  | 5        | D+7   | 3      |
		| 4  | 5        | D+10  | 3      |
	When trying to add a booking with
		| rentalId | start | nights |
		| 5        | D+4   | 3      |
	Then should throw 'RentalUnitsNotAvailableException'
	
Scenario Outline: Invalid input
	When trying to add a booking with
		| rentalId   | start   | nights   |
		| <rentalId> | <start> | <nights> |
	Then should throw 'DataContractValidationException'

Examples:
	| rentalId | start | nights |
	|          |       |        |
	| 0        | D0    | 0      |
	| -1       | D0    | 0      |
	| 1        | D0    | -1     |

		
Scenario: Invalid rentalId input
	When trying to add a booking with
		| rentalId | start | nights |
		| 1        | D+1   | 1      |
	Then should throw 'RentalNotFoundException'
