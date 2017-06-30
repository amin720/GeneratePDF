namespace PdfReport_Iran.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? DepartmentId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Salary { get; set; }

        public virtual Person Person { get; set; }
    }
}
