using FieldAgent.Core;
using FieldAgent.Core.Entities;
using FieldAgent.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FieldAgent.DAL.CRUD
{
    public class MissionRepository : IMissionRepository
    {

        public DBFactory DbFac { get; set; }

        public MissionRepository(DBFactory dBFactory)
        {
            DbFac = dBFactory;
        }

        public Response Delete(int missionId)
        {
            var response = new Response<Mission>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    var missionAgents = (from ma in db.MissionAgent
                                         join m in db.Mission on ma.MissionId equals m.MissionId
                                         where m.MissionId == missionId
                                         select ma).ToList();
                    foreach (var missionAgent in missionAgents)
                    {
                        db.MissionAgent.Attach(missionAgent);
                        db.MissionAgent.Remove(missionAgent);
                        db.SaveChanges();
                    }
                    var mission = new Mission() { MissionId = missionId };
                    db.Mission.Attach(mission);
                    db.Mission.Remove(mission);
                    db.SaveChanges();
                    response.Success = true;
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"Mission was not deleted.";
            }
            return response;
        }

        public Response<Mission> Get(int missionId)
        {
            var response = new Response<Mission>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    var mission = db.Mission.Find(missionId);
                    if (mission != null)
                    {
                        response.Success = true;
                        response.Data = mission;
                        response.Message = $"{missionId} was found.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = $"Mission Id:{missionId} was not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }

        public Response<List<Mission>> GetByAgency(int agencyId)
        {
            var response = new Response<List<Mission>>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    var missions = db.Mission
                        .Where(a => a.AgencyId == agencyId)
                        .ToList();
                    if (missions != null)
                    {
                        response.Success = true;
                        response.Data = missions;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = $"No missions were found for Agency Id: {agencyId}.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;

        }

        public Response<List<Mission>> GetByAgent(int agentId)
        {
            var response = new Response<List<Mission>>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {

                    var missions = (from m in db.Mission
                                    join ma in db.MissionAgent on m.MissionId equals ma.MissionId
                                    join a in db.Agent on ma.AgentId equals a.AgentId
                                    where a.AgentId == agentId
                                    select m).ToList();
                    if (missions != null)
                    {
                        response.Success = true;
                        response.Data = missions;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = $"No missions were found for Agent Id: {agentId}.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }

        public Response<Mission> Insert(Mission mission)
        {
            var response = new Response<Mission>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    db.Mission.Add(mission);
                    db.SaveChanges();
                    response.Success = true;
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"Mission was not inserted.";
            }

            return response;
        }

        public Response Update(Mission mission)
        {
            var response = new Response<Mission>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    db.Mission.Update(mission);
                    db.SaveChanges();
                    response.Success = true;
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"Mission was not updated.";
            }

            return response;
        }

        public Response<MissionAgent> AddAgent(MissionAgent missionAgent)
        {
            var response = new Response<MissionAgent>();
            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    db.MissionAgent.Add(missionAgent);
                    db.SaveChanges();
                    response.Success = true;
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"MissionAgent was not inserted.";
            }
            return response;
        }

        public Response DeleteAgent(int missionId, int agentId)
        {
            var response = new Response<Mission>();

            try
            {
                using (var db = DbFac.GetDbContext())
                {
                    var missionAgent = (from ma in db.MissionAgent
                                         join m in db.Mission on ma.MissionId equals m.MissionId
                                         join a in db.Agent on ma.AgentId equals a.AgentId
                                         where m.MissionId == missionId
                                         where a.AgentId == agentId
                                         select ma).FirstOrDefault();
                    db.MissionAgent.Attach(missionAgent);
                    db.MissionAgent.Remove(missionAgent);
                    db.SaveChanges();

                    response.Success = true;
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"MissionAgent was not deleted.";
            }
            return response;
        }
    }
}
