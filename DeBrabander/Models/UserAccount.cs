using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeBrabander.Models
{
	public class UserAccount
	{
            [Key]
            public int UserID { get; set; }

            [DisplayName("Voornaam")]
            [Required(ErrorMessage ="Voornaam is verplicht")]
            public string FirstName { get; set; }
            [DisplayName("Naam")]
            [Required(ErrorMessage = "Naam is verplicht")]
            public string LastName { get; set; }
            [DisplayName("Username")]
            [Required(ErrorMessage = "Username is verplicht")]
            public string Username { get; set; }
            [DisplayName("Email")]
            [Required(ErrorMessage = "Email is verplicht")]
            //validator nog maken
            public string Email { get; set; }
            [StringLength(4000)]
            [DisplayName("Wachtwoord")]   
            [Required(ErrorMessage = "Wachtwoord is verplicht")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DisplayName("Bevestig je wachtwoord")]
            [Compare("Password", ErrorMessage ="Gelieve je wachtwoord te bevestigen")]
            [DataType(DataType.Password)]
            [NotMapped] //Zorgt ervoor dat deze kolom niet aangemaakt wordt in de tabel.
            public string ConfirmPassword { get; set; }
            public int Rights { get; set; }
        
    }
}