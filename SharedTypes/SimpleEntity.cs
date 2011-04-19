using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedTypes
{
    public class SimpleEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            if ((obj as SimpleEntity) == null) return false;
            return ID == (obj as SimpleEntity).ID;
        }

        public override string ToString()
        {
            return "se: " + Name;
        }
    }
}
