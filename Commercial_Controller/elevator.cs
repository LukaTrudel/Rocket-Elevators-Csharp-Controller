using System.Threading;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    
    {
        public string ID;
        public string status;
        public List<int> servedFloors;
        public int currentFloor;
        public string direction;
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

    //     DEFINE Elevator USING _id AND _status AND _amountOfFloors AND _currentFloor
    // '//---------------------------------Initialization--------------------------------------------//
    // SET ID TO _id
    // SET status TO _status
    // SET amountOfFloors TO _amountOfFloors
    // SET currentFloor TO _currentFloor
    // SET Door TO NEW Door WITH _id AND closed
    // SET floorRequestsList TO EMPTY ARRAY
    // SET direction TO null
    // SET overweight TO false
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
                        //this.screenDisplay = this.currentFloor;
                    }
                }
                else if (this.currentFloor > destination) {
                    this.direction = "down";
                    this.sortFloorList();
                    while (this.currentFloor > destination) {
                        this.currentFloor--;
                        //this.screenDisplay = this.currentFloor;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.completedRequestsList.Add(this.floorRequestsList[0]);
                this.floorRequestsList.RemoveAt(0);
            }
            this.status = "idle";
        }
    //     SEQUENCE move  
    //     WHILE THIS requestList IS NOT empty
    //         SET destination TO first element of THIS requestList
    //         SET THIS status TO moving
    //         IF THIS currentFloor IS LESS THAN destination THEN
    //             SET THIS direction TO up
    //             CALL THIS sortFloorList
    //             WHILE THIS currentFloor IS LESS THAN destination
    //                 INCREMENT THIS currentFloor
    //                 SET THIS screenDisplay TO THIS currentFloor
    //             ENDWHILE
    //         ELSE IF THIS currentFloor IS GREATER THAN destination THEN
    //             SET THIS direction TO down
    //             CALL THIS sortFloorList
    //             WHILE THIS currentFloor IS LESS THAN destination
    //                 DECREMENT THIS currentFloor
    //                 SET THIS screenDisplay TO THIS currentFloor
    //             ENDWHILE
    //         ENDIF
    //         SET THIS status TO stopped
    //         CALL THIS operateDoors
    //         REMOVE first element OF THIS requestList
    //     ENDWHILE
    //     SET THIS status TO idle
    // ENDSEQUENCE

        public void sortFloorList()
        {
            if(direction == "up"){
               floorRequestsList.Sort();
           }
           else{
               floorRequestsList.Reverse();
           }

        }

    //     SEQUENCE sortFloorList
    //     IF THIS direction EQUALS up THEN
    //         SORT THIS requestList ASCENDING
    //     ELSE 
    //         SORT THIS requestList DESCENDING
    //     ENDIF
    // ENDSEQUENCE
        public void operateDoors()
        {

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

    //     SEQUENCE addNewRequest USING requestedFloor
    //     IF THIS floorRequestsList DOES NOT CONTAIN requestedFloor THEN
    //         ADD requestedFloor TO THIS floorRequestsList
    //     ENDIF

    //     IF THIS currentFloor < requestedFloor THEN
    //         SET THIS direction TO up
    //     ENDIF
    //     IF THIS currentFloor > requestedFloor THEN
    //         SET THIS direction TO down
    //     ENDIF
    // ENDSEQUENCE
        
    }
}