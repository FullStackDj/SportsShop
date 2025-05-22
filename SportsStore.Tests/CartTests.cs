using SportsStore.Models;

namespace SportsStore.Tests;

public class CartTests
{
    [Fact]
    public void CanAddNewLines()
    {
        Product product1 = new Product { ProductID = 1, Name = "Product1" };
        Product product2 = new Product { ProductID = 2, Name = "Product2" };

        Cart target = new();

        target.AddItem(product1, 1);
        target.AddItem(product2, 1);
        CartLine[] results = target.Lines.ToArray();

        Assert.Equal(2, results.Length);
        Assert.Equal(product1, results[0].Product);
        Assert.Equal(product2, results[1].Product);
    }

    [Fact]
    public void CanAddQuantityForExistingLines()
    {
        Product product1 = new Product { ProductID = 1, Name = "Product1" };
        Product product2 = new Product { ProductID = 2, Name = "Product2" };

        Cart target = new();

        target.AddItem(product1, 1);
        target.AddItem(product2, 1);
        target.AddItem(product1, 10);
        CartLine[] results = (target.Lines ?? new())
            .OrderBy(c => c.Product.ProductID).ToArray();

        Assert.Equal(2, results.Length);
        Assert.Equal(11, results[0].Quantity);
        Assert.Equal(1, results[1].Quantity);
    }

    [Fact]
    public void CanRemoveLine()
    {
        Product product1 = new Product { ProductID = 1, Name = "Product1" };
        Product product2 = new Product { ProductID = 2, Name = "Product2" };
        Product product3 = new Product { ProductID = 3, Name = "Product3" };

        Cart target = new();

        target.AddItem(product1, 1);
        target.AddItem(product2, 3);
        target.AddItem(product3, 5);
        target.AddItem(product2, 1);

        target.RemoveLine(product2);

        Assert.Empty(target.Lines.Where(c => c.Product == product2));
        Assert.Equal(2, target.Lines.Count());
    }

    [Fact]
    public void CalculateCartTotal()
    {
        Product product1 = new Product { ProductID = 1, Name = "Product1", Price = 80M };
        Product product2 = new Product { ProductID = 2, Name = "Product2", Price = 30M };

        Cart target = new();

        target.AddItem(product1, 1);
        target.AddItem(product2, 1);
        target.AddItem(product1, 3);
        decimal result = target.ComputeTotalValue();

        Assert.Equal(350M, result);
    }

    [Fact]
    public void CanClearContents()
    {
        Product product1 = new Product { ProductID = 1, Name = "Product1", Price = 120M };
        Product product2 = new Product { ProductID = 2, Name = "Product2", Price = 60M };

        Cart target = new();

        target.AddItem(product1, 1);
        target.AddItem(product2, 1);

        target.Clear();

        Assert.Empty(target.Lines);
    }
}