using System;
using System.Collections.Generic;
using System.Collections;

namespace Rocket_Elevators_Csharp_Controller
{
    public class Column
    {
        public string ID;
        public string status;
        public int amountOfElevators;
        public int amountOfFloors;
        
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
            
            this.createElevators(amountOfFloors, amountOfElevators);
            this.createCallButtons(amountOfFloors, isBasement);
        }
        
        public void createCallButtons(int amountOfFloors, bool isBasement){
            int callButtonID = 1;
            if (isBasement)
            {
                int buttonFloor = -1;
                for(int i = 0; i < amountOfFloors; i++){
                    callButtonsList.Add(new CallButton(buttonFloor, "up"));
                    buttonFloor--;
                    callButtonID++;
                }
            }
            else{
                int buttonFloor = 1;
                for(int i = 0; i < amountOfFloors; i++){
                    callButtonsList.Add(new CallButton(buttonFloor, "down"));
                    buttonFloor++;
                    callButtonID++;
                }
            }
        }
    

        public void createElevators(int amountOfFloors, int amountOfElevators)
        {
            int elevatorID = 1;
            for (int i = 0; i < amountOfElevators; i++)
            {
                elevatorsList.Add(new Elevator(elevatorID.ToString()));
                elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
            Console.WriteLine($"||Passenger requests elevator from {userPosition} going {direction} to the lobby||");
            Elevator chosenElevator = this.findElevator(userPosition, direction);
            Console.WriteLine($"||{chosenElevator.ID} is the assigned elevator for this request||");
            chosenElevator.addNewRequest(userPosition);
            chosenElevator.move();
            chosenElevator.addNewRequest(1);
            return chosenElevator;    
        }

        //We use a score system depending on the current elevators state. Since the bestScore and the referenceGap are 
        //higher values than what could be possibly calculated, the first elevator will always become the default bestElevator, 
        //before being compared with to other elevators. If two elevators get the same score, the nearest one is prioritized. Unlike
        //the classic algorithm, the logic isn't exactly the same depending on if the request is done in the lobby or on a floor.
    
        public Elevator findElevator(int requestedFloor, string requestedDirection)
        {
            
            Hashtable bestElevatorInfo = new Hashtable(){
                {"bestElevator", null},
                {"bestScore", 6},
                {"referenceGap", int.MaxValue}
            };
            if(requestedFloor == 1){
                foreach(Elevator elevator in this.elevatorsList){
                    //The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed
                    if(1 == elevator.currentFloor && elevator.status == "stopped"){
                        bestElevatorInfo = checkIfElevatorIsBetter(1, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is at the lobby and has no requests
                    else if(1 == elevator.currentFloor && elevator.status == "idle"){
                        bestElevatorInfo = checkIfElevatorIsBetter(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is lower than me and is coming up.
                    //It means that I'm requesting an elevator to go to a basement, and the elevator is on it's way to me.
                    else if(1 > elevator.currentFloor && elevator.direction == "up"){
                        bestElevatorInfo = checkIfElevatorIsBetter(3, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is above me and is coming down.
                    //It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me
                    else if(1 < elevator.currentFloor && elevator.direction == "down"){
                        bestElevatorInfo = checkIfElevatorIsBetter(3, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is not at the first floor, but doesn't have any request
                    else if(elevator.status == "idle"){
                        bestElevatorInfo = checkIfElevatorIsBetter(4, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is not available, but still could take the call if nothing better is found
                    else{
                        bestElevatorInfo = checkIfElevatorIsBetter(5, elevator, requestedFloor, bestElevatorInfo);
                    }
                }
            }
            else{
                foreach(Elevator elevator in elevatorsList){
                    //The elevator is at the same level as me, and is about to depart to the first floor
                    if(requestedFloor == elevator.currentFloor && elevator.status == "idle" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkIfElevatorIsBetter(1, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way
                    else if(requestedFloor > elevator.currentFloor  && elevator.direction == "up" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkIfElevatorIsBetter(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way
                    else if(requestedFloor < elevator.currentFloor  && elevator.direction == "down" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkIfElevatorIsBetter(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    //The elevator is idle and has no requests
                    else if(elevator.status == "stopped"){
                        bestElevatorInfo = checkIfElevatorIsBetter(4, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else{
                        //The elevator is not available, but still could take the call if nothing better is found
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
}