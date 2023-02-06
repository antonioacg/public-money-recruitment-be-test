Feature: GetBookings

Scenario: Get existing booking
	Given that exists a booking with id '1'
	When trying to fetch a booking with id '1'
	Then should return the booking with id '1'

Scenario: Non existing booking
	Given that exists a booking with id '1'
	When trying to fetch a booking with id '2'
	Then should return null
	
Scenario Outline: Invalid id input
	Given that exists a booking with id '1'
	When trying to fetch a booking with id '<id>'
	Then should throw 'ArgumentException'

Examples:
	| id |
	|    |
	| 0  |
	| -1 |
