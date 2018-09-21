using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace notes_api.Models.Domain
{
    public class Entity
    {
        [Key]
        public virtual int Id { get; set; }

        [DisplayName("Created At")]
        public virtual DateTime CreatedAt { get; set; }

        [DisplayName("Last Modified At")]
        public virtual DateTime LastModifiedAt { get; set; }
    }
}