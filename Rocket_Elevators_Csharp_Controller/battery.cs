using System;
using System.Collections.Generic;


namespace Rocket_Elevators_Csharp_Controller
{
    public class Battery
    {
        public int ID;
        public int amountOfColumns;
        public string status;
        public int amountOfFloors;
        public int amountOfBasements;
        public int amountOfElevatorPerColumn;
        public int servedFloors;
        public int columnID = 1;
        public int floorRequestButtonID = 1;

        public int floor;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorRequestButtonsList;

        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _ID;
            this.amountOfColumns = _amountOfColumns;
            this.status = "online";
            this.amountOfFloors = _amountOfFloors;
            this.amountOfBasements = _amountOfBasements;
            this.amountOfElevatorPerColumn = _amountOfElevatorPerColumn;
            this.columnsList = new List<Column>();
            this.floorRequestButtonsList = new List<FloorRequestButton>();

            if (this.amountOfBasements > 0){
                createBasementFloorRequestButtons(amountOfBasements);
                createBasementColumn(this.amountOfBasements, amountOfElevatorPerColumn);
                amountOfColumns--;
            }
            createFloorRequestButtons(amountOfFloors);
            createColumns(amountOfColumns, this.amountOfFloors, amountOfElevatorPerColumn);

        }

        public void createBasementColumn(int amountOfBasements, int amountOfElevatorPerColumn)
        {
            List<int> servedFloors = new List<int>();
            this.floor = -1;
    
            for (int i = 0; i < amountOfBasements; i++)
            {
                servedFloors.Add(floor);
                floor--;
            }
            columnsList.Add(new Column(columnID.ToString(), amountOfElevatorPerColumn, servedFloors, true));
            columnID++;
        }

        public void createColumns(int amountOfColumns, int amountOfFloors, int amountOfElevatorPerColumn){
            int amountOfFloorsPerColumn = (int)Math.Ceiling((double)amountOfFloors / amountOfColumns);
            this.floor = 1;

            for (int i = 0; i <= amountOfColumns; i++){ 
                List<int> servedFloors = new List<int>(); 
                
                for (int n = 0; n < amountOfFloorsPerColumn; n++){
                    if(floor <= amountOfFloors){
                        servedFloors.Add(floor);
                        floor++;
                    }
                }
                columnsList.Add(new Column(columnID.ToString(), amountOfElevatorPerColumn, servedFloors, false));
                columnID++;
            }
        }    
        public void createFloorRequestButtons(int _amountOfFloors)
        {
            int buttonFloor = 1;
            for (int i = 0; i < _amountOfFloors; i++)
            {
                floorRequestButtonsList.Add(new FloorRequestButton(floorRequestButtonID, "up"));
                buttonFloor++;
                floorRequestButtonID++;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;
            for (int i = 1; i <= _amountOfBasements; i++)
            {
                floorRequestButtonsList.Add(new FloorRequestButton(floorRequestButtonID, "down"));
                buttonFloor--;
                floorRequestButtonID++;
            }
        }

    //     SEQUENCE createBasementFloorRequestButtons USING _amountOfBasements
    //     SET buttonFloor TO -1
    //     FOR _amountOfBasements
    //         SET floorRequestButton TO NEW FloorRequestButton WITH floorRequestButtonID AND OFF AND buttonFloor AND Down
    //         ADD floorRequestButton TO THIS floorButtonsList
    //         DECREMENT buttonFloor
    //         INCREMENT floorRequestButtonID
    //     ENDFOR
    // ENDSEQUENCE

        public Column findBestColumn(int _requestedFloor)
        {
            Column bestColumn = null;
            foreach (Column column in this.columnsList)
            {
                if (column.servedFloors.Contains(_requestedFloor))
                {
                    bestColumn = column;
                }
            }
            return bestColumn;  
        }
    //     SEQUENCE findBestColumn USING _requestedFloor RETURNING column
    //     FOR EACH column IN THIS columnsList
    //         IF column servedFloorsList CONTAINS _requestedFloor
    //             RETURN column
    //         ENDIF
    //     ENDFOR
    // ENDSEQUENCE
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            Column chosenColumn = this.findBestColumn(_requestedFloor);
            Elevator chosenElevator = chosenColumn.findElevator(1, _direction);
            //elevator.floorRequestsList.Add(_requestedFloor);
            //elevator.sortFloorList();
            chosenElevator.addNewRequest(1);
            chosenElevator.move();
            chosenElevator.addNewRequest(_requestedFloor);
            chosenElevator.move();
            chosenElevator.operateDoors();
            return (chosenColumn, chosenElevator);
        }

    //     SEQUENCE assignElevator USING _requestedFloor AND _direction
    //     SET column TO THIS findBestColumn WITH _requestedFloor RETURNING column
    //     SET elevator TO CALL column findElevator WITH 1 AND _direction RETURNING bestElevator '// The floor is always 1 because that request is always made from the lobby.
    //     CALL elevator addNewRequest WITH 1
    //     CALL elevator move

    //     CALL elevator addNewRequest WITH _requestedFloor
    //     CALL elevator move
    // ENDSEQUENCE
    }
}
