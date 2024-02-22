using System;
using System.Collections.Generic;

namespace BusinessObject.Modals;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleDesc { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
