using System.ComponentModel.DataAnnotations;

namespace Core.Server.ChuBao.DTOs
{
    public class UserLoginDto
    {
        [Required]
        [StringLength(maximumLength: 15, ErrorMessage = "请输入{2}至{1}位密码", MinimumLength = 1)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 15, ErrorMessage = "请输入{2}至{1}位密码", MinimumLength = 6)]
        public string Password { get; set; }
    }

}
