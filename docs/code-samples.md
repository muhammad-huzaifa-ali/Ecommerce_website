# Code Samples

## Example: Add to Cart (C# Controller)

```csharp
public IActionResult AddToCart(int id)
{
    string userid = HttpContext.Session.GetString("User_Session");
    if (userid == null) return RedirectToAction("SignIn");
    int customerId = Convert.ToInt32(userid);
    var cartItem = new Cart { customer_id = customerId, product_id = id, product_status = 0 };
    _context.Tbl_Cart.Add(cartItem);
    _context.SaveChanges();
    return RedirectToAction("CartPage");
}
```

## Example: Product List Table (Razor View)

```html+razor
@foreach (var product in Model.Products)
{
    <tr>
        <td>@product.product_name</td>
        <td>@product.product_price</td>
        <td>@product.category.category_name</td>
        <td>
            <a asp-action="EditProduct" asp-route-id="@product.product_id">Edit</a>
            <a asp-action="DeleteProduct" asp-route-id="@product.product_id">Delete</a>
        </td>
    </tr>
}
```

## Example: Session Configuration (Program.cs)
```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```
