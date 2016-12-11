/*
HaxWin - lightweight xulrunner based browser for play haxball
Copyright (C) 2016  Oskari Pöntinen <mail.morko@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace HaxWin
{
    class HaxWinAutoJoin
    {
        private HaxWinForm haxWin;
        private Thread joinerThread;
        public volatile bool started = false;
        public volatile bool stopRequest = false;
        private bool roomJoined = false;
        private bool error = false;

        public HaxWinAutoJoin(HaxWinForm haxWin)
        {
            this.haxWin = haxWin;
        }

        public void startJoining()
        {
            stopRequest = false;
            roomJoined = false;
            error = false;
            started = true;
            joinerThread = new Thread(new ThreadStart(this.joinRoom));
            joinerThread.Start();
        }
        public void stopJoining()
        {
            stopRequest = true;
        }
        public void joinRoom()
        {
            if (findButton("play_button.png") == null)
            {
                haxWin.Invoke(new Action(() => MessageBox.Show(haxWin, 
                                                "Please navigate to nickname selection screen " +
                                                "before starting AutoJoin.",
                                                "Error")));
                haxWin.Invoke(new Action(() => haxWin.unCheckAutoJoinButton()));
                started = false;
                return;
            }

            TimeSpan maxDuration = TimeSpan.FromSeconds(20);
            Stopwatch sw1 = Stopwatch.StartNew();
            Stopwatch sw2 = Stopwatch.StartNew();
            int delay = 1000;

            while (!roomJoined && !error && !stopRequest)
            {
                if (findAndClick("play_button.png"))
                {
                    sw2.Restart();
                    while (!roomJoined && !error && !stopRequest)
                    {
                        if (findButton("back_button.png") != null)
                        {
                            // call method on UI thread
                            haxWin.Invoke(new Action(() => haxWin.refresh()));
                            // restart the first clock
                            sw1.Restart();
                            break;
                        }
                        if (findButton("ok_button.png") != null)
                            error = true;
                        else if (findButton("menu_button.png") != null)
                            roomJoined = true;
                        // if we have not found anything for a while then stop
                        if (sw2.Elapsed > maxDuration)
                        {
                            error = true;
                        }
                        Thread.Sleep(delay);
                    }
                }
                // if we have not found play button for a while then stop
                if (sw1.Elapsed > maxDuration)
                {
                    error = true;
                }
                Thread.Sleep(delay);
            }
            if (error)
            {
                haxWin.Invoke(new Action(() => MessageBox.Show(haxWin,
                                                "AutoJoin had to be stopped.",
                                                "Error")));
            }
            haxWin.Invoke(new Action(() => haxWin.unCheckAutoJoinButton()));
            started = false;
        }
        private HaxCoords findButton(string image)
        {
            HaxCoords obj = haxWin.findImage(Path.Combine(
                                            haxWin.templatePath,
                                            image));
            return obj;
        }
        private bool findAndClick(string image)
        {
            HaxCoords button = findButton(image);
            if (button != null)
            {
                haxWin.clickRelative(button.ToCenterPoint());
                return true;
            }
            return false;
        }
    }

}