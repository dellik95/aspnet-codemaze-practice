using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service.Implementations
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly ILoggerManager _loggerManager;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly JwtOptions _options;
		private User _user;

		public AuthenticationService(ILoggerManager loggerManager, IMapper mapper, UserManager<User> userManager,
			IOptions<JwtOptions> options)
		{
			_loggerManager = loggerManager;
			_mapper = mapper;
			_userManager = userManager;
			_options = options.Value;
		}


		public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
		{
			var user = _mapper.Map<User>(userForRegistration);

			var createResult = await _userManager.CreateAsync(user, userForRegistration.Password);
			if (createResult.Succeeded)
			{
				await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
			}

			return createResult;
		}

		public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication)
		{
			_user = await _userManager.FindByNameAsync(userForAuthentication.UserName);
			var result = (_user != null &&
			              await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password));
			if (!result)
				_loggerManager.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
			return result;
		}


		public async Task<TokenDto> CreateToken(bool populateExp)
		{
			var signingCredentials = GetSigningCredentials();
			var claims = await GetClaims();
			var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

			var refreshToken = GenerateToken();
			_user.RefreshToken = refreshToken;
			if (populateExp)
			{
				_user.RefreshTokeExpireTime = DateTime.Now.AddDays(7);
			}

			await _userManager.UpdateAsync(_user);
			var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
			return new TokenDto(accessToken, refreshToken);
		}

		public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
		{
			var tokenPrincipal = GetClaimsPrincipalFromExpiredToken(tokenDto.AccessToken);
			var user = await _userManager.FindByNameAsync(tokenPrincipal.Identity.Name);
			if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
			    user.RefreshTokeExpireTime <= DateTime.Now)
				throw new RefreshTokenBadRequest();
			return await CreateToken(false);
		}

		private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
		{
			var tokenOptions = new JwtSecurityToken(
				issuer: _options.ValidIssuer,
				audience: _options.ValidAudience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_options.Expires)),
				signingCredentials: signingCredentials);
			return tokenOptions;
		}

		private async Task<List<Claim>> GetClaims()
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, _user.FirstName)
			};

			var userRoles = await _userManager.GetRolesAsync(_user);
			claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
			return claims;
		}

		private SigningCredentials GetSigningCredentials()
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
			return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		}


		private string GenerateToken()
		{
			var randomNumber = new byte[32];

			using var rnt = RandomNumberGenerator.Create();
			rnt.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
				ValidAudience = _options.ValidAudience,
				ValidIssuer = _options.ValidIssuer
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
			{
				throw new SecurityTokenException("Invalid Token");
			}

			return principal;
		}
	}
}