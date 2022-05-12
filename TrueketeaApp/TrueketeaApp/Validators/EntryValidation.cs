using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrueketeaAdmin.Services.Security;
using TrueketeaApp.AuthHelpers;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using Xamarin.Auth;

namespace TrueketeaApp.ViewModels
{
    public class EntryValidation 
    {

        private B8Clases B8 = new B8Clases();
        private readonly DBContext _dbContext = ViewModelLocator.sql;
        private readonly Encrypting Encrypt = new Encrypting();
        private int MinimumPasswordLength = 8;
       
        public string ValidationError { get; set; }
        



        public bool Email_validacion(string value)
        {
            var isMail = new Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (isMail.IsMatch(value))
            {
                return true;
            }

           return false;

        }


        public bool Verify_Email(string value)
        {

            string existe = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "Count(User_Email)", "User_Email", value,"Active","1","Rol_Id","0");

            if(string.IsNullOrEmpty(existe) == false)
            {
                return true;
            }

            return false;
            
        }

        public bool Verify_Pass(string value,string email)
        {
            string pass = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "Password", "User_Email", email, "Active", "1", "Rol_Id", "0");


            if (string.IsNullOrEmpty(pass) == false)
            {
                pass = Encrypt.Decrypt(pass, email);

                if(pass.Equals(value))
                {
                    return true;
                }
               
            }

            return false;
        }


        public bool Register_Validation (string Name, string Email, string Pass, string ValPass)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (string.IsNullOrEmpty(Email))
            {
                ValidationError = "El campo Email es obligatorio.";
                return false;
            }
            if (string.IsNullOrEmpty(Name))
            {
                ValidationError = "El campo Usuario es obligatorio.";
                return false;
            }
            if (string.IsNullOrEmpty(Pass))
            {
                ValidationError = "El campo Contraseña es obligatorio.";
                return false;
            }
            if (string.IsNullOrEmpty(ValPass))
            {
                ValidationError = "El campo Validar Contraseña es obligatorio.";
                return false;
            }

            if(this.Email_validacion(Email)== false)
            {
                ValidationError = "El formato del campo Email no es correcto.";
                return false;
            }

            if(Pass.Length < MinimumPasswordLength)
            {
                ValidationError = "La contraseña debe tener al  menos 8 caracteres.";
                return false;
            }
            if (!hasNumber.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener al  menos un caracter númerico.";
                return false;
            }
            if (!hasUpperChar.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener mayusculas.";
                return false;
            }
            if (!hasSymbols.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener caracteres alfanumericos.";
                return false;
            }

            if (!hasLowerChar.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener caracteres alfanumericos.";
                return false;
            }

            if (Pass.Equals(ValPass) == false)
            {
                ValidationError = "Las contraseñas no coinciden.";
                return false;
            }


            return true;
            
        }

        public bool Product_Validation(string Categoria, string Titulo, string ShotDesc, string Estado)
        {
           
            if (string.IsNullOrEmpty(Categoria))
            {
                ValidationError = "La Categoria es obligatorio.";
                return false;
            }
            if (string.IsNullOrEmpty(Titulo))
            {
                ValidationError = "El Nombre del Producto es obligatorio.";
                return false;
            }
            if (string.IsNullOrEmpty(ShotDesc))
            {
                ValidationError = "La Descripcion Corta es obligatorio.";
                return false;
            }
            if (string.IsNullOrEmpty(Estado))
            {
                ValidationError = "El Estado del producto es obligatorio.";
                return false;
            }


            return true;

        }

        public bool CheckPass(string Pass)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasLowerChar = new Regex(@"[a-z]+");


            if (string.IsNullOrEmpty(Pass))
            {
                ValidationError = "El campo Contraseña es obligatorio.";
                return false;
            }

            if (Pass.Length < MinimumPasswordLength)
            {
                ValidationError = "La contraseña debe tener al  menos 8 caracteres.";
                return false;
            }
            if (!hasNumber.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener al  menos un caracter númerico.";
                return false;
            }
            if (!hasUpperChar.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener mayusculas.";
                return false;
            }
            if (!hasSymbols.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener caracteres alfanumericos.";
                return false;
            }

            if (!hasLowerChar.IsMatch(Pass))
            {
                ValidationError = "La contraseña debe tener caracteres alfanumericos.";
                return false;
            }

            return true;
        }

            public bool Register_Profile( string Email)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (string.IsNullOrEmpty(Email))
            {
                ValidationError = "El campo Email es obligatorio.";
                return false;
            }
            

            if (this.Email_validacion(Email) == false)
            {
                ValidationError = "El formato del campo Email no es correcto.";
                return false;
            }

            return true;

        }

    }
}
