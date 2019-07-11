﻿using ProTick.ResourceDTOs;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProTickDatabase;


namespace ProTick.Singletons
{
    public interface IResourceDTOConverter
    {

        #region ------ DB to DTO

        AddressDTO AddressToDTO(Address a);

        EmployeeDTO EmployeeToDTO(Employee a);

        EmployeeTeamDTO EmployeeTeamToDTO(EmployeeTeam a);

        EmployeeTeamPrivilegeDTO EmployeeTeamPrivilegeToDTO(EmployeeTeamPrivilege a);

        ParentChildRelationDTO ParentChildRelationToDTO(ParentChildRelation a);

        PrivilegeDTO PrivilegeToDTO(Privilege a);

        ProcessDTO ProcessToDTO(Process a);

        StateDTO StateToDTO(State a);

        SubprocessDTO SubprocessToDTO(Subprocess a);

        TeamDTO TeamToDTO(Team a);

        TicketDTO TicketToDTO(Ticket a);

        #endregion

        #region ------ DTO to DB

        Address DTOToAddress(AddressDTO a);

        Employee DTOToEmployee(EmployeeDTO a);

        EmployeeTeam DTOToEmployeeTeam(EmployeeTeamDTO a);

        EmployeeTeamPrivilege DTOToEmployeeTeamPrivilege(EmployeeTeamPrivilegeDTO a);

        ParentChildRelation DTOToParentChildRelation(ParentChildRelationDTO a);

        Privilege DTOToPrivilege(PrivilegeDTO a);

        Process DTOToProcess(ProcessDTO a);

        State DTOToState(StateDTO a);

        Subprocess DTOToSubprocess(SubprocessDTO a);

        Team DTOToTeam(TeamDTO a);

        Ticket DTOToTicket(TicketDTO a); 
        

        #endregion
    }
}