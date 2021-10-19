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
            while(floorRequestsList.Count != 0){
                int destination = floorRequestsList[0];
                status = "moving";
                if(currentFloor < destination){
                    direction = "up";
                    while(currentFloor < destination){
                        currentFloor++;
                    }
                }
                else if(currentFloor > destination){
                    direction = "down";
                    while(currentFloor > destination){
                        currentFloor--;
                    }
                }
                status = "idle";
                floorRequestsList.RemoveAt(0);
            }

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

        public void addNewRequest()
        {

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