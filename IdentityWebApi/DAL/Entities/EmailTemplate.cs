using System;

namespace IdentityWebApi.DAL.Entities
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Layout { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}