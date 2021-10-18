using System;
using System.Collections.Generic;

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
                var elevator = new Elevator(elevatorID);
                elevatorsList.Add(elevator);
                elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
    
        }

    }
}