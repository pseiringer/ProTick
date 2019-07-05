using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class ParentChildRelation
    {
        public int ParentChildRelationID { get; set; }

        public virtual Subprocess ParentID { get; set; }
        public virtual Subprocess ChildID { get; set; }
    }
}
