﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]"), Authorize]
    public class SubprocessController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public SubprocessController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }


        [HttpGet("{id}")]
        public SubprocessDTO GetSubprocess(int id)
        {
            return converter.SubprocessToDTO(dbm.FindSubprocessByID(id));
        }

        [HttpGet]
        public IEnumerable<SubprocessDTO> GetSubprocesses()
        {
            return dbm.FindAllSubprocesses(true).Select(x => converter.SubprocessToDTO(x)).ToList();
        }

        [HttpGet("{id}/Children")]
        public IEnumerable<SubprocessDTO> GetChildrenOfSubprocess(int id)
        {
            return dbm.FindAllChildrenBySubprocessID(id)
                .Select(x =>
                {
                    if (x != null) return converter.SubprocessToDTO(x);
                    return null;
                })
                .ToList();
        }

        [HttpGet("{id}/Tickets")]
        public IEnumerable<TicketDTO> GetTicketsOfSubprocess(int id)
        {
            return dbm.FindAllTicketsBySubprocessID(id)
                .Select(x =>
                {
                    if (x != null) return converter.TicketToDTO(x);
                    return null;
                })
                .ToList();
        }

        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public SubprocessDTO PostSubprocess([FromBody] SubprocessDTO s)
        {
            var a = db.Subprocess.Add(converter.DTOToSubprocess(s));

            Console.WriteLine(s.Description);

            db.SaveChanges();

            return converter.SubprocessToDTO(a.Entity);
        }

        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public SubprocessDTO PutSubprocess(int id, [FromBody] SubprocessDTO s)
        {
            var sp = dbm.FindSubprocessByID(id);

            bool changesMade = false;
            if (s.Description != null && s.Description != "" && sp.Description != s.Description)
            {
                sp.Description = s.Description;
                changesMade = true;
            }
            if (s.ProcessID > 0 && sp.Process.ProcessID != s.ProcessID)
            {
                sp.Process = dbm.FindProcessByID(s.ProcessID);
                changesMade = true;
            }
            if (s.TeamID > 0 && sp.Team.TeamID != s.TeamID)
            {
                sp.Team = dbm.FindTeamByID(s.TeamID);
                changesMade = true;
            }

            if (changesMade) db.SaveChanges();
            return converter.SubprocessToDTO(sp);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteSubprocess(int id)
        {
            var subprocess = dbm.FindSubprocessByID(id);

            db.Ticket.Where(y => y.Subprocess.SubprocessID == subprocess.SubprocessID)
                            .ToList()
                            .ForEach(ticket => db.Ticket.Remove(ticket));

            db.ParentChildRelation.Where(pcl => pcl.Child.SubprocessID == subprocess.SubprocessID || pcl.Parent.SubprocessID == subprocess.SubprocessID)
                .ToList()
                .ForEach(pcl => db.ParentChildRelation.Remove(pcl));

            db.Subprocess.Remove(subprocess);
            db.SaveChanges();
        }
    }
}