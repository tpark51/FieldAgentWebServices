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
    public class AgentsController : ControllerBase
    {
        private readonly IAgentRepository _agentRepository;

        public AgentsController(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        [HttpGet]
        public IActionResult GetAgents()
        {
            var result = _agentRepository.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [HttpGet("{id}", Name = "GetAgent")]
        public IActionResult GetAgent(int id)
        {
            var result = _agentRepository.Get(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("{id}/missions", Name = "AgentMissions")]
        public IActionResult GetMissions(int id)
        {
            var result = _agentRepository.GetMissions(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [HttpPost, Authorize]
        public IActionResult AddAgent(AgentModel agentModel)
        {
            if (ModelState.IsValid)
            {
                Agent a = new Agent
                {
                    FirstName = agentModel.FirstName,
                    LastName = agentModel.LastName,
                    DateOfBirth = agentModel.DateOfBirth,
                    Height = agentModel.Height
                };

                var result = _agentRepository.Insert(a);

                if (result.Success)
                {
                    return CreatedAtRoute(nameof(GetAgent), new { id = a.AgentId }, a);
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
        public IActionResult EditAgent(AgentModel agentModel)
        {
            if (ModelState.IsValid && agentModel.AgentId > 0)
            {
                Agent agent = new Agent
                {
                    AgentId = agentModel.AgentId,
                    FirstName = agentModel.FirstName,
                    LastName = agentModel.LastName,
                    DateOfBirth = agentModel.DateOfBirth,
                    Height = agentModel.Height
                };

                if (!_agentRepository.Get(agent.AgentId).Success)
                {
                    return NotFound($"Agent {agent.AgentId} not found");
                }

                var result = _agentRepository.Update(agent);

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
                if (agentModel.AgentId < 1)
                    ModelState.AddModelError("agentId", "Invalid Agent ID");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteAgent(int id)
        {
            if (!_agentRepository.Get(id).Success)
            {
                return NotFound($"Agent {id} not found");
            }

            var result = _agentRepository.Delete(id);

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
