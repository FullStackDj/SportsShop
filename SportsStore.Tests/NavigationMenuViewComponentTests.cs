using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests
{
    [Fact]
    public void CanSelectCategories()
    {
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product
            {
                ProductID = 1, Name = "Product1",
                Category = "Category1"
            },
            new Product
            {
                ProductID = 2, Name = "Product2",
                Category = "Category1"
            },
            new Product
            {
                ProductID = 3, Name = "Product3",
                Category = "Category2"
            },
            new Product
            {
                ProductID = 4, Name = "Product4",
                Category = "Category3"
            },
        }).AsQueryable<Product>());
        NavigationMenuViewComponent target = new(mock.Object);

        string[] results = ((IEnumerable<string>?)(target.Invoke() as ViewViewComponentResult)?.ViewData?.Model
                            ?? Enumerable.Empty<string>()).ToArray();

        Assert.True(Enumerable.SequenceEqual(new string[]
        {
            "Category1",
            "Category2", "Category3"
        }, results));
    }

    [Fact]
    public void IndicatesSelectedCategory()
    {
        string categoryToSelect = "Category1";
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1", Category = "Category1" },
            new Product { ProductID = 4, Name = "Product2", Category = "Category2" },
        }).AsQueryable<Product>());
        NavigationMenuViewComponent target = new(mock.Object);
        target.ViewComponentContext = new ViewComponentContext
        {
            ViewContext = new ViewContext
            {
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            }
        };
        target.RouteData.Values["category"] = categoryToSelect;

        string? result = (string?)(target.Invoke()
            as ViewViewComponentResult)?.ViewData?["SelectedCategory"];

        Assert.Equal(categoryToSelect, result);
    }
}