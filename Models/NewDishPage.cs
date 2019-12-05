using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace crudDelicious.Models
{
    [NotMapped]
    public class NewDishPage
    {
        public List<Chef> AllChefs;
        public Dish NewDish { get; set; }
        public int SelectedChefId { get; set; }
    }
}