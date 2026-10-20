Harden a CRUD Web API
ASP.NET Core Web API demonstrating:

Data Annotation Validation
Custom Validation using IValidatableObject
Domain Exceptions
Global Exception Handling
RFC 9457 Problem Details
Standardized HTTP Responses
Paging with data/meta envelope

Endpoint Status Codes
	Endpoint				Success							Errors
GET /api/products			200 OK							500 Internal Server Error
GET /api/products/{id}		200 OK	404 Not Found,			500 Internal Server Error
POST /api/products			201 Created	400 Bad Request,	409 Conflict, 500 Internal Server Error
PUT /api/products/{id}		200 OK	400 Bad Request,		404 Not Found, 500 Internal Server Error
DELETE /api/products/{id}	204 No Content	404 Not Found,	500 Internal Server Error