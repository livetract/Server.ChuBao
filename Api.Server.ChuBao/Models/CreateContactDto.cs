using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Server.ChuBao.Models
{
    public class CreateContactDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength:12, ErrorMessage = "Phone number is too lang!",
            MinimumLength =11, ErrorMessageResourceName = "Phone number is too short!")]
        public string Telephone { get; set; }
        public string Complex { get; set; }
        public string Doorplate { get; set; }
    }
}
