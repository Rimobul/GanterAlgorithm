using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public static class TestContext
    {
        public static FormalContext GeneratePlanets()
        {
            List<Attribute> attributes = new List<Attribute>()
            {
                new Attribute() {Name = "Mala" },
                new Attribute() {Name = "Stredna" },
                new Attribute() {Name = "Velka" },
                new Attribute() {Name = "Blizko" },
                new Attribute() {Name = "Daleko" },
                new Attribute() {Name = "Ano - mesiac" },
                new Attribute() {Name = "Nie - mesiac" }
            };

            List<Item> planets = new List<Item>()
            {
                new Item() { Name = "Merkur" },
                new Item() {Name = "Venusa" },
                new Item() {Name= "Zem" },
                new Item() {Name="Mars" },
                new Item() {Name="Jupiter" },
                new Item() {Name="Saturn" },
                new Item() {Name="Uran" },
                new Item() {Name="Neptun" },
                new Item() {Name="Pluto" }
            };

            bool[,] matrix = new bool[,]
            {
                {true, false, false, true, false, false, true },
                {true, false, false, true, false, false, true },
                {true, false, false, true, false, true, false },
                {true, false, false, true, false, true, false },
                {false, false, true, false, true, true, false },
                {false, false, true, false, true, true, false },
                {false, true, false, false, true, true, false },
                {false, true, false, false, true, true, false },
                {true, false, false, false, true, true, false }
            };

            return new FormalContext(attributes, planets, matrix, true);
        }

        public static FormalContext GeneratePapers()
        {
            List<Attribute> attributes = new List<Attribute>()
            {
                new Attribute() {Name="brilliant white" },
                new Attribute() {Name="fine white" },
                new Attribute() {Name="white" },
                new Attribute() {Name="high-performance copiers" },
                new Attribute() {Name="copiers" },
                new Attribute() {Name="liquid toner copiers" },
                new Attribute() {Name="type writers" },
                new Attribute() {Name="double sided" },
            };

            List<Item> papers = new List<Item>()
            {
                new Item() {Name = "Copy-Lux" },
                new Item() {Name = "Copy-X" },
                new Item() {Name = "Copy" },
                new Item() {Name = "Liquid-Copy" },
                new Item() {Name = "Office" },
                new Item() {Name = "Offset" },
            };

            bool[,] matrix = new bool[,]
            {
                {true, false, false, true, true, false, true, true },
                {false, true, false, true, true, false, true, true },
                {false, false, true, true, true, false, true, true},
                {false, false, true, false, true, true, false, true },
                {false, false, true, false, true, false, true, false},
                {false, false, true, false, false, false, true, false},
            };

            return new FormalContext(attributes, papers, matrix, true);
        }
    }
}
