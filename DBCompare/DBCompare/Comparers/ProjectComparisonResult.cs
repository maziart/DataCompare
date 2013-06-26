using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.Config;

namespace DBCompare.Comparers
{
    class ProjectComparisonResult
    {
        public ProjectComparisonResult()
        {
            Tables = new List<TableComparisonResult>();
        }
        public List<TableComparisonResult> Tables { get; private set; }
        public void Add(TableComparisonResult result)
        {
            Tables.Add(result);
        }
        public bool SourcesAreEqual()
        {
            return Tables.All(n => n.Changes.Count == 0);
        }
        public override string ToString()
        {
            var changed = Tables.Where(n => n.Changes.Count != 0).ToList();
            if (changed.Count == 0)
                return "<Identical>";
            return string.Join(",\r\n", changed);
        }
    }
}
