using System;
using System.Collections.Generic;

namespace BusinessObject.Modals;

public partial class User
{
    public int UserId { get; set; }

    public string? EmailAddress { get; set; }

    public string? Password { get; set; }

    public string? Source { get; set; }

    public string? FristName { get; set; }

    public string? LastName { get; set; }

    public int? RoleId { get; set; }

    public int? PubId { get; set; }

    public DateTime? HireDate { get; set; }

    public virtual Publisher? Pub { get; set; }

    public virtual Role? Role { get; set; }
}
