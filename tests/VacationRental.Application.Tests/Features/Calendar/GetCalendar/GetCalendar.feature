Feature: GetCalendar

Scenario: Get calendar
	Given that exists a rental with
		| id | units | preparationTimeInDays |
		| 5  | 3     | 2                     |
	And that exists a booking with
		| id | rentalId | start | nights |
		| 1  | 5        | D+1   | 3      |
		| 2  | 5        | D+4   | 3      |
		| 3  | 5        | D+7   | 3      |
		| 4  | 5        | D+10  | 3      |
	When fetching a calendar with
		| rentalId | start | nights |
		| 5        | D+5   | 6      |
	Then should return calendar
		| rentalId | Date | BookingId | BookingUnit | PreparationTimeUnit |
		| 5        | D+5  | 2         | 2           | 1                   |
		| 5        | D+6  | 2         | 2           |                     |
		| 5        | D+7  | 3         | 1           | 2                   |
		| 5        | D+8  | 3         | 1           | 2                   |
		| 5        | D+9  | 3         | 1           |                     |
		| 5        | D+10 | 4         | 2           | 1                   |

Scenario Outline: Invalid input
	When fetching a calendar with
		| rentalId   | start   | nights   |
		| <rentalId> | <start> | <nights> |
	Then should throw 'DataContractValidationException'

Examples:
	| rentalId | start | nights |
	|          | D0    | 1      |
	| 1        |       | 1      |
	| 1        | D0    |        |
			
Scenario: Invalid rentalId input
	When fetching a calendar with
		| rentalId | start | nights |
		| 1        | D+1   | 1      |
	Then should throw 'RentalNotFoundException'
