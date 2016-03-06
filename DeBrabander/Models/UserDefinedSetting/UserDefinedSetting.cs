using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class UserDefinedSetting
    {
        [Key]
        public int Id { get; set; }
        public int IndexResultLength { get; set; }
        public int DetailsResultLength { get; set; }
        
    
    }

    }

