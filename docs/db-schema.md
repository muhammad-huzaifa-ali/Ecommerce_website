# Database Schema Overview

This project uses Entity Framework Core.  
Below are main tables/entities and their relationships.

```mermaid
erDiagram
    Customer ||--o{ Order : places
    Customer ||--o{ Cart : has
    Order }o--|| Product : contains
    Cart }o--|| Product : contains
    Product }o--|| Category : belongs_to
    Admin ||--o{ Product : manages

    Customer {
        int customer_id PK
        string customer_name
        string customer_email
        ...
    }
    Order {
        int order_id PK
        int customer_id FK
        int product_id FK
        int quantity
        datetime order_date
    }
    Cart {
        int cart_id PK
        int customer_id FK
        int product_id FK
        int product_status
    }
    Product {
        int product_id PK
        string product_name
        int category_id FK
        ...
    }
    Category {
        int category_id PK
        string category_name
    }
    Admin {
        int admin_id PK
        string admin_email
        ...
    }
```

## Tables

- **Customer**: User profiles (sign-in, orders, cart)
- **Product**: Store’s items (details, images, pricing, category etc.)
- **Category**: Used to group products
- **Order**: Tracks all orders made
- **Cart**: User’s current cart before checkout
- **Admin**: Store owner/managers; controls admin panel

_Note: This diagram is based on code inspection. Names may differ slightly._
