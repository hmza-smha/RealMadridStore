using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        public Product product { get; set; }

        public int productId { get; set; }

        public int Amount { get; set; }


        public string ShoppingCartId { get; set; }

    }
}
