using System;
using System.Collections.Generic;

namespace BusinessObject.DTOs;

public partial class UserDTO
{
    public int UserId { get; set; }

    public string? OldPassword { get; set; }

    public string? NewPassword { get; set; }

    public string? ComfirmPassword { get; set; }

}
