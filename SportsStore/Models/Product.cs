using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models;

public class Product
{
    public long? ProductID { get; set; }
    
    [Required(ErrorMessage = "product name")]
    public string Name { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "description")]
    public string Description { get; set; } = String.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "positive price")]
    [Column(TypeName = "decimal(8, 2)")] 
    public decimal Price { get; set; }

    [Required(ErrorMessage = "category")]
    public string Category { get; set; } = String.Empty;
}