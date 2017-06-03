using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatsDown.Core.Domain
{
    public class Test
    {
        [Key]
        public int Id
        {
            get; set;
        }
        
        public string Title
        {
            get; set;
        }
    }
}