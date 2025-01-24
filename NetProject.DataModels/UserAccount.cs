using System;
using System.Collections.Generic;

namespace NetProject.DataModels;

public partial class UserAccount
{
    public long Id { get; set; }

    public long? RoleId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool IsDelete { get; set; }
}
