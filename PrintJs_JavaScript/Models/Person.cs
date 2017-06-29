namespace PrintJs_JavaScript.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Person
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsMarreid { get; set; }

        public int Age { get; set; }

        public int GenderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        public string Name { get; set; }

        public virtual Gender Gender { get; set; }
    }
}
