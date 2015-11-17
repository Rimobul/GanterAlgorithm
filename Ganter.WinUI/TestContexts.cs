using Ganter.Algorithm;
using System;
using System.Collections.Generic;

namespace Ganter.WinUI
{
    public static class TestContexts
    {
        public static FormalContext GeneratePlanets()
        {
            List<Algorithm.Attribute> attributes = new List<Algorithm.Attribute>()
            {
                new Algorithm.Attribute() {Name = "Mala" },
                new Algorithm.Attribute() {Name = "Stredna" },
                new Algorithm.Attribute() {Name = "Velka" },
                new Algorithm.Attribute() {Name = "Blizko" },
                new Algorithm.Attribute() {Name = "Daleko" },
                new Algorithm.Attribute() {Name = "Ano - mesiac" },
                new Algorithm.Attribute() {Name = "Nie - mesiac" }
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
            List<Algorithm.Attribute> attributes = new List<Algorithm.Attribute>()
            {
                new Algorithm.Attribute() {Name="brilliant white" },
                new Algorithm.Attribute() {Name="fine white" },
                new Algorithm.Attribute() {Name="white" },
                new Algorithm.Attribute() {Name="high-performance copiers" },
                new Algorithm.Attribute() {Name="copiers" },
                new Algorithm.Attribute() {Name="liquid toner copiers" },
                new Algorithm.Attribute() {Name="type writers" },
                new Algorithm.Attribute() {Name="double sided" },
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

        public static FormalContext GenerateOneToSeven()
        {
            List<Algorithm.Attribute> attributes = new List<Algorithm.Attribute>()
            {
                new Algorithm.Attribute() {Name="composit" },
                new Algorithm.Attribute() {Name="even" },
                new Algorithm.Attribute() {Name="odd" },
                new Algorithm.Attribute() {Name="prime" },
                new Algorithm.Attribute() {Name="square" }
            };

            List<Item> numbers = new List<Item>()
            {
                new Item() {Name="1" },
                new Item() {Name="2" },
                new Item() {Name="3" },
                new Item() {Name="4" },
                new Item() {Name="5" },
                new Item() {Name="6" },
                new Item() {Name="7" },
            };

            bool[,] matrix = new bool[,]
            {
                {false, false, true, false, true },
                {false, true, false, true, false },
                {false, false, true, true, false },
                {true, true, false, false, true },
                {false, false, true, true, false },
                {true, true, false, false, false },
                {false, false, true, true, false }
            };

            return new FormalContext(attributes, numbers, matrix, true);
        }
    }
}
