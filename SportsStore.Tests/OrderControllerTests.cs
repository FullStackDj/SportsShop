using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStore.Tests;

public class OrderControllerTests
{
    [Fact]
    public void CannotCheckoutEmptyCart()
    {
        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        Order order = new();
        OrderController target = new(mock.Object, cart);

        ViewResult? result = target.Checkout(order) as ViewResult;

        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        Assert.True(string.IsNullOrEmpty(result?.ViewName));
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void CannotCheckoutInvalidShippingDetails()
    {
        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        cart.AddItem(new Product(), 1);
        OrderController target = new(mock.Object, cart);
        target.ModelState.AddModelError("error", "error");

        ViewResult? result = target.Checkout(new Order()) as ViewResult;

        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        Assert.True(string.IsNullOrEmpty(result?.ViewName));
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void CanCheckoutAndSubmitOrder()
    {
        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        cart.AddItem(new Product(), 1);
        OrderController target = new(mock.Object, cart);

        RedirectToPageResult? result = target.Checkout(new Order()) as RedirectToPageResult;

        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
        Assert.Equal("/Completed", result?.PageName);
    }
}