using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheory.Core {
    public class Connection {

        public Node FromNode, ToNode;

        public Connection(Node fromNode, Node toNode) {
            this.FromNode = fromNode;
            this.ToNode = toNode;
        }

        public bool IsOneWay() {
            return !this.IsTwoWay();
        }

        public bool IsTwoWay() {
            return ToNode.Connections.Contains(ToNode.Connections.FirstOrDefault(c => c.ToNode == this.FromNode));
        }
    }
}