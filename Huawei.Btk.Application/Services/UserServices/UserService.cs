using Huawei.Btk.Application.Services.UserServices.Dtos;
using Huawei.Btk.Core.Context;
using Huawei.Btk.Core.Models;
using Huawei.Btk.Core.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Huawei.Btk.Application.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<User> _userManager;

		public UserService(ApplicationDbContext dbContext, UserManager<User> userManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
		}

		public Task<ResponseDto<NoContentDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseDto<NoContentDto>> CreateUserAsync(UserRegisterDto registerDto)
		{
			throw new NotImplementedException();
		}

		public async Task<User> GetByIdAsync(Guid userId)
		{
			var user = await _dbContext.Users
				.Include(x => x.Allergies).Include(x => x.ActiveIngredients).Include(x => x.Analyses)
				.FirstOrDefaultAsync(x => x.Id == userId);

			return user;
		}

		public Task<ResponseDto<NoContentDto>> LoginAsync(LoginDto loginDto)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateAsync(UpdateUserDto updateUserDto, Guid userId)
		{
			var user = await GetByIdAsync(userId);
			user.Name = updateUserDto.Name;
			user.Surname = updateUserDto.Surname;
			user.PhoneNumber = updateUserDto.PhoneNumber;
			user.Age = updateUserDto.Age;
			user.Tall = updateUserDto.Tall;
			user.Weight = updateUserDto.Weight;
			user.DefaultLanguage = updateUserDto.DefaultLanguage;

			if (updateUserDto.Allergies == null)
			{
				user.Allergies = new List<Allergy>();
			} else
			{
				user.Allergies = updateUserDto.Allergies.Split(",").Select(x => new Allergy
				{
					UserId = userId,
					AllergyText = x.Trim(),
				}).ToList();
			}

			if (updateUserDto.ActiveIngredients == null)
			{
				user.ActiveIngredients = new List<ActiveIngredient>();
			} else
			{
				user.ActiveIngredients = updateUserDto.ActiveIngredients.Split(",").Select(x => new ActiveIngredient
				{
					UserId = userId,
					ActiveIngredientText = x.Trim()
				}).ToList();
			}

			await _userManager.UpdateAsync(user);
		}
	}
}
