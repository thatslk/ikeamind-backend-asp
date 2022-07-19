﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikeamind_backend.Core.Models.EFModels
{
    [Table("IkeaSE")]
    [Index(nameof(Id), IsUnique = true)]
    public partial class IkeaProductSE : AbstractIkeaProduct
    { }
}
