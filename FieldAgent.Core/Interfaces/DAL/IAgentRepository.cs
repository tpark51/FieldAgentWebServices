﻿using FieldAgent.Core.Entities;
using System.Collections.Generic;

namespace FieldAgent.Core.Interfaces
{
    public interface IAgentRepository
    {
        Response<Agent> Insert(Agent agent);
        Response Update(Agent agent);
        Response Delete(int agentId);
        Response<Agent> Get(int agentId);
        Response<List<Mission>> GetMissions(int agentId);
        Response<List<Agent>> GetAll();
    }
}
