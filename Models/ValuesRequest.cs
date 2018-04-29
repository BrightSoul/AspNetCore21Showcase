using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore21Showcase.Models
{
    public class ValuesRequest {
        [Required]
        public string Text { get; set; }
    }
}