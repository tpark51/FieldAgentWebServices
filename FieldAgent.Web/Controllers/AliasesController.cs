using FieldAgent.Core.Entities;
using FieldAgent.Core.Interfaces;
using FieldAgent.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FieldAgent.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AliasesController : ControllerBase
    {
        private readonly IAliasRepository _aliasRepository;

        public AliasesController(IAliasRepository aliasRepository)
        {
            _aliasRepository = aliasRepository;
        }

        [HttpGet("agent{id}")]
        public IActionResult GetAliases(int id)
        {
            var result = _aliasRepository.GetByAgent(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [HttpGet("{id}", Name = "GetAlias")]
        public IActionResult GetAlias(int id)
        {
            var result = _aliasRepository.Get(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost, Authorize]
        public IActionResult AddAlias(AliasModel aliasModel)
        {
            if (ModelState.IsValid)
            {
                Alias a = new Alias
                {
                    AgentId = aliasModel.AgentId,
                    AliasName = aliasModel.AliasName,
                    InterpolId = aliasModel.InterpolId,
                    Persona = aliasModel.Persona

                };

                var result = _aliasRepository.Insert(a);

                if (result.Success)
                {
                    return CreatedAtRoute(nameof(GetAlias), new { id = a.AliasId }, a);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut, Authorize]
        public IActionResult EditAlias(AliasModel aliasModel)
        {
            if (ModelState.IsValid && aliasModel.AliasId > 0)
            {
                Alias alias = new Alias
                {
                    AliasId = aliasModel.AliasId,
                    AgentId = aliasModel.AgentId,
                    AliasName = aliasModel.AliasName,
                    InterpolId = aliasModel.InterpolId,
                    Persona = aliasModel.Persona

                };
                if (!_aliasRepository.Get(alias.AliasId).Success)
                {
                    return NotFound($"Alias {alias.AliasId} not found");
                }

                var result = _aliasRepository.Update(alias);

                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            else
            {
                if (aliasModel.AliasId < 1)
                    ModelState.AddModelError("aliasId", "Invalid Alias ID"); 
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteAlias(int id)
        {
            if (!_aliasRepository.Get(id).Success)
            {
                return NotFound($"Alias {id} not found");
            }

            var result = _aliasRepository.Delete(id);

            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}

