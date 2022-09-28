# Inventory
Inventory App demonstrating multiple REST based services including 2 GET methods and 2 PUT methods


Requirements per interviewer: 

Title: REST API development – Inventory API

Develop and document a REST API service that manages the inventory of auto parts in local warehouses for an auto part store chain. 
Use whatever back-end language and libraries you’d like (Java, Python, Node, .NET, Golang, etc.) Focus solely on the API code by providing only an interface 
(i.e., not the actual implementation) for the data layer, which would otherwise be implemented in a local database (you could provide a simple mock implementation for testing).

For nomenclature, a model is a specific kind of part (Manufacturer/Model Number), while an item is a physical part in inventory that is identified by model. 
Models are child elements of a “part category”, e.g. transmission, brakes, engine, etc.

Allow for the following API functions:

1) List the quantity of items per warehouse given a particular model ID
2) List the model name and quantity of items given an optional warehouse identifier of a specified part category
3) Given a model ID, store ID and optional warehouse ID, reserve an item for a particular store (i.e., take it out of available inventory)
4) Request an email notification (given an email address) when an item arrives at a store
5) If familiar, also document the API using the OpenAPI 3.0 (Swagger) standard in either JSON or YAML.