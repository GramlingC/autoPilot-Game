using System;

namespace TestApp
{
    public class Ship
    {
        private int hullIntegrity = 100;
        private int fuel = 100;
        private int lifesigns = 100;
        private int weapons = 100;
        private int empathyLevel = 0;

        public Ship()
        {
        }

        public Ship(int hullIntegrity, int fuel, int lifesigns, int empathyLevel)
        {
            this.hullIntegrity = hullIntegrity;
            this.fuel = fuel;
            this.lifesigns = lifesigns;
            this.empathyLevel = empathyLevel;
        }

        //hullIntegrity methods
        public int HullIntegrity
        {
            get { return hullIntegrity; }
            set { hullIntegrity = value; }
        }

        public void ChangeHullIntegrity(int change)
        {
            hullIntegrity += change;
        }

        //fuel methods
        public int Fuel
        {
            get { return fuel; }
            set { fuel = value; }
        }

        public void ChangeFuel(int change)
        {
            fuel += change;
        }

        //lifesigns methods
        public int Lifesigns
        {
            get { return lifesigns; }
            set { lifesigns = value; }
        }

        public void ChangeLifesigns(int change)
        {
            lifesigns += change;
        }

        //empathyLevel methods
        public int EmpathyLevel
        {
            get { return empathyLevel; }
            set { empathyLevel = value; }
        }

        public void ChangeEmpathyLevel(int change)
        {
            empathyLevel += change;
        }

        //Weapons methods
        public int Weapons
        {
            get { return weapons; }
            set { weapons = value; }
        }

        public void ChangeWeap(int change)
        {
            weapons += change;
        }

        public override string ToString()
        {
            return "Ship: H=" + hullIntegrity + " F=" + fuel + " L=" + lifesigns + " E=" + empathyLevel;
        }
    }
}
