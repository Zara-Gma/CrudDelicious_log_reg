using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crudDelicious.Models
{
    public class Chef
    {

        [Key] // denotes PK, not needed if named ModelNameId
        public int ChefId { get; set; }

        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [MinAge(18)] // 18 is the parameter of constructor. 
        public DateTime Dob { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        [Required]
        [MinLength(5, ErrorMessage = "Must be more than 5 characters.")]
        public string Biography { get; set; }

        public int age
        {
            get
            {

                DateTime today = DateTime.Today;
                int age = today.Year - Dob.Year;
                if (Dob.Date > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property for the relationship
        public List<Dish> Dishes { get; set; } // 1 Chef : M dishes relationship
    }
}
