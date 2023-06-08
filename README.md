# BikeStores
 
Main Requirements:
Connect to attached DB using EF (Database-first)
Provide APIs for Orders
Create Order
Get single order (by ID)
Get list of orders (with paging)
Cancel Order (using only Order ID)
All APIs should follow REST approach
On GET Endpoints Order information should include all information about order, including Customer, Store, Stuff information and Order 
Items (Order Item should include Product Name additionaly to base Order Item information)
DB connection details must be manageble in configuration



Additional Requirements:
APIs can be implemented using Minimal APIs
Add authorization schemas to Order APIs (e.g. authorized customer can view only his orders). You can use static JWT tokens.
Cover application code with Unit tests
Create Docker Image from the application