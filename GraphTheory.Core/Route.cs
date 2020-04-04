using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheory.Core {
    public class Route {
        public List<Node> Nodes;

        public Route() {
            this.Nodes = new List<Node>();
        }

        public Route(List<Node> nodes) {
            this.Nodes = new List<Node>();
            foreach (Node n in nodes) {
                this.Nodes.Add(n);
            }
        }
        public string ToString(string delimiter = "→") {
            if (this.Nodes.Count == 1)
                return this.Nodes[0].Name;
            return this.Nodes.Select(n => n.Name).ToList().Aggregate((a, b) => a + delimiter + b);
        }

        public bool IsEmpty() {
            return (this.Nodes.Count == 0);
        }

        public Node GetStartingNode() {
            if (this.Nodes.Count == 0)
                return null;
            return this.Nodes[0];
        }
        public Node GetDestinationNode() {
            if (this.Nodes.Count == 0)
                return null;
            return this.Nodes[this.Nodes.Count - 1];
        }

        public bool DoesAlreadyUseThisConnection(Node nodeA, Node nodeB) {
            // The length of the Route must be at least 2
            if (this.Nodes.Count < 2)
                return false;

            // Both nodes need to be a part of the node if the connection was already used
            if (!this.Nodes.Contains(nodeA) || !this.Nodes.Contains(nodeB))
                return false;

            // If the connection was already used, the index of toNode must be one higher than fromNode
            for (int i = 0, j = 1; j < this.Nodes.Count; i++, j++) {
                if ((this.Nodes[i] == nodeA) && (this.Nodes[j] == nodeB)) {
                    // Is it a two way connection?
                    if ((nodeA.IsDirectlyConnectedToNode(nodeB)) && (nodeB.IsDirectlyConnectedToNode(nodeA)))
                        // If so, then the connection was already used
                        return true;
                    else {
                        // one way connection
                        if (nodeA.IsDirectlyConnectedToNode(nodeB))
                            return true;
                    }
                } else if ((this.Nodes[j] == nodeA) && (this.Nodes[i] == nodeB)) {
                    // Check the other way around

                    // Is it a two way connection?
                    if ((nodeA.IsDirectlyConnectedToNode(nodeB)) && (nodeB.IsDirectlyConnectedToNode(nodeA)))
                        // If so, then the connection was already used
                        return true;
                    else {
                        // one way connection
                        if (nodeB.IsDirectlyConnectedToNode(nodeA))
                            return true;
                    }
                }
            }
            // Otherwise it was already used
            return false;
        }
    }
}
