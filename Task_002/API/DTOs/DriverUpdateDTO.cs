using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
public class DriverUpdateDTO
{
    [Required(ErrorMessage = "Driver Id name is required.")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Driver name is required.")]
    public string Name { get; set; }
}