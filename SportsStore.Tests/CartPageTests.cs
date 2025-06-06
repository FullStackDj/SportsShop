﻿using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;

namespace SportsStore.Tests;

public class CartPageTests
{
    [Fact]
    public void CanLoadCart()
    {
        Product product1 = new Product { ProductID = 1, Name = "Product1" };
        Product product2 = new Product { ProductID = 2, Name = "Product2" };
        Mock<IStoreRepository> mockRepo = new();
        mockRepo.Setup(m => m.Products).Returns((new Product[]
        {
            product1, product2
        }).AsQueryable<Product>());

        Cart testCart = new();
        testCart.AddItem(product1, 2);
        testCart.AddItem(product2, 1);

        CartModel cartModel = new(mockRepo.Object, testCart);
        cartModel.OnGet("myUrl");

        Assert.Equal(2, cartModel.Cart.Lines.Count());
        Assert.Equal("myUrl", cartModel.ReturnUrl);
    }

    [Fact]
    public void CanUpdateCart()
    {
        Mock<IStoreRepository> mockRepo = new();
        mockRepo.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1" }
        }).AsQueryable<Product>());
        Cart testCart = new();

        CartModel cartModel = new(mockRepo.Object, testCart);
        cartModel.OnPost(1, "myUrl");

        Assert.Single(testCart.Lines);
        Assert.Equal("Product1", testCart.Lines.First().Product.Name);
        Assert.Equal(1, testCart.Lines.First().Quantity);
    }
}