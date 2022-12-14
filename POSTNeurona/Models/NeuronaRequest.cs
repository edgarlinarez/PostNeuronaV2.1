using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POSTNeurona.Models
{
    [AttributeUsage(AttributeTargets.Property,
        Inherited = false,
        AllowMultiple = false)]
    internal sealed class OptionalAttribute : Attribute { }

    public class NeuronaRequest
    {
        [Required]
        public string Url { set; get; }
        [Required]
        public string MethodApi { set; get; }
        [Optional]
        public string SubMethodApi { set; get; }
        [Required]
        public string Method { set; get; }
        [Optional]
        public string Body { get; set; }
    }
}