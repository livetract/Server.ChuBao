using System;

namespace Core.Server.ChuBao.DTOs;

public class ContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Complex { get; set; }
    public string Door { get; set; }
}
