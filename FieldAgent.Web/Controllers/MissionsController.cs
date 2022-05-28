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
    public class MissionsController : ControllerBase
    {
        private readonly IMissionRepository _missionRepository;

        public MissionsController(IMissionRepository missionRepository)
        {
            _missionRepository = missionRepository;
        }

        [HttpGet("agency{id}")]
        public IActionResult GetAgencyMissions(int id)
        {
            var result = _missionRepository.GetByAgency(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [HttpGet("agent{id}")]
        public IActionResult GetAgentMissions(int id)
        {
            var result = _missionRepository.GetByAgent(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [HttpGet("{id}", Name = "GetMission")]
        public IActionResult GetMission(int id)
        {
            var result = _missionRepository.Get(id);

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
        public IActionResult AddMission(MissionModel missionModel)
        {
            if (ModelState.IsValid)
            {
                Mission m = new Mission
                {
                    AgencyId = missionModel.AgencyId,
                    CodeName = missionModel.CodeName,
                    Notes = missionModel.Notes,
                    StartDate = missionModel.StartDate,
                    ProjectedEndDate = missionModel.ProjectedEndDate,
                    ActualEndDate = missionModel.ActualEndDate,
                    OperationalCost = missionModel.OperationalCost
                };

                var result = _missionRepository.Insert(m);

                if (result.Success)
                {
                    return CreatedAtRoute(nameof(GetMission), new { id = m.MissionId }, m);
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
        public IActionResult EditMission(MissionModel missionModel)
        {
            if (ModelState.IsValid)
            {
                Mission mission = new Mission
                {
                    AgencyId = missionModel.AgencyId,
                    CodeName = missionModel.CodeName,
                    Notes = missionModel.Notes,
                    StartDate = missionModel.StartDate,
                    ProjectedEndDate = missionModel.ProjectedEndDate,
                    ActualEndDate = missionModel.ActualEndDate,
                    OperationalCost = missionModel.OperationalCost
                };
                if (!_missionRepository.Get(mission.MissionId).Success)
                {
                    return NotFound($"Mission {mission.MissionId} not found");
                }

                var result = _missionRepository.Update(mission);

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
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteMission(int id)
        {
            if (!_missionRepository.Get(id).Success)
            {
                return NotFound($"Mission {id} not found");
            }

            var result = _missionRepository.Delete(id);

            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("agent"), Authorize]
        public IActionResult AddMissionAgent(MissionAgent missionAgent)
        {
            MissionAgent m = new MissionAgent
            {
                MissionId = missionAgent.MissionId,
                AgentId = missionAgent.AgentId
            };

            var result = _missionRepository.AddAgent(missionAgent);

            if (result.Success)
            {
                return CreatedAtRoute(nameof(GetMission), new { id = m.MissionId }, m);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpDelete("{mId}/agent{aId}"), Authorize]
        public IActionResult DeleteMissionAgent(int mId, int aId)
        {
            if (!_missionRepository.Get(mId).Success)
            {
                return NotFound($"Mission {mId} not found");
            }

            var result = _missionRepository.DeleteAgent(mId, aId);

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
