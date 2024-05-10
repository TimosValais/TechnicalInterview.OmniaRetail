# TechnicalInterview.OmniaRetail

Swagger is working for testing
Swagger authentication needs one of these retailer Ids as there are dummy data seeded in database creation :
e3a87e20-36d3-42ee-8993-c8dfbfa01c3b , bc024a7e-c4f6-42a5-a67d-76b8c9efd432 , 6e94c46e-a0b1-4831-bea9-fa48900c3065, 3a66173d-ff1f-4458-9bd9-2fc27887ad35

The use case is, first get token from identity/token. If retailId is in the claims, then with that token there is access
also to the Retailer endpoints.

Then, add to the request header Authorization - (the jwt includes the Scheme which is Bearer)

Without any Authorization there is are some Get endpoints that allow anonymous, the ones withouth the prices.

The identity implementation is a mock, so the API runs only in the development ASP environment, as also I wouldn't put connection strings and jwt secrets in the "application.json", they would need to be secure. 
