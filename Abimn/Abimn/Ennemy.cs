﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Abimn
{
    class Ennemy : Entity
    {
        /// <summary>
        /// Récupère ou définit les dégâts de l'ennemy
        /// </summary>
        public int Damages
        {
            get { return _damages; }
            set { _damages = value; }
        }
        private int _damages;

        public int Life
        {
            get { return _life; }
            set { _life = value; }
        }
        private int _life;

        public Ennemy(bool visible = true) : base(visible) { }
        public Ennemy(Pos position, bool visible = true) : base(position, visible) { }
        public Ennemy(Pos position, Vector2 direction, float speed, bool visible = true) : base(position, direction, speed, visible) { }
    }
}
