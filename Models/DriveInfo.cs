using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore21Showcase.Models
{
    public class DriveInfo {
        public string Letter { get; set; }
        public int RemainingSpaceInMB { get; set; }
    }
}