using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Credipaz.Comercio.Shared.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "CUIT")]
        [StringLength(100, ErrorMessage = "Debe Ingresar los 11 Dígitos Numéricos", MinimumLength = 11)]
        public string CUIT { get; set; }

        [Required]
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "Debe Ingresar hasta 50 carcteres validos", MinimumLength = 50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "La {0} Debe contener al menos {2} caracteres válidos.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
