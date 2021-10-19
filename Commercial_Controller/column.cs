using System;
using System.Collections.Generic;
using System.Collections;

namespace Commercial_Controller
{
    public class Column
    {
        public string ID;
        public string status;
        public int amountOfElevators;
        
        public bool isBasement;
        public List<int> servedFloors;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;
        public Column(string _ID, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            this.ID = _ID;
            this.status = "online";
            this.amountOfElevators = _amountOfElevators;
            this.isBasement = _isBasement;
            this.elevatorsList = new List<Elevator>();
            this.callButtonsList = new List<CallButton>();
            this.servedFloors = _servedFloors;  
        }

    //     DEFINE Column USING _id AND _status AND _amountOfFloors AND _amountOfElevators AND _servedFloors AND _isBasement
    // '//---------------------------------Initialization--------------------------------------------//
    // SET ID TO _id
    // SET status TO _status 
    // SET amountOfFloors TO _amountOfFloors
    // SET amountOfElevators TO _amountOfElevators
    // SET elevatorsList TO EMPTY ARRAY
    // SET callButtonsList TO EMPTY ARRAY
    // SET servedFloorsList TO _servedFloors

    // CALL THIS createElevators USING _amountOfFloors AND _amountOfElevators 
    // CALL THIS createCallButtons USING _amountOfFloors AND _isBasement

        public void createCallButtons(int floorsServed, int amountOfBasements, bool isBasement)
        {
            int callButtonID = 1;
            if (isBasement)
            {
                int buttonFloor = -1;
                for (int i = 0; i < amountOfBasements; i++)
                {
                    var callButton = new CallButton(buttonFloor, "up");
                    callButtonsList.Add(callButton);
                    buttonFloor--;
                    callButtonID++;
                }
            }
            else
            {
                int buttonFloor = 1;
                foreach (int floor in servedFloors)
                {
                    var callButton = new CallButton(floor, "down");
                    callButtonsList.Add(callButton);
                    buttonFloor++;
                    callButtonID++;
                }
            }
        }

        public void createElevators(int[] servedFloors, int amountOfElevators)
        {
            int elevatorID = 1;
            for (int i = 0; i < amountOfElevators; i++)
            {
                var elevator = new Elevator(elevatorID.ToString());
                elevatorsList.Add(elevator);
                elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public void requestElevator(int userPosition, string direction)
        {
            Console.WriteLine($"||Passenger requests elevator from {userPosition} going {direction} to the lobby||");
            Elevator elevator = this.findElevator(userPosition, direction);
            Console.WriteLine($"||{elevator.ID} is the assigned elevator for this request||");
            elevator.floorRequestList.Add(1);
            elevator.sortFloorList();
            elevator.move();
            elevator.operateDoors();
    
        }
    //     SEQUENCE requestElevator USING userPosition AND direction
    //     SET elevator TO CALL THIS findElevator WITH userPosition AND direction RETURNING elevator
    //     CALL elevator addNewRequest WITH _requestedFloor
    //     CALL elevator move

    //     CALL elevator addNewRequest WITH 1 '//Always 1 because the user can only go back to the lobby
    //     CALL elevator move

    // ENDSEQUENCE
        public void findElevator(int requestedFloor, string requestedDirection)
        {
            
            Hashtable bestElevatorInfo = new Hashtable(){
                {"bestElevator", null},
                {"bestScore", 6},
                {"referenceGap", int.MaxValue}
            };
            if(requestedFloor == 1){
                foreach(Elevator elevator in elevatorsList){
                    if(1 == elevator.currentFloor && elevator.status == "stopped"){
                        bestElevatorInfo = checkIfElevatorIsBetter(1, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(1 == elevator.currentFloor && elevator.status == "idle"){
                        bestElevatorInfo = checkIfElevatorIsBetter(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(1 > elevator.currentFloor && elevator.direction == "up"){
                        bestElevatorInfo = checkIfElevatorIsBetter(3, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(1 < elevator.currentFloor && elevator.direction == "down"){
                        bestElevatorInfo = checkIfElevatorIsBetter(3, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInfo = checkIfElevatorIsBetter(4, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else{
                        bestElevatorInfo = checkIfElevatorIsBetter(5, elevator, requestedFloor, bestElevatorInfo);
                    }
                }
            }
            else{
                foreach(Elevator elevator in elevatorsList){
                    if(requestedFloor == elevator.currentFloor && elevator.status == "idle" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkIfElevatorIsBetter(1, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(requestedFloor > elevator.currentFloor  && elevator.direction == "up" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkIfElevatorIsBetter(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(requestedFloor < elevator.currentFloor  && elevator.direction == "down" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkIfElevatorIsBetter(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(elevator.status == "stopped"){
                        bestElevatorInfo = checkIfElevatorIsBetter(4, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else{
                        bestElevatorInfo = checkIfElevatorIsBetter(5, elevator, requestedFloor, bestElevatorInfo);
                    }
                }
            }
            return (Elevator)bestElevatorInfo["bestElevator"];
        }

        public Hashtable checkIfElevatorIsBetter(int baseScore, Elevator elevator, int floor, Hashtable bestElevatorInfo){
            if(baseScore < (int)bestElevatorInfo["bestScore"]){
                bestElevatorInfo["bestScore"] = baseScore;
                bestElevatorInfo["bestElevator"] = elevator;
                bestElevatorInfo["referenceGap"] = (int)Math.Abs((double)elevator.currentFloor - floor);
            }
            else if((int)bestElevatorInfo["bestScore"] == baseScore){
                int gap = (int)Math.Abs((double)elevator.currentFloor - floor);
                if((int)bestElevatorInfo["referenceGap"] > gap){
                    bestElevatorInfo["bestElevator"] = elevator;
                    bestElevatorInfo["referenceGap"] = gap;
                }
            }
            return bestElevatorInfo;
        }
        
    }


        
//     '//We use a score system depending on the current elevators state. Since the bestScore and the referenceGap are 
//     '//higher values than what could be possibly calculated, the first elevator will always become the default bestElevator, 
//     '//before being compared with to other elevators. If two elevators get the same score, the nearest one is prioritized. Unlike
//     '//the classic algorithm, the logic isn't exactly the same depending on if the request is done in the lobby or on a floor.
//     SEQUENCE findElevator USING requestedFloor AND requestedDirection RETURNING bestElevator
//         INIT bestElevator
//         SET bestScore TO 6
//         SET referenceGap TO 10000000
//         INIT bestElevatorInformations

//         IF requestedFloor EQUALS 1 THEN
//             FOR EACH elevator IN THIS elevatorsList
//                 '//The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed
//                 IF 1 EQUALS elevator currentFloor AND elevator status EQUALS stopped THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 1 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is at the lobby and has no requests
//                 ELSE IF 1 EQUALS elevator currentFloor AND elevator status EQUALS idle THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 2 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is lower than me and is coming up. It means that I'm requesting an elevator to go to a basement, and the elevator is on it's way to me.
//                 ELSE IF 1 IS GREATER THAN elevator currentFloor AND elevator direction EQUALS up THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 3 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is above me and is coming down. It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me
//                 ELSE IF 1 IS LESS THAN elevator currentFloor AND elevator direction EQUALS down THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 3 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is not at the first floor, but doesn't have any request
//                 ELSE IF elevator status EQUALS idle THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 4 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is not available, but still could take the call if nothing better is found
//                 ELSE 
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 5 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 ENDIF
//                 SET bestElevator TO bestElevatorInformations bestElevator
//                 SET bestScore TO bestElevatorInformations bestScore
//                 SET referenceGap TO bestElevatorInformations referenceGap
//             ENDFOR
//         ELSE
//             FOR EACH elevator IN THIS elevatorsList
//                 '//The elevator is at the same level as me, and is about to depart to the first floor
//                 IF requestedFloor EQUALS elevator currentFloor AND elevator status EQUALS stopped AND requestedDirection EQUALS elevator direction THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 1 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way
//                 ELSE IF requestedFloor IS GREATER THAN elevator currentFloor AND elevator direction EQUALS up AND requestedDirection EQUALS up THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 2 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way
//                 ELSE IF requestedFloor IS LESS THAN elevator currentFloor AND elevator direction EQUALS down AND requestedDirection EQUALS down THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 2 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is idle and has no requests
//                 ELSE IF elevator status EQUALS idle THEN
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 4 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 '//The elevator is not available, but still could take the call if nothing better is found
//                 ELSE 
//                     SET bestElevatorInformations TO CALL THIS checkIfElevatorIsBetter WITH 5 AND elevator AND bestScore AND referenceGap AND bestElevator AND requestedFloor RETURNING bestElevatorInformations
//                 ENDIF
//                 SET bestElevator TO bestElevatorInformations bestElevator
//                 SET bestScore TO bestElevatorInformations bestScore
//                 SET referenceGap TO bestElevatorInformations referenceGap
//             ENDFOR
//         ENDIF
//         RETURN bestElevator
//     ENDSEQUENCE

//     SEQUENCE checkIfElevatorIsBetter USING scoreToCheck AND newElevator AND bestScore AND referenceGap AND bestElevator AND floor RETURNING bestElevatorInformations
//         IF scoreToCheck IS LESS THAN bestScore THEN
//             SET bestScore TO scoreToCheck
//             SET bestElevator TO newElevator
//             SET referenceGap TO ABSOLUTE VALUE OF newElevator currentFloor - floor
//         ELSE IF bestScore EQUALS scoreToCheck
//             SET gap TO ABSOLUTE VALUE OF newElevator currentFloor - floor
//             IF referenceGap IS GREATER THAN gap THEN
//                 SET bestElevator TO newElevator
//                 SET referenceGap TO gap
//             ENDIF
//         ENDIF
//         RETURN bestElevator AND bestScore AND referenceGap AS bestElevatorInformations
//     ENDSEQUENCE

// ENDDEFINE '//Column
    
            

    
    
}