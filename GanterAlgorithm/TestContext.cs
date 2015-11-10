﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public static class TestContext
    {
        public static FormalContext GenerateFormalContext()
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
    }
}