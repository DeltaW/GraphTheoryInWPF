using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphTheory.Core {
    public class Node {
        
        public List<Connection> Connections { get; private set; }
        public string Name;
        public Point Position;

        public Node(string name) {
            this.Name = name;
            this.Connections = new List<Connection>();
        }

        public Node(string name, Point position) {
            this.Name = name;
            this.Connections = new List<Connection>();
            this.Position = position;
        }

        public void AddConnectionToNode(Node node) {
            // Adds a connection to a node
            this.Connections.Add(new Connection(this, node));
        }
        
        public bool IsDirectlyConnectedToNode(Node node) {
            // Loop through each connection
            foreach (Connection connection in this.Connections) {
                // Return true if there is a direct connection
                if (connection.ToNode == node)
                    return true;
            }
            // There is no direct connection
            return false;
        }

        public bool IsIndirectlyConnectedToNode(Node node) {
            // Returns true if there is an indirect Connection
            return this.IsIndirectlyConnectedToNode(node, new List<Node>());
        }

        public bool IsIndirectlyConnectedToNode(Node node, List<Node> previousConnections) {
            if (previousConnections.Contains(this))
                return false;
            if (this.IsDirectlyConnectedToNode(node))
                return true;

            bool isConnected = false;
            previousConnections.Add(this);
            foreach (Node connectedNode in this.Connections.Select(c => c.ToNode)) {
                isConnected = connectedNode.IsIndirectlyConnectedToNode(node, previousConnections);
                if (isConnected)
                    return true;
            }

            return false;
        }

        public void RemoveConnectionToNode(Node node) {
            // Remove the connection
            this.Connections.Remove(this.Connections.FirstOrDefault(c => c.ToNode == node));
        }

        public void RemoveAllConnectionsToNode(Node node) {
            // Remove the connection
            this.Connections.RemoveAll(c => c.ToNode == node);
        }

        public void RemoveAllConnectionsFromThisNode() {
            // Remove all connections from this node
            this.Connections.Clear();
        }

        public List<Route> GetAllRoutesToNode(Node destinationNode) {
            return this.GetRoutesToNode(destinationNode, new Route(), new List<Route>());

        }

        public List<Route> GetRoutesToNode(Node destinationNode, Route currentRoute, List<Route> allRoutes) {
            // Add the current node to the current route
            currentRoute.Nodes.Add(this);

            // Have we reached our destination?
            if (this == destinationNode) {
                // If so, add the current path to all routes
                allRoutes.Add(currentRoute);
                //currentRoute.Nodes.RemoveAt(currentRoute.Nodes.Count - 1);
            } else {
                // Otherwise go deeper
                foreach (Connection connection in this.Connections) {
                    // Connection to check is from node to connected node and vice versa
                    if (!currentRoute.DoesAlreadyUseThisConnection(this, connection.ToNode)) {
                        Route temp = new Route(currentRoute.Nodes);
                        /*List<Route> temp2 = */connection.ToNode.GetRoutesToNode(destinationNode, temp, allRoutes);
                    }
                }
            }
            return allRoutes;
        }
    }
}