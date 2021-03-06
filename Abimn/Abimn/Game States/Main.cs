﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;


namespace Abimn
{
    /// <summary>
    /// Ecran de vue sur map
    /// </summary>
    public class Main : GameType
    {
        private Map _backmap;
        private Pos _poshero, _shift;
        private int _idhero, _idmap, _time;
        private string _dir;
        float _timeSinceMove = 0f;
        float _timeStart = 0f;
        private Entity EntityHero;
        private bool event1;
        private bool shitIsDone = false;

        public Main() : base(true) { }

        public override void Initialize()
        {

            Music.Play("1");
            this._idmap = 3;
            this._backmap = new Map(_idmap);
            this._poshero = _backmap.StartPos;
            this._idhero = 0;
            this._dir = "right";
            this._shift = new Pos();
            this._time = 0;
            EntityHero = new Entity(new Pos((9 * 50), (7 * 50)));
            this.event1 = false;

        }



        /// <summary>
        /// permet de bouger le HERO d'une case à l'autre en fonction de la touche
        /// fonctionne avec zqsd et flèches(haut, bas, gauche, droite)
        /// </summary>
        public void MoveHeros()
        {
            _time++;

            if (_time > 10)//changement de l'indice pour les sprites
            {
                if (E.IsDown(Keys.B)) //courrir
                    _idhero = (_idhero + 1) % 3;

                else
                    _idhero = (_idhero + 1) % 2;

                _time = 0;
            }


            if (E.IsPushed(Keys.Q) || E.IsPushed(Keys.Left) || E.IsDown(Keys.Q) || E.IsDown(Keys.Left))
            {
                _dir = "left";
                Pos NextStep = new Pos(_poshero.X - 1, _poshero.Y);
                if (_backmap.CanMoveOn(NextStep))
                    _shift.X -= 3;
                else
                {
                    if (_shift.X <= 4)
                    {
                        _shift.X = 0;
                        EntityHero.Pos.X -= 5;
                    }
                    else
                        _shift.X -= 3;
                }
            }
            else
            {
                if (E.IsPushed(Keys.D) || E.IsPushed(Keys.Right) || E.IsDown(Keys.D) || E.IsDown(Keys.Right))
                {
                    _dir = "right";
                    Pos NextStep = new Pos(_poshero.X + 1, _poshero.Y);
                    if (_backmap.CanMoveOn(NextStep))
                    { _shift.X += 3; }

                    else
                    {
                        if (_shift.X >= -4)
                        {
                            _shift.X = 0;
                            EntityHero.Pos.X += 5;
                        }
                        else
                            _shift.X += 3;
                    }
                }
                else
                {
                    if (E.IsPushed(Keys.Z) || E.IsPushed(Keys.Up) || E.IsDown(Keys.Z) || E.IsDown(Keys.Up))
                    {
                        _dir = "up";
                        Pos NextStep = new Pos(_poshero.X, _poshero.Y - 1);
                        if (_backmap.CanMoveOn(NextStep))
                            _shift.Y -= 3;
                        else
                        {
                            if (_shift.Y <= 4)
                            {
                                _shift.Y = 0;
                                EntityHero.Pos.Y -= 5;
                            }
                            else
                                _shift.Y -= 3;
                        }
                    }
                    else
                    {
                        if (E.IsPushed(Keys.S) || E.IsPushed(Keys.Down) || E.IsDown(Keys.S) || E.IsDown(Keys.Down))
                        {
                            _dir = "down";
                            Pos NextStep = new Pos(_poshero.X, _poshero.Y + 1);
                            if (_backmap.CanMoveOn(NextStep))
                                _shift.Y += 3;
                            else
                            {
                                if (_shift.Y >= -4)
                                {
                                    _shift.Y = 0;
                                    EntityHero.Pos.Y += 5;
                                }
                                else
                                    _shift.Y += 3;
                            }
                        }

                        else
                            _idhero = 0;
                    }
                }
            }

        }

        /// <summary>
        /// Ajustement de changement de case
        /// Necessaire pour des colisions nettes (plus ou moins)
        /// </summary>
        public void MoveTile()
        {
            if (_shift.X <= -25)
            {
                _shift.X = 25;
                _poshero.X--;
            }
            if (_shift.X >= 27)
            {
                _shift.X = -23;
                _poshero.X++;
            }
            if (_shift.Y <= -40)
            {
                _shift.Y = 10;
                _poshero.Y--;
            }
            if (_shift.Y >= 15)
            {
                _shift.Y = -35;
                _poshero.Y++;
            }
        }

        /// <summary>
        /// Regarde les trois cases en face du personnage
        /// Retourne vrai si elles sont occupées par un marchand
        /// </summary>
        public int Interact()
        {
            Pos FrontTile = new Pos();

            if (FrontTile.X == 0 && FrontTile.X == 49)
            {
                if (_dir == "left")
                    FrontTile = new Pos(_poshero.X - 1, _poshero.Y);
                if (_dir == "right")
                    FrontTile = new Pos(_poshero.X + 1, _poshero.Y);
                if (_dir == "down")
                    FrontTile = new Pos(_poshero.X, _poshero.Y + 1);
                if (_dir == "up")
                    FrontTile = new Pos(_poshero.X, _poshero.Y - 1);
                if (FrontTile.X == 0 && FrontTile.X == 49 && _backmap.Decoration(FrontTile) == 5)
                    return 5;
            }
            else
            {
                Pos FrontTileLeft = new Pos();
                Pos FrontTileRight = new Pos();
                Pos FrontTileFront = new Pos();

                if (_dir == "left")
                {
                    FrontTile = new Pos(_poshero.X - 1, _poshero.Y);
                    FrontTileLeft = new Pos(FrontTile.X, FrontTile.Y - 1);
                    FrontTileRight = new Pos(FrontTile.X, FrontTile.Y + 1);
                    FrontTileFront = new Pos(FrontTile.X - 1, FrontTile.Y);
                }
                if (_dir == "right")
                {
                    FrontTile = new Pos(_poshero.X + 1, _poshero.Y);
                    FrontTileLeft = new Pos(FrontTile.X, FrontTile.Y - 1);
                    FrontTileRight = new Pos(FrontTile.X, FrontTile.Y + 1);
                    FrontTileFront = new Pos(FrontTile.X + 1, FrontTile.Y);
                }
                if (_dir == "down")
                {
                    FrontTile = new Pos(_poshero.X, _poshero.Y + 1);
                    FrontTileLeft = new Pos(FrontTile.X - 1, FrontTile.Y);
                    FrontTileRight = new Pos(FrontTile.X + 1, FrontTile.Y);
                    FrontTileFront = new Pos(FrontTile.X, FrontTile.Y + 1);
                }
                if (_dir == "up")
                {
                    FrontTile = new Pos(_poshero.X, _poshero.Y - 1);
                    FrontTileLeft = new Pos(FrontTile.X - 1, FrontTile.Y);
                    FrontTileRight = new Pos(FrontTile.X + 1, FrontTile.Y);
                    FrontTileFront = new Pos(FrontTile.X, FrontTile.Y - 1);
                }

                if (_backmap.Decoration(FrontTile) == 5 ||
                    _backmap.Decoration(FrontTileLeft) == 5 ||
                    _backmap.Decoration(FrontTileRight) == 5 ||
                    _backmap.Decoration(FrontTileFront) == 5) //id du renard Mesmer
                    return 5;

                if (_backmap.Decoration(FrontTile) == 7 ||
                    _backmap.Decoration(FrontTileLeft) == 7 ||
                    _backmap.Decoration(FrontTileRight) == 7 ||
                    _backmap.Decoration(FrontTileFront) == 7) //id du PNJ 
                    return 7;

                if (_backmap.Decoration(FrontTile) == 8 ||
                    _backmap.Decoration(FrontTileLeft) == 8 ||
                    _backmap.Decoration(FrontTileRight) == 8) //id du PNJ 
                    return 8;

                if (_backmap.Decoration(FrontTile) == 9 ||
                   _backmap.Decoration(FrontTileLeft) == 9 ||
                   _backmap.Decoration(FrontTileRight) == 9) //id du PNJ 
                    return 9;
            }
            return 0;
        }



        /// <summary>
        /// Appelle une nouvelle map
        /// </summary>
        public void Travel()
        {
            Pos HighTile = new Pos(_poshero.X, _poshero.Y - 1);
            Pos DownTile = new Pos(_poshero.X, _poshero.Y + 1);
            if ((_backmap.Decoration(HighTile) == 4 && (E.IsPushed(Keys.Up) || E.IsPushed(Keys.Z))) || (_backmap.Decoration(DownTile) == 4 && (E.IsPushed(Keys.Down) || E.IsPushed(Keys.S))))
            {
                _idmap = 2;
                _backmap = new Map(_idmap);
                _shift = new Pos(0, 0);
                _poshero = _backmap.StartPos;
                _idhero = 0;
                G.currentGame.Push(new Abimn.Game_States.Transition());

            }

            if (((_backmap.Decoration(HighTile) == 3 && (E.IsDown(Keys.Up) || E.IsDown(Keys.Z))) || (_backmap.Decoration(DownTile) == 3 && (E.IsDown(Keys.Down) || E.IsDown(Keys.S)))) && _idmap == 2)
            {
                _idmap = 1;
                _poshero = new Pos(45, 46);
                _backmap = new Map(_idmap);
                _idhero = 0;
                _dir = "down";
                _shift = new Pos(0, 0);
                G.currentGame.Push(new Abimn.Game_States.Transition());

            }

            if (((_backmap.Decoration(HighTile) == 3 && (E.IsDown(Keys.Up) || E.IsDown(Keys.Z))) || (_backmap.Decoration(DownTile) == 3 && (E.IsDown(Keys.Down) || E.IsDown(Keys.S)))) && _idmap == 3)
            {
                _idmap = 1;
                _backmap = new Map(_idmap);
                _idhero = 0;
                _dir = "right";
                _poshero = new Pos(0, 25);
                _shift = new Pos(0, 0);
                G.currentGame.Push(new Abimn.Game_States.Transition());

            }

            if (((_backmap.Decoration(_poshero) == 6 && (E.IsDown(Keys.Up) || E.IsDown(Keys.Z))) || (_backmap.Decoration(_poshero) == 6 && (E.IsDown(Keys.Down) || E.IsDown(Keys.S)))) && _idmap == 4)
            {
                _idmap = 1;
                _backmap = new Map(_idmap);
                _idhero = 0;
                _dir = "down";
                _poshero = new Pos(39, 03);
                _shift = new Pos(0, 0);
                G.currentGame.Push(new Abimn.Game_States.Transition());

            }
            if (((_backmap.Decoration(_poshero) == 6 && (E.IsDown(Keys.Up) || E.IsDown(Keys.Z))) || (_backmap.Decoration(_poshero) == 6 && (E.IsDown(Keys.Down) || E.IsDown(Keys.S)))) && _idmap == 1  && _poshero.X < 40)
            {

                _idmap = 4;
                Music.Play("map2");
                _backmap = new Map(_idmap);
                _idhero = 0;
                _dir = "up";
                _poshero = new Pos(05, 48);
                _shift = new Pos(0, 0);
                G.currentGame.Push(new Abimn.Game_States.Transition());
            }


            /* if (((_backmap.Decoration(_poshero) == 6 && (E.IsDown(Keys.Right) || E.IsDown(Keys.D)))) && _idmap == 5  && _poshero.X>40)
             {
                 _idmap = 5;
                 _backmap = new Map(_idmap);
                 _idhero = 0;
                 _dir = "right";
                 _poshero = new Pos(01, 25);
                 _shift = new Pos(0, 0);
                 G.currentGame.Push(new Abimn.Game_States.Transition());
             }*/



            if (((_backmap.Decoration(_poshero) == 6 && (E.IsDown(Keys.Left) || E.IsDown(Keys.Q)))) && _idmap == 5)
            {
                _idmap = 1;
                _backmap = new Map(_idmap);
                _idhero = 0;
                _dir = "left";
                _poshero = new Pos(48, 25);
                _shift = new Pos(0, 0);
                G.currentGame.Push(new Abimn.Game_States.Transition());

            }
        }

        public override void Update(GameTime gameTime)
        {
            _timeSinceMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeStart += (float)gameTime.TotalGameTime.TotalSeconds;
            EntityHero = new Entity(new Pos((9 * 50), (7 * 50)));

            Cursor.SetVisibility(false);

            if (!shitIsDone || E.IsPushed(Keys.M))
            {
                shitIsDone = true;
                G.currentGame.Push(new Abimn.Game_States.Interact(1));
            }


            //Change CurrentGame
            if (E.IsPushed(Keys.Escape))
                G.currentGame.Push(new PauseMenu());

            /*if (E.IsPushed(Keys.I))
                G.currentGame.Push(new Inventory());*/

            if (_backmap.Decoration(_poshero) == 2)
            {
                G.currentGame.Push(new Fight());
                _backmap.SetCell(_poshero, new Cell(false, 1, 0));
            }

            if (Interact() == 5 && event1 == false)
            {
                G.currentGame.Push(new Abimn.Game_States.Interact(2));
                event1 = true;
            }

            if (Interact() == 5 && E.IsPushed(Keys.Space))
                G.currentGame.Push(new Abimn.Game_States.Interact(3));

            if (Interact() == 7 && E.IsPushed(Keys.Space))
                G.currentGame.Push(new Abimn.Game_States.Interact(4));

            if (Interact() == 8 && E.IsPushed(Keys.Space))
                G.currentGame.Push(new Abimn.Game_States.Interact(5));

            if (Interact() == 9 && E.IsPushed(Keys.Space))
            {
                G.willFightBoss = true;
                G.currentGame.Push(new Fight());
                G.currentGame.Push(new Abimn.Game_States.Interact(10));
            }


            //Change de map
            Travel();


            //Deplacement
            if (E.IsDown(Keys.B)) //courrir
            {
                if (_timeSinceMove > 0.002f)//règle la vitesse de défilement
                {
                    MoveHeros();
                    MoveHeros();
                    MoveHeros();
                }//Vérifie les touches de mouvements, accumulation pour course (update trop proches)

            }

            else
            {
                if (_timeSinceMove > 0.01f)//règle la vitesse de défilement
                    MoveHeros(); //Vérifie les touches de mouvements

            }
            EntityHero.LoadContent("new hero", _dir + _idhero.ToString());
            MoveTile();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public override void Draw()
        {
            _backmap.Draw(_poshero, _shift);
            EntityHero.Draw();
            base.Draw();
        }
    }
}