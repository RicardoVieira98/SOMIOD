using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Library.Models
{
    public class Container
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Parent {  get; set; }
    }
}