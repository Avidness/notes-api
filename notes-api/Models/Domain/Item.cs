using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace notes_api.Models.Domain
{
    public class Item : Entity
    {
        [Required]
        [StringLength(128)]
        [Display(Name = "Name")]
        public string Label { get; set; }

        [StringLength(1024)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        public Category Category { get; set; }

        public int Ordering { get; set; }
    }
}