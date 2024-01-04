using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SOMIOD.Library.Models
{
    [XmlRoot(ElementName = "Application")]
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}