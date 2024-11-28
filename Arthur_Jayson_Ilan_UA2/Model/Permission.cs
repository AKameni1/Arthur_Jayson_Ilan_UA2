using System;
using System.Collections.Generic;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
