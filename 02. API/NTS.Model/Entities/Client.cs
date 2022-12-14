using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NTS.Model.Entities
{
    public class Client
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string SecretKey { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int ApplicationType { get; set; }
        public int Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
    }
}