using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class ParentChildRelation
    {
        public int ParentChildRelationID { get; set; }

        [ForeignKey("ParentID")]
        public virtual Subprocess Parent { get; set; }
        [ForeignKey("ChildID")]
        public virtual Subprocess Child { get; set; }
    }
}
