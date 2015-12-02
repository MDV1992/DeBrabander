using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeBrabander.Models
{
	public class User
	{
            [Key]
            public int UserID { get; set; }

            [Required(ErrorMessage ="Voornaam is verplicht")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Familienaam is verplicht")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Username is verplicht")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Email is verplicht")]
            //validator nog maken
            public string Email { get; set; }

            [Required(ErrorMessage = "Wachtwoord is verplicht")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Compare("Password", ErrorMessage ="Gelieve je wachtwoord te bevestigen")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
            public int Rights { get; set; }
        
    }
}