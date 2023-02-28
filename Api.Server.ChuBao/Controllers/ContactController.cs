using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.IRepositories;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<Results<Ok<IList<Contact>>, NotFound>> GetContacts() 
        {
            try
            {
                var items =await _work.Contacts.GetAllAsync();
                return TypedResults.Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(GetContacts)}");
                return TypedResults.NotFound();
            }
        }

        [HttpGet("[action]")]
        public async Task<Results<Ok<Contact>, NotFound>> GetContact(Guid id)
        {
            try 
            { 
                var item = await _work.Contacts.GetAsync(i => i.Id == id);
                return item == null ?  TypedResults.NotFound():TypedResults.Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(GetContact)}");
                return TypedResults.NotFound();
            }

        }

        [HttpPost("[action]")]
        public async Task<Results<Created<Contact>, BadRequest<string>>> CreateContact(Contact contact)
        {
            try
            {
                var item = await _work.Contacts.GetAsync(q => q.Id == contact.Id);
                if (item != null) return TypedResults.BadRequest("数据重复！");

                await _work.Contacts.InsertAsync(contact);
                var result = await _work.CommitAsync();
                if (result > 0)
                {
                    _logger.LogInformation($"Insert {result} picks to database!");
                }
                return TypedResults.Created(Url.Action(nameof(GetContact), contact.Id), contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(GetContact)}");
                return TypedResults.BadRequest("要添加的数据出现错误！");
            }
        }

        [HttpPut("[action]")]
        public async Task<Results<Ok<Contact>, BadRequest<string>>> UpdateContact(Contact contact)
        {
            try
            {
                var item = await _work.Contacts.GetAsync(q => q.Id == contact.Id);
                if (item == null)
                {
                    return TypedResults.BadRequest("所要操作的对象没有找到！");
                }
                _work.Contacts.Update(contact);
                var result = await _work.CommitAsync();
                if (result > 0)
                {
                    _logger.LogInformation($"Updated contact: {contact.Id}");
                    return TypedResults.Ok(contact);
                }
                _logger.LogWarning("更新失败。");
                return TypedResults.BadRequest("更新失败。");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(UpdateContact)}");
                return TypedResults.BadRequest("更新数据错误！");
            }
        }

        [HttpDelete("[action]")]
        public async Task<Results<Ok<string>, BadRequest<string>>> DeleteContact(Contact contact)
        {
            try
            {
                var item = await _work.Contacts.GetAsync(q => q.Id == contact.Id);
                if (item == null)
                {
                    return TypedResults.BadRequest("所要操作的对象没有找到！");
                }
                _work.Contacts.Delete(contact);
                var result = await _work.CommitAsync();
                if (result > 0)
                {
                    _logger.LogInformation($"Deleted contact: {contact.Id}");
                    return TypedResults.Ok("删除成功。");
                }
                _logger.LogWarning("删除失败。");
                return TypedResults.BadRequest("删除失败。");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(UpdateContact)}");
                return TypedResults.BadRequest("删除数据错误！");
            }
        }
    }
}
