using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crudDelicious.Models
{
    public class Dish
    {

        [Key] // Primary Key
        public int DishId { get; set; }

        // Foreign Key 
        public int ChefId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 2 characters.")]
        public string Name { get; set; }

        [Range(1, 5)]
        public int Tastiness { get; set; }

        [Range(1, 5000)]
        public int Calories { get; set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property for related User object
        public Chef Creator { get; set; }

    }
}