using System;
using System.Collections.Generic;

namespace BusinessObject.Modals;

public partial class Publisher
{
    public int PibId { get; set; }

    public string? PiblisherName { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
