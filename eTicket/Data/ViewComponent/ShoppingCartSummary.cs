using eTicket.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace eTicket.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;
        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();

            // Specify the view name to be associated with this ViewComponent
            return View(items.Count);
        }
    }
}
