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
            public int ID { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }

            public int rights { get; set; }
        
    }
}