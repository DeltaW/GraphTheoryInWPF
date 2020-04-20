using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheory.Core {
    public class Graph {

        // The graph's data
        private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

        public static void SaveGraphAsFile(Graph graph, string path) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.Write(Graph.SaveGraphAsString(graph));
            }
        }

        public static Graph LoadGraphFromFile(string path) {
            using (StreamReader sr = new StreamReader(path)) {
                return Graph.LoadGraphFromString(sr.ReadToEnd());
            }
        }

        public static Graph LoadGraphFromString(string graphData) {
            try {
                Graph output = new Graph();
                string[] GraphNodesData = graphData.Split('}');

                // Add each node
                for (int i = 0; i < GraphNodesData.Length - 1; i++) {
                    string[] lines = GraphNodesData[i].Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string name = lines[1].Split('\"')[1];
                    int x = int.Parse(lines[3].Split('\"')[1]);
                    int y = int.Parse(lines[4].Split('\"')[1]);
                    output.AddNewNodeToGraph(name, new Point(x, y));
                }

                // Add the connections
                for (int i = 0; i < GraphNodesData.Length - 1; i++) {
                    string[] lines = GraphNodesData[i].Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string fromNodeName = lines[1].Split('\"')[1];
                    for (int j = 7/*3*/; j < lines.Length - 1; j++) {
                        string toNodeName = lines[j].Split('\"')[1];
                        output.AddOneWayNodeConnetionToGraph(fromNodeName, toNodeName);
                    }
                }
                return output;
            } catch (Exception) {
                throw new GraphException("INVALID OR CORRUPT DATA!");
            }
        }

        public static string SaveGraphAsString(Graph graph) {
            string output = "";
            foreach (KeyValuePair<string, Node> pair in graph._nodes) {
                output += "{\n";
                output += $"\tName: \"{pair.Key}\"\n";
                output += $"\tPosition: [\n";     // 2
                if (pair.Value.Position == null) {
                    output += $"\t\tX: \"null\"\n"; // 3
                    output += $"\t\tY: \"null\"\n"; // 4
                } else {
                    output += $"\t\tX: \"{pair.Value.Position.X}\"\n";
                    output += $"\t\tY: \"{pair.Value.Position.Y}\"\n";
                }
                output += $"\t]\n";
                output += $"\tConnections: [\n";
                foreach (Connection connection in pair.Value.Connections) {
                    string name = connection.ToNode.Name;
                    output += $"\t\tName: \"{name}\"\n";
                }
                output += "\t]\n}\n";
            }
            return output;
        }

        public Node GetRandomNode() {
            if (this._nodes.Count == 0) {
                return null;
            }
            return this._nodes[this.GetAllNodeNames().ToList()[new Random().Next(0, this._nodes.Count - 1)]];
        }

        public IEnumerable<string> GetAllNodeNames() {
            return this._nodes.Select(x => x.Key);
        }

        public Node GetNode(string uniqueName) {
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            return this._nodes[uniqueName];
        }

        public void AddExistingNodeToGraph(string uniqueName, Node node) {
            // It needs to be a unique name
            if (this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a unique name!");

            // Adding the node to the graph
            this._nodes[uniqueName] = node;
        }

        public void AddNewNodeToGraph(string uniqueName) {
            // It needs to be a unique name
            if (this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a unique name!");

            // Adding a new node to the graph
            this._nodes[uniqueName] = new Node(uniqueName);
        }

        public void AddNewNodeToGraph(string uniqueName, Point position) {
            // It needs to be a unique name
            if (this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a unique name!");

            // Adding a new node to the graph
            this._nodes[uniqueName] = new Node(uniqueName, position);
        }

        public void AddOneWayNodeConnetionToGraph(string fromNodeName, string toNodeName) {
            // fromNodeName needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(fromNodeName))
                throw new GraphException($"\"{fromNodeName}\" is not a valid key!");

            // toNodeName needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(toNodeName))
                throw new GraphException($"\"{toNodeName}\" is not a valid key!");

            // Can not add a connection that already exists
            if (this._nodes[fromNodeName].IsDirectlyConnectedToNode(this._nodes[toNodeName]))
                throw new GraphException($"\"{fromNodeName}\" is already connceted to \"{toNodeName}\"!");

            // Creating a one way node connection
            this._nodes[fromNodeName].AddConnectionToNode(this._nodes[toNodeName]);
        }

        public void AddTwoWayNodeConnetionToGraph(string nodeNameA, string nodeNameB) {
            // nodeNameA needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(nodeNameA))
                throw new GraphException($"\"{nodeNameA}\" is not a valid key!");

            // nodeNameB needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(nodeNameB))
                throw new GraphException($"\"{nodeNameB}\" is not a valid key!");

            // Can not add a connection that already exists
            if (this._nodes[nodeNameA].IsDirectlyConnectedToNode(this._nodes[nodeNameB]))
                throw new GraphException($"\"{nodeNameA}\" is already connceted to \"{nodeNameB}\" !");

            // Can not add a connection that already exists
            if (this._nodes[nodeNameB].IsDirectlyConnectedToNode(this._nodes[nodeNameA]))
                throw new GraphException($"\"{nodeNameB}\" is already connceted to \"{nodeNameA}\" !");

            // Creating a two way node connection
            this._nodes[nodeNameA].AddConnectionToNode(this._nodes[nodeNameB]);
            this._nodes[nodeNameB].AddConnectionToNode(this._nodes[nodeNameA]);
        }

        public void RemoveOneWayConnections(string nodeNameA, string nodeNameB) {
            // nodeNameA needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(nodeNameA))
                throw new GraphException($"\"{nodeNameA}\" is not a valid key!");

            // nodeNameB needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(nodeNameB))
                throw new GraphException($"\"{nodeNameB}\" is not a valid key!");

            this._nodes[nodeNameA].RemoveConnectionToNode(this._nodes[nodeNameB]);
        }

        public void RemoveNodeFromGraph(string uniqueName) {
            // It needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            // Removing the connection
            this.RemoveAllConnectionsToNode(uniqueName);

            // Remove the node
            this._nodes.Remove(uniqueName);
        }

        public void RemoveAllConnectionsFromNode(string uniqueName) {
            // It needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            this._nodes[uniqueName].RemoveAllConnectionsFromThisNode();
        }

        public void RemoveOneConnectionsToNode(string uniqueName) {
            // It needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            // Loop through each node
            foreach (KeyValuePair<string, Node> pair in this._nodes) {
                // Does a direct connection to the node that is about to be removed exist?
                if (pair.Value.IsDirectlyConnectedToNode(this._nodes[uniqueName])) {
                    // Remove those direct connections
                    pair.Value.RemoveConnectionToNode(this._nodes[uniqueName]);
                }
            }
        }

        public bool IsConnectedToAnyNode(string uniqueName) {
            // It needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            return this._nodes[uniqueName].Connections.Count != 0;

        }

        public bool IsAnyNodeConnectedToNode(string uniqueName) {
            // It needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            foreach (KeyValuePair<string, Node> pair in this._nodes) {
                if (pair.Value.IsDirectlyConnectedToNode(this._nodes[uniqueName]))
                    return true;
            }

            return false;
        }

        public void RemoveAllConnectionsToNode(string uniqueName) {
            // It needs to be a key to an already existing node
            if (!this._nodes.Keys.Contains(uniqueName))
                throw new GraphException($"\"{uniqueName}\" is not a valid key!");

            // Loop through each node
            foreach (KeyValuePair<string, Node> pair in this._nodes) {
                // Does a direct connection to the node that is about to be removed exist?
                if (pair.Value.IsDirectlyConnectedToNode(this._nodes[uniqueName])) {
                    // Remove those direct connections
                    pair.Value.RemoveAllConnectionsToNode(this._nodes[uniqueName]);
                }
            }
        }

        public List<Route> GetAllPossibleRoutesFromNode(string uniqueName) {
            List<Route> allRoutes = new List<Route>();
            Node startNode = this._nodes[uniqueName];
            foreach (var destinationNode in _nodes) {
                if (!(startNode == destinationNode.Value) && !startNode.IsIndirectlyConnectedToNode(destinationNode.Value))
                    continue;
                allRoutes = startNode.GetRoutesToNode(destinationNode.Value, new Route(), allRoutes);
            }
            return allRoutes;
        }

        public List<Route> GetAllPossibleRoutes() {
            List<Route> allRoutes = new List<Route>();
            foreach (var startNode in _nodes) {
                foreach (var destinationNode in _nodes) {
                    if (!(startNode.Key == destinationNode.Key) && !startNode.Value.IsIndirectlyConnectedToNode(destinationNode.Value))
                        continue;
                    allRoutes = startNode.Value.GetRoutesToNode(destinationNode.Value, new Route(), allRoutes);
                }
            }
            return allRoutes;
        }

        public List<Route> GetAllRoutesFromNodeAToNodeB(string nodeNameA, string nodeNameB) {
            // Returns a List of all possible paths, where each connection can only be traversed once
            return this._nodes[nodeNameA].GetAllRoutesToNode(this._nodes[nodeNameB]);
        }

        public List<Node> GetShortestRouteFromNodeAToNodeBAsAList(string nodeNameA, string nodeNameB) {
            return this.GetShortestRouteFromNodeAToNodeB(nodeNameA, nodeNameB).Nodes;
        }

        public Route GetShortestRouteFromNodeAToNodeB(string nodeNameA, string nodeNameB) {
            // The two nodes need to be either directly or indirectly connected
            if (!this._nodes[nodeNameA].IsIndirectlyConnectedToNode(this._nodes[nodeNameB]))
                throw new GraphException($"\"{nodeNameA}\" is not connected to \"{nodeNameB}\"!");

            // Get all Routes
            List<Route> allRoutes = this.GetAllRoutesFromNodeAToNodeB(nodeNameA, nodeNameB);

            // References for the shortest path
            int shortestLength = allRoutes[0].Nodes.Count;
            int shortestPathIndex = 0;

            // Loop through all possible routes
            for (int i = 1; i < allRoutes.Count; i++) {
                // Is the current Path the shortest of the paths so far checked?
                if (allRoutes[i].Nodes.Count < shortestLength) {
                    // If so, set this to be the currently shortest path
                    shortestLength = allRoutes[i].Nodes.Count;
                    shortestPathIndex = i;
                }
            }

            // Return the shortest path
            return allRoutes[shortestPathIndex];
        }
    }

    public class GraphException: Exception {
        public GraphException(string message) : base("Graph Error: " + message) { }
    }
}
