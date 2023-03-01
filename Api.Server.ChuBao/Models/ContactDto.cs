using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Server.ChuBao.Models
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Complex { get; set; }
        public string Door { get; set; }
    }

    public class CreateContactDto
    {
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string Complex { get; set; }
        public string Door { get; set; }
    }

}
