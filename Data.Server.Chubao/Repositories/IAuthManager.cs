using Core.Server.ChuBao.DTOs;
using System.Threading.Tasks;

namespace Data.Server.Chubao.Repositories
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserLoginDto model);
        Task<string> CreateToken();
    }
}
