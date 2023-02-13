using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IUnitOfWork _work;
        private readonly ILogger _logger;

        public ContactController(
            IUnitOfWork unitOfWork,
            ILogger<ContactController> logger)
        {
            this._work = unitOfWork;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<Results<Ok<IList<Contact>>, NotFound>> GetContacts() 
        {
            try
            {
                var items =await _work.Contacts.GetAll();
                return TypedResults.Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(GetContacts)}");
                return TypedResults.NotFound();
            }
        }

    }
}
