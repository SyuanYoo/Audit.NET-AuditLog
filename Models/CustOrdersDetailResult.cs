﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditLog.Models
{
    public partial class CustOrdersDetailResult
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public int? Discount { get; set; }
        public decimal? ExtendedPrice { get; set; }
    }
}