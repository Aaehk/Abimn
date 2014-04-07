﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Abimn
{
    class Cell
    {
        public byte IdFloor
        {
            get { return _idFloor; }
            set { _idFloor = value; }
        }
        private byte _idFloor;

        public byte IdDeco
        {
            get { return _idDeco; }
            set { _idDeco = value; }
        }
        private byte _idDeco;

        public bool Blocking
        {
            get { return _blocking; }
            set { _blocking = value; }
        }
        private bool _blocking;

        public Cell(bool blocking, byte idFloor, byte idDeco = 0)
        {
            Blocking = blocking;
            IdFloor = idFloor;
            IdDeco = idDeco;
        }

        public void Draw(Pos pos, Center center = Center.None, bool fighting = false)
        {
            if (fighting)
                G.tiles[(int)Tiles.Fight][IdFloor - 1].Draw(pos, center);
            else
            {
                G.tiles[(int)Tiles.Main][IdFloor - 1].Draw(pos, center);
                if (IdDeco != 0)
                    G.tiles[(int)Tiles.MainDeco][IdDeco].Draw(pos, center);
            }
        }
    }
}
