using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Library.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Parent { get; set; }
    }
}