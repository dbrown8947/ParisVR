using System;
using System.Collections.Generic;
using System.Linq;

namespace GrahamScan
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Program p = new Program();
            List<OSMPositionNode> newList = new List<OSMPositionNode>();
            List<OSMPositionNode> listOfGrahamScanPoints = new List<OSMPositionNode>();
            List<PolarAngle> listOfAngles = new List<PolarAngle>();
            List<PolarAngle> listOfNodesToRemove = new List<PolarAngle>();
            List<OSMPositionNode> nodeList = new List<OSMPositionNode>();

            List<OSMPositionNode> tempList = new List<OSMPositionNode>();

            List<OSMPositionNode> listOfNodes = new List<OSMPositionNode>()
            {
                new OSMPositionNode(){ id = 0, longitude = 0, latidude = 3},
                new OSMPositionNode(){id = 1, longitude =1, latidude = 1},
                new OSMPositionNode(){id = 2,  longitude = 2, latidude = 2},
                new OSMPositionNode(){id = 3,  longitude = 4, latidude = 4 },
                new OSMPositionNode(){id = 4,  longitude = 0, latidude =0},
                new OSMPositionNode(){id = 5,  longitude = 1, latidude = 2},
                new OSMPositionNode(){id = 6,  longitude = 3, latidude = 1 },
                new OSMPositionNode(){id = 7,  longitude = 3, latidude = 3 }
            };

            //for (int i = 0; i < 7; i++)
            //{
            //    double lon = random.Next(-15, 15);
            //    listOfNodes[i].longitude = lon;
            //    double lat = random.Next(-15, 15);
            //    listOfNodes[i].latidude = lat;


            //}
            Console.WriteLine("Before Sort:");

            foreach (OSMPositionNode o in listOfNodes)
            {
                Console.WriteLine("x: " + o.longitude + " y: " + o.latidude);

            }

            OSMPositionNode anchorNode = new OSMPositionNode();
            anchorNode = listOfNodes.OrderBy(node => node.latidude).First();
            Console.WriteLine("Anchor x: " + anchorNode.longitude + " y: " + anchorNode.latidude);

            OSMPositionNode currentMinNode = new OSMPositionNode();



            // Create a list of nodes with their polar angle relative to the anchor node
            foreach(OSMPositionNode o in listOfNodes)
            {
                PolarAngle currentPolarAngle = new PolarAngle();

                double angle = PolarAngle(o, anchorNode)* 180f / Math.PI;

                currentPolarAngle.angle = angle;
                currentPolarAngle.node = o;

                listOfAngles.Add(currentPolarAngle);                   
                
				Console.WriteLine("x: "+o.longitude+" y: "+o.latidude+" angle: "+ angle);
                              
            }

            // get the duplicate angles
            var duplicates = listOfAngles.GroupBy(i => new { i.angle })
              .Where(g => g.Count() > 1)
              .Select(g => g.Key);

            List<PolarAngle> tempAngleList = new List<PolarAngle>();
            PolarAngle tempPolarAngle = new PolarAngle();

			
			/*NOTE: I didn't have time to test it. But this might be used to skip the prev,cur,next method below
			because this returns the outer most nodes only! */
            // iterate through each duplicate angle
          
            foreach (var v in duplicates)
            {
                // add the duplicate angles with their nodes to a temporary list
                foreach (PolarAngle currentAngle in listOfAngles)
                {
                    if (v.angle == currentAngle.angle)
                    {
                        
                        tempAngleList.Add(currentAngle);
                    }
                }

                // get the polar angle with the highest y coordinate
               tempPolarAngle = tempAngleList.OrderByDescending(node => node.node.latidude).First();

                // iterate through the temporary angle list that hold the duplicate angles
                foreach (PolarAngle n in tempAngleList)
                {
                    // check if it exists in the main node list
                    bool node_exists = listOfNodes.Exists(s => s.id == n.node.id);

                    // if it does exist
                    if(node_exists)
                    {
                        //make sure it is not the polar angle with the highest y coordinate
                        // if it is not then remove the lower y coordinate duplicate nodes from the main list
                        if(n.node.id != tempPolarAngle.node.id)
                        {
                            listOfNodes.Remove(n.node);
                        }
                    }
                }

                // clear the temp list if there are more duplicate angles to check.
                tempList.Clear();
            }

			//order the nodes by the polar angle
            listOfNodes = listOfNodes.OrderBy(x => PolarAngle(x,anchorNode)).ToList();

            Console.WriteLine("After Sort:");

            foreach (OSMPositionNode o in listOfNodes)
            {
                double angle = PolarAngle(o, anchorNode) * 180 / Math.PI;

				Console.WriteLine("X: "+o.longitude+" Y: "+o.latidude+" Angle: "+ angle);

            }

			
			// Here is where the perimeter is made by determining if the current angle being checked is counter clockwise 
			//https://www.geeksforgeeks.org/convex-hull-set-2-graham-scan/
			
			/*The output from this example is 
			(0, 3)
			(4, 4)
			(3, 1)
			(0, 0) */
			
			/* The output generated here is only (0,0) (3,1) and (4,4) */
			/*if you look at the geeksforgeek diagram where they show how the prev,cur,next checking is implemented
			I might be checking them wrong? not sure. I just add the anchor node to the beginning and the greatest y coord with the least x coord node to the end */
            OSMPositionNode prev = new OSMPositionNode();
            OSMPositionNode cur = new OSMPositionNode();
            OSMPositionNode next = new OSMPositionNode();
			
			// add the anchor node to the list of needed cooridnates for graham scan
            listOfGrahamScanPoints.Add(anchorNode);
			
			// iterate through each node checking if the current node is counter clockwise
            for(int i = 1; i < listOfNodes.Count-1; i++)
            {
                cur = listOfNodes[i];
				next = listOfNodes[i + 1];
                prev = listOfNodes[i - 1];

				// check if the current node being checked is counter clockwise
				// reject the node if it is clockwise.
                if (direction(cur, prev, next) <= 0 )
                {
                    listOfGrahamScanPoints.Add(cur);

                }

				// add the final polar angle node to the graham scan list
				if(i == listOfNodes.Count - 1)
				{
					listOfGrahamScanPoints.Add(listOfNodes[i+1]);
				}
            }


            Console.WriteLine("Graham Scan Points:");

            foreach (OSMPositionNode o in listOfGrahamScanPoints)
            {
                Console.WriteLine("x: " + o.longitude + " y: " + o.latidude);

            }
        }

		//http://www2.lawrence.edu/fast/GREGGJ/CMSC210/convex/convex.html
        // Determine the turn direction around the corner
        // formed by the points a, b, and c. Return a 
        // positive number for a left turn and negative
        // for a right turn.
        static double direction(OSMPositionNode a, OSMPositionNode b, OSMPositionNode c)
        {
            return (b.longitude - a.longitude) * (c.latidude - a.latidude) - (c.longitude - a.longitude) * (b.latidude - a.latidude);
        }

        // Compute the polar angle in radians formed
        // by the line segment that runs from p0 to p
        static double PolarAngle(OSMPositionNode p, OSMPositionNode p0)
        {
            return Math.Atan2(p.latidude - p0.latidude, p.longitude - p0.longitude);
        }


       static bool Compare(OSMPositionNode previous, OSMPositionNode current, OSMPositionNode next)
        {

            bool counterClockwise = false;

            double angle1 = Math.Atan2(previous.latidude - current.latidude, previous.longitude - current.longitude);
            double angle2 = Math.Atan2(next.latidude - current.latidude, next.longitude - current.longitude);

            //For counter-clockwise, just reverse the signs of the return values
            if (angle1 < angle2)
            {
                counterClockwise = false;
            }
            else if (angle2 < angle1)
            {
                counterClockwise = true;

            }

            return counterClockwise;
        }

        
    }


    class OSMPositionNode
    {
        public long id;
        public double longitude { get; set; }
        public double latidude { get; set; }
        public double angle { get; set; }

    }

    class PolarAngle
    {
        public double angle { get; set;}
        public OSMPositionNode node { get; set; }
    }
}
