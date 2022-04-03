using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class EmailTemplateDto
{
    public Guid EmailTemplateId { get; set; }

    public string Name { get; set; }

    public string Layout { get; set; }

    public DateTime CreationDate { get; set; }
}
