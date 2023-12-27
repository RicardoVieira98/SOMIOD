using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SOMIOD.Library.Models
{
    [XmlRoot(ElementName = "Application")]
    public class Application
    {
        public int Id { get; set; }
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}