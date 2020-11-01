using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        public bool Completed { get; set; }
    }
}
