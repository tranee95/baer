using System;
using Microsoft.AspNetCore.Identity;

namespace Api.Model
{
	public class User : IdentityUser
	{
		public bool Active { get; set; }

		public string UserDisplay { get; set; }

		public int GroupId { get; set; }

		public int PersonId { get; set; }
		public byte[] Avatar { get; set; }
		public DateTime? BirthDate { get; set; }
		public int? Sex { get; set; }
	}
}
