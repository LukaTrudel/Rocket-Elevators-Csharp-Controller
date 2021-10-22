using System.Threading;
using System.Collections.Generic;

namespace Rocket_Elevators_Csharp_Controller
{
    public class Elevator
    
    {
        public string ID;
        public string status;
        public List<int> servedFloors;
        public int currentFloor;
        public string direction;
        public bool overweight;
        public bool obstructed;
        public int screenDisplay;
        public Door door;
        public List<int> floorRequestsList;
        public List<int> completedRequestsList;
        public Elevator(string _elevatorID)
        
        {
            this.ID = _elevatorID;
            this.status = "online";
            this.direction = "";
            this.door = new Door();
            this.floorRequestsList = new List<int>();
            this.completedRequestsList = new List<int>();
            
        }

        public void move()
        {
            while (this.floorRequestsList.Count != 0) {
                int destination = this.floorRequestsList[0];
                this.status = "moving";
                if (this.currentFloor < destination) {
                    this.direction = "up";
                    this.sortFloorList();
                    while (this.currentFloor < destination) {
                        this.currentFloor++;
                        this.screenDisplay = this.currentFloor;
                    }
                }
                else if (this.currentFloor > destination) {
                    this.direction = "down";
                    this.sortFloorList();
                    while (this.currentFloor > destination) {
                        this.currentFloor--;
                        this.screenDisplay = this.currentFloor;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.completedRequestsList.Add(this.floorRequestsList[0]);
                this.floorRequestsList.RemoveAt(0);
            }
            this.status = "idle";
        }

        public void sortFloorList()
        {
            if(direction == "up"){
               floorRequestsList.Sort();
           }
           else{
               floorRequestsList.Reverse();
           }

        }

        public void operateDoors()
        {
            this.door.status = "opened";

            if (!this.overweight) {
                this.door.status = "closing";
                if (!this.obstructed) {
                    this.door.status = "closed";
                }
                else {
                    this.operateDoors();
                }
            }

            else {
                while(this.overweight) {
                    //ALARM STARTS
                }
                this.operateDoors();
            }

        }

        public void addNewRequest(int requestedFloor)
        {
            if (!floorRequestsList.Contains(requestedFloor))
            {
                this.floorRequestsList.Add(requestedFloor);
            }
            else if(currentFloor < requestedFloor)
            {
                this.direction = "up";
            }
            else if(currentFloor > requestedFloor)
            {
                this.direction = "down";

            }
        }
    }
}