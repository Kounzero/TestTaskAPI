﻿using AutoMapper;
using EmployeesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Models.Dtos.Subdivisions
{
    [AutoMap(typeof(Subdivision))]
    public class SubdivisionDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime FormDate { get; set; }
        public string Description { get; set; }
        public int? ParentSubdivisionID { get; set; }
        public bool HasChildren { get; set; }
    }
}
