namespace Rocket_Elevators_Csharp_Controller
{
    //Button on a floor or basement to go back to lobby
    public class FloorRequestButton
    {
        public int ID;
        public string status;
        public int floor;
        public string direction;
        public FloorRequestButton(int _floor, string _direction)
        {
            this.floor = _floor;
            this.direction = _direction;
            
        }
    }
}