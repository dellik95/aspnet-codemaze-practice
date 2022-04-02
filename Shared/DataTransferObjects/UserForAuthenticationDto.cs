﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
	public record UserForAuthenticationDto
	{
		[Required(ErrorMessage = "UserName is required")]
		public string? UserName { get; init; }

		[Required(ErrorMessage = "Password is required")]
		public string? Password { get; init; }
	};
}