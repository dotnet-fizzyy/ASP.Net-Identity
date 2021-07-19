using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Constants;
using IdentityWebApi.PL.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApi.PL.Controllers
{
    [ApiController]
    [Authorize(Roles = UserRoleConstants.Admin)]
    [Route("api/email-template")]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet("id/{id:guid}")]
        public async Task<ActionResult<EmailTemplateDto>> GetEmailTemplate(Guid id)
        {
            var emailTemplateResult = await _emailTemplateService.GetEmailTemplateDtoAsync(id);

            if (emailTemplateResult.Result is ServiceResultType.NotFound)
            {
                return NotFound();
            }
            
            return emailTemplateResult.Data;
        }
    }
}