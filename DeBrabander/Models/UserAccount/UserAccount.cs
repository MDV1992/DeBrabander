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
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }
        [DisplayName("Naam")]
        [Required(ErrorMessage = "Naam is verplicht")]
        public string LastName { get; set; }
        [DisplayName("Gebruikersnaam")]
        [Required(ErrorMessage = "Username is verplicht")]
        public string Username { get; set; }
        [DisplayName("E-mail")]
        [Required(ErrorMessage = "Email is verplicht")]
        //validator nog maken
        public string Email { get; set; }
        [StringLength(4000)]
        [DisplayName("Wachtwoord")]
        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Bevestig je wachtwoord")]
        [Compare("Password", ErrorMessage = "Gelieve je wachtwoord te bevestigen")]
        [DataType(DataType.Password)]
        [NotMapped] //Zorgt ervoor dat deze kolom niet aangemaakt wordt in de tabel.
        public string ConfirmPassword { get; set; }
        [Column("Role")]
        public string RoleString
        {
            get { return Role.ToString(); }
            private set { Role = EnumExtensions.ParseEnum<Roles>(value); }
        }
        [DisplayName("Rechten")]
        [Required(ErrorMessage = "Welke rechten wil je de gebruiker toewijzen?")]
        [NotMapped]
        public Roles Role { get; set; }
        public enum Roles
        {
            SuperAdmin = 1,
            Admin = 2,
            User = 3
        }
    }

    public class EnumExtensions
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}