using System.ComponentModel.DataAnnotations;

namespace Core.Server.ChuBao.DTOs;

public class CreateContactDto
{
    public string Name { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    public string Complex { get; set; }
    public string Door { get; set; }
}
