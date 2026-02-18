# API & Controller Routes

This app follows the ASP.NET MVC pattern (controllers, actions, models).  
Below are key user-accessible routes and their purposes.

## User Routes

| Path                      | Method | Purpose                              |
|---------------------------|--------|--------------------------------------|
| `/User/Shop`              | GET    | Browse and filter products           |
| `/User/Experience`        | GET    | View curated experience bundles      |
| `/User/CartPage`          | GET    | See and manage cart                  |
| `/User/BuyNow/{id}`       | GET    | Buy a product directly (by id)      |
| `/User/ConfirmOrder`      | POST   | Finalize order                       |
| `/User/FetchOrders`       | GET    | See order history                    |
| `/User/SignIn/SignOut`    | POST   | User login/logout                    |

## Admin Routes

| Path                        | Method | Purpose                                  |
|-----------------------------|--------|------------------------------------------|
| `/Admin/FetchProduct`       | GET    | List all products                        |
| `/Admin/AddProduct`         | GET/POST | Show/add product form                   |
| `/Admin/EditProduct/{id}`   | GET/POST | Edit a product                          |
| `/Admin/DeleteProduct/{id}` | POST   | Delete a product                         |
| `/Admin/FetchOrders`        | GET    | View order list                          |
| `/Admin/OrderDetails/{id}`  | GET    | Order detail                             |
| `/Admin/Profile`            | GET/POST | Admin profile view/update               |

_Note: Actual implementation may allow both GET and POST for some actions (as per ASP.NET conventions)._
