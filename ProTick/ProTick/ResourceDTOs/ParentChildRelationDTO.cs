using System;
using System.Collections.Generic;
using System.Text;

namespace ProTick.ResourceDTOs
{
    public class ParentChildRelationDTO
    {
        public int ParentChildRelationID { get; set; }
        public int ParentID { get; set; }
        public int ChildID { get; set; }
    }
}
