using System;
using System.Drawing;

namespace _2DAtoms
{
    class Particle
    {
        double rX, rY;  //position in X,Y
        double vX, vY;  //velocity in X,Y
        double mass;
        double r;       //radius
        int count = 0;  //total number of collisions with wall or other particle

        public double PosX { get { return rX; } }
        public double PosY { get { return rY; } }
        public double Radius { get { return r; } }
        public Color Color
        {
            get;
            private set;
        }

        public Particle(double x, double y, double vX, double vY, double mass, double r, Color color)
        {
            rX = x;
            rY = y;
            this.vX = vX;
            this.vY = vY;
            this.mass = mass;
            this.r = r;
            this.Color = color;
        }

        public double collidesX()
        {
            if (vX == 0)
            {
                return Double.PositiveInfinity;
            }
            else if (vX > 0)
            {
                return Math.Round((1 - r - rX) / vX, 10);
            }
            else
            {
                return Math.Round((r - rX) / vX, 10);
            }
        }

        public double collidesY()
        {
            if (vY == 0)
            {
                return Double.PositiveInfinity;
            }
            else if (vY > 0)
            {
                return (1 - r - rY) / vY;
            }
            else
            {
                return (r - rY) / vY;
            }
        }
        //this Particle is i, that Particle is j
        public double collides(Particle j)
        {
            double dx = j.rX - rX;                                      //difference in x position
            double dy = j.rY - rY;                                      //difference in y position
            double dvx = j.vX - vX;                                     //difference in x velocity
            double dvy = j.vY - vY;                                     //difference in y velocity
            double drdr = Math.Round((dx * dx) + (dy * dy), 10);
            double dvdv = Math.Round((dvx * dvx) + (dvy * dvy), 10);
            double dvdr = Math.Round((dvx * dx) + (dvy * dy), 10);
            double d = Math.Round((dvdr * dvdr) - (dvdv * (drdr - ((r + j.r) * (r + j.r)))), 10);

            if (dvdr >= 0)
            {
                return Double.NegativeInfinity;
            }
            else if (d < 0)
            {
                return Double.NegativeInfinity;
            }
            else
            {
                return Math.Round(-1 * ((dvdr + Math.Sqrt(d)) / dvdv), 10);
            }
        }
        //this Particle is i, that Particle is j
        public void bounces(Particle b)
        {
            double sigma = r + b.r;
            double dx = b.rX - rX;
            double dy = b.rY - rY;
            double dvx = b.vX - vX;
            double dvy = b.vY - vY;
            double dvdr = Math.Round((dvx * dx) + (dvy * dy), 10);
            double j = Math.Round((2 * mass * b.mass * dvdr) / (sigma * (mass + b.mass)), 10);
            double jX = Math.Round((j * (b.rX - rX)) / sigma, 10);
            double jY = Math.Round((j * (b.rY - rY)) / sigma, 10);
            vX = Math.Round(vX + jX / mass, 10);
            vY = Math.Round(vY + jY / mass, 10);
            b.vX = Math.Round(b.vX - jX / b.mass, 10);
            b.vY = Math.Round(b.vY - jY / b.mass, 10);

            count++;
            b.count++;
        }
        public void bounceX()
        {
            this.vX = -vX;
            count++;
        }

        public void bounceY()
        {
            this.vY = -vY;
            count++;
        }

        public void update(double t)
        {
            rX = Math.Round(rX + t * vX, 10);
            rY = Math.Round(rY + t * vY, 10);
        }

        public int getCollisionCount()
        {
            return count;
        }

        public override string ToString()
        {
            string toReturn = "";
            toReturn += "(" + rX.ToString("0.00") + ", " + rY.ToString("0.00") + ")";
            return toReturn;
        }
    }
}
