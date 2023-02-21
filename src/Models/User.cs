using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigátorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Campo Obrigátorio")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo Obrigátorio")]
        public string Role { get; set; }
    }
}