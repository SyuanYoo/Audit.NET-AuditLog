﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AuditLog.Models
{
    public partial class CustomerDemographics
    {
        public CustomerDemographics()
        {
            Customer = new HashSet<Customers>();
        }

        public string CustomerTypeId { get; set; }
        public string CustomerDesc { get; set; }

        public virtual ICollection<Customers> Customer { get; set; }
    }
}