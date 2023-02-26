using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Server.ChuBao.Models
{
    public class UserDto
    {
        [Required]
        [StringLength(maximumLength: 15, ErrorMessage = "请输入{2}至{1}位密码", MinimumLength = 1)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength:15,ErrorMessage = "请输入{2}至{1}位密码", MinimumLength =6)]
        public string Password { get; set; }

    }

    public class LoginUserDto : UserDto
    {

    }
    public class RegisterUserDto
    {
        //[Required]
        //[DataType(DataType.Password)]
        //[StringLength(maximumLength: 15, ErrorMessage = "请输入{2}至{1}位密码", MinimumLength = 6)]
        //public string ComfirmPassword { get; set; }

        [Required]
        [StringLength(maximumLength: 15, ErrorMessage = "请输入{2}至{1}位密码", MinimumLength = 1)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 15, ErrorMessage = "请输入{2}至{1}位密码", MinimumLength = 6)]
        public string Password { get; set; }

        public ICollection<string> Roles { get; set; }
    }

}
