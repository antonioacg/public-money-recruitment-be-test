Feature: GetRentals

Scenario: Get existing rental
	Given that exists a rental with id '1'
	When trying to fetch a rental with id '1'
	Then should return the rental with id '1'

Scenario: Non existing rental
	Given that exists a rental with id '1'
	When trying to fetch a rental with id '2'
	Then should return null
	
Scenario Outline: Invalid id input
	Given that exists a rental with id '1'
	When trying to fetch a rental with id '<id>'
	Then should throw 'ArgumentException'

Examples:
	| id |
	|    |
	| 0  |
	| -1 |
