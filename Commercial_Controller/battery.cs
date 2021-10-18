using System;
using System.Collections.Generic;

namespace Commercial_Controller
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

        }

    //     SET ID TO _id
    // SET status TO 'online' 
    // SET columnsList TO EMPTY ARRAY
    // SET floorRequestsButtonsList TO EMPTY ARRAY

    // IF _amountOfBasements IS GREATER THAN 0 THEN
    //     CALL THIS createBasementFloorRequestButtons WITH _amountOfBasements
    //     CALL THIS createBasementColumn WITH _amountOfBasements AND _amountOfElevatorPerColumn
    //     DECREMENT _amountOfColumns
    // ENDIF   

    // CALL THIS createFloorRequestButtons WITH _amountOfFloors
    // CALL THIS createColumns WITH _amountOfColumns AND _amountOfFloors AND _amountOfElevatorPerColumn
        public void createBasementColumn(int amountOfBasements, int amountOfElevatorPerColumn)
        {
            List<int> servedFloors = new List<int>(amountOfBasements + 1);
            int floor = -1;
            for (int i = 0; i < (amountOfBasements + 1); i++)
            {
                if (i == 0)
                {
                    servedFloors[i] = 1;
                }
                else
                {
                    servedFloors[i] = floor;
                    floor--;
                }
            }

            var column = new Column(columnID, amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(column);
            columnID++;
        }

    //     SEQUENCE createBasementColumn USING _amountOfBasements AND _amountOfElevatorPerColumn
    //     INIT servedFloors TO EMPTY ARRAY
    //     SET floor TO -1 
    //     FOR _amountOfBasements
    //         ADD floor TO servedFloors
    //         DECREMENT floor
    //     ENDFOR
        
    //     SET column TO NEW Column WITH columnID AND online AND _amountOfBasements AND _amountOfElevatorPerColumn AND servedFloors AND true
    //     ADD column TO THIS columnsList
    //     INCREMENT columnID
    // ENDSEQUENCE

        public void createColumns(int amountOfColumns, int amountOfFloors, int amountOfElevatorPerColumn)
        {
            int amountOfFloorsPerColumn = (int)Math.Ceiling((double)(amountOfFloors / amountOfColumns));
            int floor = 1;
            for (int i = 0; i < amountOfColumns; i++)
            {
                List<int> servedFloors = new List<int>(amountOfFloorsPerColumn + 1);
                for (int x = 0; x < amountOfFloorsPerColumn; x++)
                {
                    if (i == 0)
                    {
                        servedFloors[x] = floor;
                        floor++;
                    }
                    else
                    {
                        servedFloors[0] = 1;
                        servedFloors[x + 1] = floor;
                        floor++;
                    }
                }
                var column = new Column(columnID, amountOfElevatorPerColumn, servedFloors, false);
                columnsList.Add(column);
                columnID++;
            }
        }
    //     SEQUENCE createColumns USING _amountOfColumns AND _amountOfFloors AND _amountOfBasements AND _amountOfElevatorPerColumn
    //     SET amountOfFloorsPerColumn TO ROUND UP (_amountOfFloors / _amountOfColumns) 
    //     SET floor TO 1

    //     FOR _amountOfColumns
    //         SET servedFloors TO EMPTY ARRAY
    //         FOR amountOfFloorsPerColumn
    //             IF floor IS LESS OR EQUAL TO _amountOfFloors
    //                 ADD floor TO servedFloors
    //                 INCREMENT floor
    //             ENDIF
    //         ENDFOR

    //         SET column TO NEW Column WITH columnID AND online AND _amountOfFloors AND _amountOfElevatorPerColumn AND servedFloors AND false
    //         ADD column TO THIS columnsList
    //         INCREMENT columnID
    //     ENDFOR
    // ENDSEQUENCE
        public void createFloorRequestButtons(int amountOfFloors)
        {
            int buttonFloor = 1;
            for (int i = 0; i < amountOfFloors; i++)
            {
                var floorRequestButton = new FloorRequestButton(floorRequestButtonID, "up");
                floorRequestButtonsList.Add(floorRequestButton);
                buttonFloor++;
                floorRequestButtonID++;
            }
        }

    //     SEQUENCE createFloorRequestButtons USING _amountOfFloors
    //     SET buttonFloor TO 1
    //     FOR _amountOfFloors
    //         SET floorRequestButton TO NEW FloorRequestButton WITH floorRequestButtonID AND OFF AND buttonFloor AND Up
    //         ADD floorRequestButton TO THIS floorButtonsList
    //         INCREMENT buttonFloor
    //         INCREMENT floorRequestButtonID
    //     ENDFOR
    // ENDSEQUENCE

        public void createBasementFloorRequestButtons(int amountOfBasements)
        {
            int buttonFloor = -1;
            for (int i = 0; i < amountOfBasements; i++)
            {
                var floorRequestButton = new FloorRequestButton(floorRequestButtonID, "down");
                floorRequestButtonsList.Add(floorRequestButton);
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
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            int? tempFloor = null;
            Column column = this.findBestColumn(_requestedFloor);
            Elevator elevator = column.findElevator(1, _direction);
            elevator.currentFloor = 1;
            Door.operateDoors();
            elevator.floorRequestList.Add(_requestedFloor);
            elevator.sortFloorList();
            elevator.move(tempFloor);
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

