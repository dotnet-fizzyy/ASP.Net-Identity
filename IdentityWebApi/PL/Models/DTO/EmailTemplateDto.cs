using System;

namespace IdentityWebApi.PL.Models.DTO
{
    public class EmailTemplateDto
    {
        public Guid EmailTemplateId { get; set; }
        
        public string Name { get; set; }
        
        public string Layout { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}