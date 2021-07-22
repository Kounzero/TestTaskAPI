namespace EmployeesAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Subdivision")]
    public partial class Subdivision
    {
        public Subdivision()
        {
            Employee = new HashSet<Employee>();
            Subdivision1 = new HashSet<Subdivision>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "date")]
        public DateTime FormDate { get; set; }

        public string Description { get; set; }

        public int? ParentSubdivisionID { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }

        public virtual ICollection<Subdivision> Subdivision1 { get; set; }

        public virtual Subdivision ParentSubdivision { get; set; }
    }
}
