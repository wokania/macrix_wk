using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace macrix_wk_backend.Models;

public class PersonModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public required AddressModel Addres { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [ReadOnly(true)]//TODO - czy to mogę ustawić żeby jakoś automatycznie się wyliczało tutaj czy lepiej w metodzie???
    public int Age { get; set; }
}