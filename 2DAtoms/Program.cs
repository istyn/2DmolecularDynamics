﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DAtoms
{
	class Program
	{
        static void Main(string[] args)
        {
            List<Particle> particles = new List<Particle>();
            particles.Add(new Particle(.5, .5, .1, 0, 1, .1, Color.Red));
            particles.Add(new Particle(.8, .5, -.1, 0, 1, .1, Color.Blue));

            PriorityQueue PQ = new PriorityQueue();
            for (int i = 0; i < particles.Count; i++)   //fill PQ with particle-wall collisions
            {
                double cX = particles[i].collidesX();
                double cY = particles[i].collidesY();
                if (!double.IsInfinity(cX) && cX >= 0)
                {
                    PQ.Enqueue(new Event(cX, particles[i], null), cX);
                }
                if (!double.IsInfinity(cY) && cY >= 0)
                {
                    PQ.Enqueue(new Event(cY, particles[i], null), cY);
                }
            }
            for (int i = 0; i < particles.Count - 1; i++)   //fill PQ with particle-particle collisions
            {
                for (int j = i + 1; j < particles.Count; j++)
                {
                    double c = particles[i].collides(particles[j]);
                    if (!Double.IsInfinity(c))
                    {
                        PQ.Enqueue(new Event(c, particles[i], particles[j]), c);
                    }
                }
            }

            var formWindow = new OutputWindow();
            var pictureBox = formWindow.TheBox;
            ParameterizedThreadStart childref = new ParameterizedThreadStart(LaunchWindow);
            Thread childThread = new Thread(childref);
            childThread.Start(formWindow);

            double time = 0;
            while (true)                            //main loop
            {
                double dt = 0;
                Event e;
                PQ.Dequeue(out e, out dt);

                if (e.wasSuperveningEvent())
                {
                    continue;
                }
                if (time != 0)
                {
                    dt = dt - time;

                }
                time = time + dt;

                Particle a = e.getParticle1();
                Particle b = e.getParticle2();
                for (int i = 0; i < particles.Count; i++)   //advance particles to time dt
                {
                    particles[i].update(dt);
                }
                //update velocities
                if (a != null && b != null)         //particle-particle collision
                {
                    a.bounces(b);

                    FutureCollisions(ref particles, ref PQ, a, time);
                    FutureCollisions(ref particles, ref PQ, b, time);
                }
                else if (a != null || b != null)    //particle-wall collision
                {
                    double cY = a.collidesY();
                    double cX = a.collidesX();
                    if (cY == cX)
                    {
                        a.bounceX();
                        a.bounceY();
                    }
                    else if (cY < cX)
                    {
                        a.bounceY();
                    }
                    else
                    {
                        a.bounceX();
                    }
                    FutureCollisions(ref particles, ref PQ, a, time);
                }
                else                                //redraw event
                {
                }

                foreach (Particle particle in particles)
                {
                    Console.Write(particle.ToString());
                }
                Console.WriteLine("\t\tEnd Frame");

                Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (Particle thing in particles)
                    {
                        int radius = (int)(thing.Radius * 100);
                        int x = (int)(thing.PosX * 100);
                        int y = (int)(thing.PosY * 100);

                        g.DrawEllipse(new Pen(thing.Color), x, y, radius, radius);
                    }
                }
                pictureBox.Image = bmp;
                Thread.Sleep(400);
            }
        }

        public static void LaunchWindow(object data) //very narrow, practically threading a needle
        {
            OutputWindow formWindow = (OutputWindow)data;
            if (formWindow == null)
            {
                throw new Exception("object was supposed to be an OutputWindow.");
            }
            formWindow.Show();
            Application.Run();
        }

        private static void FutureCollisions(ref List<Particle> particles, ref PriorityQueue PQ, Particle a, double time)
		{
			double cX = a.collidesX();
			double cY = a.collidesY();
			if (!Double.IsInfinity(cX))
			{
				PQ.Enqueue(new Event(time + cX, a, null), time + cX);
			}
			if (!Double.IsInfinity(cY))
			{
				PQ.Enqueue(new Event(time + cY, a, null), time + cY);
			}
			for (int i = 0; i < particles.Count; i++)
			{
				if (particles[i] == a)  //do not compare same objects
				{
					continue;
				}
				double dt = a.collides(particles[i]);
				if (!Double.IsInfinity(dt))
				{
					PQ.Enqueue(new Event(time + dt, a, particles[i]), time + dt);
					
				}
			}
		}

	}
}
