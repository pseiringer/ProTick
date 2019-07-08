using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class ParentChildRelation
    {
        public int ParentChildRelationID { get; set; }

        public virtual Subprocess Parent { get; set; }
        public virtual Subprocess Child { get; set; }
    }
}
