using System.ComponentModel.DataAnnotations;
using IdentityWebApi.PL.Validation;
using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.Settings
{
    public class DbSettings : IValidatable
    {
        [Required]
        public string Host { get; set; }
        
        [DefaultValue]
        public int Port { get; set; }
        
        [Required]
        public string Instance { get; set; }
        
        [Required]
        public string User { get; set; }
        
        [Required]
        public string Password { get; set; }

        public string ConnectionString => $"Server=tcp:{Host},{Port};Database={Instance};User={User};Password={Password};MultipleActiveResultSets=True;";
        
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}