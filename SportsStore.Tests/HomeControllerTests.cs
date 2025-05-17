using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1" },
            new Product { ProductID = 2, Name = "Product2" }
        }).AsQueryable<Product>());
        HomeController controller = new HomeController(mock.Object);

        ProductsListViewModel result =
            controller.Index()?.ViewData.Model as ProductsListViewModel ?? new();

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("Product1", prodArray[0].Name);
        Assert.Equal("Product2", prodArray[1].Name);
    }

    [Fact]
    public void Can_Paginate()
    {
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1" },
            new Product { ProductID = 2, Name = "Product2" },
            new Product { ProductID = 3, Name = "Product3" },
            new Product { ProductID = 4, Name = "Product4" },
            new Product { ProductID = 5, Name = "Product5" }
        }).AsQueryable<Product>());
        HomeController controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        ProductsListViewModel result =
            controller.Index(2)?.ViewData.Model as ProductsListViewModel ?? new();

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("Product4", prodArray[0].Name);
        Assert.Equal("Product5", prodArray[1].Name);
    }
}