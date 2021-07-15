using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Models
{
    public class Role
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
