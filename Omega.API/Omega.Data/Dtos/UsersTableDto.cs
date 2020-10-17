using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Infrastructure.Dtos
{
    public class UsersTableDto
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }

        public bool isAdmin { get; set; }
    }
}
