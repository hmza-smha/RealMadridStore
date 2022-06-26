using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Models.ViewModel
{
    public class ProductVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }
    }
}
