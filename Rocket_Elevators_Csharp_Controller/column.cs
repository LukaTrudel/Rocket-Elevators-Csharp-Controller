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
    
        public Elevator findElevator(int requestedFloor, string requestedDirection)
        {
            
            Hashtable bestElevatorInfo = new Hashtable(){
                {"bestElevator", null},
                {"bestScore", 6},
                {"referenceGap", int.MaxValue}
            };
            if(requestedFloor == 1){
                foreach(Elevator elevator in this.elevatorsList){
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
}