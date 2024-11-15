using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace macrix_wk_backend.Models;

public class AddressModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required]
    public required string Street { get; set; }
    public string? ApartmentNo { get; set; }
    [Required]
    public required string PostalCode { get; set; }
    [Required]
    public required string Town { get; set; }
    [Required]
    public required string PhoneNumber { get; set; }
}
