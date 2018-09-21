using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace notes_api.Models.Domain
{
    public class Category : Entity
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Label { get; set; }
    }
}