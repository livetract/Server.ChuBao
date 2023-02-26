using Api.Server.ChuBao.Models;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserDto userDto);
        Task<string> CreateToken();
    }
}
