using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFirstMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Kategori")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(500)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        
        public virtual ICollection<Project> Projects { get; set; }
    }
}