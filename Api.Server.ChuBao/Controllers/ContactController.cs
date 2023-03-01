using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.IRepositories;
using Api.Server.ChuBao.Models;
using AutoMapper;
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
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ContactController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ContactController> logger)
        {
            this._work = unitOfWork;
            this._mapper = mapper;
            this._logger = logger;
        }


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

        [HttpGet]
        public async Task<ActionResult<ContactDto>> GetContact(Guid id)
        {
            try 
            { 
                var entity = await _work.Contacts.GetAsync(i => i.Id == id);
                if (entity == null)
                {
                    _logger.LogWarning($"not found entity -- {id}");
                    return NotFound();
                }
                var dto = _mapper.Map<ContactDto>(entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(GetContact)}");
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto model)
        {
            // 本案例不做检查重复值
            if (!ModelState.IsValid)
            {
                _logger.LogError($"model is invalid!--{nameof(CreateContact)}");
                return BadRequest(ModelState);
            }

            try
            {
                var entity = _mapper.Map<Contact>(model);
                entity.Id = Guid.NewGuid();
                await _work.Contacts.InsertAsync(entity);
                var result = await _work.CommitAsync();

                if (result > 0)
                {
                    _logger.LogInformation($"Insert {result} picks to database!");
                    return CreatedAtAction(nameof(GetContact),new {id = entity.Id},_mapper.Map<ContactDto>(entity));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something were wrong in the {nameof(CreateContact)}");
                return BadRequest("要添加的数据出现错误！");
            }
        }

        [HttpPut]
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

        [HttpDelete]
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
