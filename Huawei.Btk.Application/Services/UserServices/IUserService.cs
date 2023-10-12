using Huawei.Btk.Application.Services.UserServices.Dtos;
using Huawei.Btk.Core.Models;
using Huawei.Btk.Core.Response;

namespace Huawei.Btk.Application.Services.UserServices
{
	public interface IUserService
	{
		public Task<ResponseDto<NoContentDto>> CreateUserAsync(UserRegisterDto registerDto);
		public Task UpdateAsync(UpdateUserDto updateUserDto, Guid userId);
		public Task<ResponseDto<NoContentDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
		public Task<User> GetByIdAsync(Guid userId);
	}
}
