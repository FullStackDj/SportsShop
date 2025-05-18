using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void CanUseRepository()
    {
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1" },
            new Product { ProductID = 2, Name = "Product2" }
        }).AsQueryable<Product>());
        HomeController controller = new(mock.Object);

        ProductsListViewModel result =
            controller.Index(null)?.ViewData.Model
                as ProductsListViewModel ?? new();

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("Product1", prodArray[0].Name);
        Assert.Equal("Product2", prodArray[1].Name);
    }

    [Fact]
    public void CanPaginate()
    {
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1" },
            new Product { ProductID = 2, Name = "Product2" },
            new Product { ProductID = 3, Name = "Product3" },
            new Product { ProductID = 4, Name = "Product4" },
            new Product { ProductID = 5, Name = "Product5" }
        }).AsQueryable<Product>());
        HomeController controller = new(mock.Object);
        controller.PageSize = 3;

        ProductsListViewModel result =
            controller.Index(null, 2)?.ViewData.Model
                as ProductsListViewModel ?? new();

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("Product4", prodArray[0].Name);
        Assert.Equal("Product5", prodArray[1].Name);
    }

    [Fact]
    public void CanSendPaginationViewModel()
    {
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1" },
            new Product { ProductID = 2, Name = "Product2" },
            new Product { ProductID = 3, Name = "Product3" },
            new Product { ProductID = 4, Name = "Product4" },
            new Product { ProductID = 5, Name = "Product5" }
        }).AsQueryable<Product>());
        HomeController controller = new(mock.Object) { PageSize = 3 };

        ProductsListViewModel result =
            controller.Index(null, 2)?.ViewData.Model as
                ProductsListViewModel ?? new();

        PagingInfo pageInfo = result.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }

    [Fact]
    public void CanFilterProducts()
    {
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1", Category = "Category1" },
            new Product { ProductID = 2, Name = "Product2", Category = "Category2" },
            new Product { ProductID = 3, Name = "Product3", Category = "Category3" },
            new Product { ProductID = 4, Name = "Product4", Category = "Category4" },
            new Product { ProductID = 5, Name = "Product5", Category = "Category5" }
        }).AsQueryable<Product>());
        HomeController controller = new(mock.Object);
        controller.PageSize = 3;

        Product[] result = (controller.Index("Category2", 1)?.ViewData.Model
            as ProductsListViewModel ?? new()).Products.ToArray();
        // Assert
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "Product2" && result[0].Category == "Category2");
        Assert.True(result[1].Name == "Product4" && result[1].Category == "Category2");
    }

    [Fact]
    public void GenerateCategorySpecificProductCount()
    {
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new Product { ProductID = 1, Name = "Product1", Category = "Category1" },
            new Product { ProductID = 2, Name = "Product2", Category = "Category2" },
            new Product { ProductID = 3, Name = "Product3", Category = "Category1" },
            new Product { ProductID = 4, Name = "Product4", Category = "Category2" },
            new Product { ProductID = 5, Name = "Product5", Category = "Category3" }
        }).AsQueryable<Product>());
        HomeController target = new(mock.Object);
        target.PageSize = 3;
        Func<ViewResult, ProductsListViewModel?> GetModel = result
            => result?.ViewData?.Model as ProductsListViewModel;

        int? result1 = GetModel(target.Index("Category1"))?.PagingInfo.TotalItems;
        int? result2 = GetModel(target.Index("Category2"))?.PagingInfo.TotalItems;
        int? result3 = GetModel(target.Index("Category3"))?.PagingInfo.TotalItems;
        int? allResults = GetModel(target.Index(null))?.PagingInfo.TotalItems;

        Assert.Equal(2, result1);
        Assert.Equal(2, result2);
        Assert.Equal(1, result3);
        Assert.Equal(5, allResults);
    }
}