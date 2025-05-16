using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

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

        IEnumerable<Product>? result =
            (controller.Index() as ViewResult)?.ViewData.Model
            as IEnumerable<Product>;

        Product[] prodArray = result?.ToArray() ?? Array.Empty<Product>();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("Product1", prodArray[0].Name);
        Assert.Equal("Product2", prodArray[1].Name);
    }
}