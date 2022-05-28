using FieldAgent.Core;
using FieldAgent.Core.DTOs;
using FieldAgent.Core.Interfaces.DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
namespace FieldAgent.DAL
{
    public class ReportsRepository : IReportsRepository
    {
        public string ConnectionString { get; set; }

        public ReportsRepository(DBFactory dBFactory)
        {
            ConnectionString = dBFactory.GetConnection();
        }
        public Response<List<ClearanceAuditListItem>> AuditClearance(int securityClearanceId, int agencyId)
        {
            var response = new Response<List<ClearanceAuditListItem>>();
            response.Data = new List<ClearanceAuditListItem>();
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand("ClearanceAudit", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AgencyId", agencyId);
                cmd.Parameters.AddWithValue("@SecurityClearanceId", securityClearanceId);

                try
                {
                    connection.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            response.Data.Add(new ClearanceAuditListItem
                            {
                                BadgeId = new Guid(dr["BadgeId"].ToString()),
                                NameLastFirst = dr["NameLastFirst"].ToString(),
                                DateOfBirth = (DateTime)dr["DateOfBirth"],
                                ActivationDate = (DateTime)(dr["ActivationDate"]),
                                DeactivationDate = dr["DeactivationDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["DeactivationDate"]),
                            });
                            response.Success = true;
                        }
                    }
                    response.Success = true;
                }
                catch (Exception)
                {
                    response.Success = false;
                    response.Message = "Clearance Audit information unavailable.";
                }
            }
            return response;
        }

        public Response<List<PensionListItem>> GetPensionList(int agencyId)
        {
            var response = new Response<List<PensionListItem>>();
            response.Data = new List<PensionListItem>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand("PensionList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AgencyId", agencyId);

                try
                {
                    connection.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            response.Data.Add(new PensionListItem
                            {
                                BadgeId = (Guid)dr["BadgeId"],
                                NameLastFirst = dr["NameLastFirst"].ToString(),
                                DateOfBirth = (DateTime)dr["DateOfBirth"],
                                DeactivationDate = (DateTime)dr["DeactivationDate"],
                            });
                            response.Success = true;
                        }
                    }
                }
                catch (Exception)
                {
                    response.Success = false;
                    response.Message = "Pension List not found.";
                }
            }
            return response;
        }

        public Response<List<TopAgentListItem>> GetTopAgents()
        {
            var response = new Response<List<TopAgentListItem>>();
            response.Data = new List<TopAgentListItem>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand("TopAgents", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            response.Data.Add(new TopAgentListItem
                            {
                                NameLastFirst = dr["NameLastFirst"].ToString(),
                                DateOfBirth = (DateTime)dr["DateOfBirth"],
                                CompletedMissionCount = (int)dr["CompletedMissionCount"],
                            });
                            response.Success = true;
                        }
                    }
                    response.Success = true;
                }
                catch (Exception)
                {
                    response.Success = false;
                    response.Message = "Top Agents not found.";
                }
            }
            return response;
        }
    }
}
