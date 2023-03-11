using AutoMapper;
using Core.Server.ChuBao.DTOs;
using Data.Server.Chubao.Entities;
using Data.Server.Chubao.Repositories;
using Data.Server.ChuBao.Entities;
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
        public async Task<ActionResult<List<ContactDto>>> GetContacts()
        {
            var entities = await _work.Contacts.GetAllAsync();
            var dtos = _mapper.Map<List<ContactDto>>(entities);
            return Ok(dtos);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact([FromRoute] Guid id)
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

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactCreateDto model)
        {
            // 本案例不做检查重复值
            if (!ModelState.IsValid)
            {
                _logger.LogError($"model is invalid!--{nameof(CreateContact)}");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<Contact>(model);
            entity.Id = Guid.NewGuid();
            await _work.Contacts.InsertAsync(entity);
            var result = await _work.CommitAsync();

            if (result > 0)
            {
                _logger.LogInformation($"Insert {result} picks to database!");
                return CreatedAtAction(nameof(GetContact), new { id = entity.Id }, _mapper.Map<ContactDto>(entity));
            }
            return BadRequest();

        }

        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromBody] ContactDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"model is invalid!--{nameof(UpdateContact)}");
                return BadRequest(ModelState);
            }

            var entity = await _work.Contacts.GetAsync(q => q.Id == model.Id);
            if (entity == null)
            {
                return BadRequest("所要操作的对象没有找到！");
            }
            entity = _mapper.Map<Contact>(model);
            _work.Contacts.Update(entity);

            var result = await _work.CommitAsync();
            if (result > 0)
            {
                _logger.LogInformation($"Updated contact: {model.Id}");
                return AcceptedAtAction(nameof(GetContact), new { id = model.Id }, _mapper.Map<ContactDto>(entity));
            }
            _logger.LogWarning("更新失败。");
            return BadRequest("更新失败。");

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContact([FromQuery] Guid id)
        {
            var entity = await _work.Contacts.GetAsync(q => q.Id == id);
            if (entity == null)
            {
                return BadRequest("所要操作的对象没有找到！");
            }
            _work.Contacts.Delete(entity);
            var result = await _work.CommitAsync();
            if (result > 0)
            {
                _logger.LogInformation($"Deleted contact: {id}");
                return NoContent();
            }
            _logger.LogWarning("删除失败。");
            return BadRequest("删除失败。");

        }

        // Contact Records


        [HttpPost]
        public async Task<IActionResult> GetContactRecords([FromBody] Guid contactId)
        {
            var entities = await _work.Records.GetAllAsync(x => x.ContactId == contactId);

            var dtos = _mapper.Map<List<RecordDto>>(entities);

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddContactRecord([FromBody] RecordCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = _mapper.Map<Record>(dto);
            entity.Id = Guid.NewGuid();
            entity.AddTime = DateTime.Now;

            await _work.Records.InsertAsync(entity);
            var result = await _work.CommitAsync();
            if (result > 0)
            {
                return Ok();
            }

            return NoContent();
        }

        // Contact Marks
        // 每个联系人创建的时候就有标签
        // 标签只能修改

        [HttpPost]
        public async Task<IActionResult> GetContactMark([FromBody] Guid contactId)
        {
            if (string.IsNullOrEmpty(contactId.ToString()))
            {
                _logger.LogWarning($"传入了一个空值");
                return BadRequest("传入空值"); 
            }

            var entity = await _work.Marks.GetAsync(x => x.ContactId == contactId);
            if (entity == null)
            {
                _logger.LogWarning("没有找到对应的联系人");
                return BadRequest();
            }
            var dto = _mapper.Map<MarkDto>(entity);

            return Ok(dto);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateContactMark([FromBody] MarkDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("传入模型失败");
                return BadRequest();
            }

            var entity = await _work.Marks.GetAsync(x => x.Id == model.Id);
            if (entity == null)
            {
                _logger.LogError("找不到这条信息");
                return NotFound();
            }
            entity = _mapper.Map<Mark>(model);
            _work.Marks.Update(entity);
            var result = await _work.CommitAsync();

            if (result > 0)
            {
                return Accepted(model);
            }
            return Problem("服务器问题");
        }

    }
}
