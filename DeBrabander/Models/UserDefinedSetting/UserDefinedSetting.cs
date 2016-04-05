using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class UserDefinedSetting
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Index aantal")]
        public int IndexResultLength { get; set; }
        [DisplayName("Details aantal")]
        public int DetailsResultLength { get; set; }
        [DisplayName("VervaldatumLengte")]
        public int ExpirationDateLenght { get; set; }
        
    
    }

    }

