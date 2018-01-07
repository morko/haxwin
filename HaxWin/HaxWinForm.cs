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
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Emgu.CV;
using Skybound.Gecko;
using System.Diagnostics;
using HttpProxy;
using System.Threading;

namespace HaxWin
{
    public partial class HaxWinForm : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private StdInReciever reciever;
        private HaxWinAutoJoin autoJoin;
        private Proxy proxy;
        private Thread proxyThread;

        private string xulrunnerPath = Path.Combine(
                Directory.GetCurrentDirectory(), "xulrunner");
        public string templatePath = Path.Combine(
                Directory.GetCurrentDirectory(), "templates");

        public HaxWinForm()
        {
            proxy = new Proxy();
            proxyThread = new Thread(new ThreadStart(proxy.Start));
            proxyThread.Start();

            InitializeComponent();

            this.autoJoin = new HaxWinAutoJoin(this);

            var xulrpath = Path.Combine(Directory.GetCurrentDirectory(), "xulrunner");
            Xpcom.Initialize(xulrpath);
            GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            GeckoPreferences.Default["javascript.enabled"] = true;
            GeckoPreferences.Default["network.proxy.type"] = 1;
            GeckoPreferences.Default["network.proxy.http"] = "127.0.0.1";
            GeckoPreferences.Default["network.proxy.http_port"] = 8080;

            browser.DocumentCompleted += new EventHandler(onDocumentLoaded);
        }

        private void HaxWin_Load(object sender, EventArgs e)
        {
            navigate("http://www.haxball.com");

            string[] clargs = Environment.GetCommandLineArgs();
            if (clargs.Length > 1 && clargs[1] == "--com")
            {
                reciever = createReciever();
                reciever.start();
            }

        }
        private StdInReciever createReciever()
        {
            StdInReciever com = new StdInReciever();

            com.MessageRecieved +=
                new StdInReciever.MessageRecievedEventHandler(onMessageRecieved);

            return com;
        }
        private void onMessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            Debug.WriteLine(e.msg.code + " " + e.msg.data);
            try
            {
                switch (e.msg.code)
                {
                    case StdInReciever.NAVIGATE:
                        // call method on UI thread
                        this.Invoke(new Action(() => this.navigate(e.msg.data)));
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception when invoking action on main thread: "
                                + ex.ToString());
                reciever.stop();
            }
        }

        private void HaxWinForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            proxy.Stop();
            if (reciever != null)
                reciever.stop();
            Debug.WriteLine("HaxWinForm closing.");
        }

        private void onDocumentLoaded(object sender, EventArgs e)
        {
            Debug.WriteLine("Loaded.");
            removeHaxBallAds();
        }

        public void navigate(string url)
        {
            if (url.StartsWith("http://www.haxball.com", StringComparison.CurrentCulture))
            {
                browser.Navigate(url);
                urlTextBox.Text = url;
            }
            else
            {
                MessageBox.Show(this,
                                "Only URLs beginning with " +
                                "http://www.haxball.com/ are allowed",
                                "Error");
                urlTextBox.Text = browser.Url.ToString();
            }
        }

        private void removeHaxBallAds()
        {
            GeckoElement body = browser.Document.Body;
            if (body != null)
            {
                GeckoElement ads = browser.Document.GetElementById("ads");
                if (ads != null)
                    body.RemoveChild(ads);

                GeckoElement header = browser.Document.GetElementById("header");
                if (header != null)
                    body.RemoveChild(header);
            }
        }

        public bool isRoomUrl(string url)
        {
            return url.StartsWith("http://www.haxball.com/?roomid=",
                                StringComparison.CurrentCulture);
        }

        public void refresh()
        {
            navigate(browser.Url.ToString());
        }
        public Bitmap screenshot()
        {
            //Create a new bitmap.
            var bmp = new Bitmap(this.Width,
                                this.Height,
                                PixelFormat.Format32bppArgb);
            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmp);

            gfxScreenshot.CopyFromScreen(this.Location.X, this.Location.Y,
                                        0, 0, bmp.Size,
                                        CopyPixelOperation.SourceCopy);

            return bmp;
        }
        public Mat screen2Mat()
        {
            Mat img = new Mat();
            CvInvoke.Imdecode(
                    screen2Byte(),
                    Emgu.CV.CvEnum.LoadImageType.Grayscale,
                    img);

            return img;
        }
        public byte[] screen2Byte()
        {
            Bitmap bmp = screenshot();
            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                result = stream.ToArray();
            }
            return result;
        }
        public HaxCoords findImage(string tmpImg)
        {
            Mat img = screen2Mat();

            Mat template = CvInvoke.Imread(tmpImg,
                            Emgu.CV.CvEnum.LoadImageType.Grayscale);

            Mat result = new Mat();
            int result_rows = img.Height - template.Height + 1;
            int result_cols = img.Width - template.Width + 1;
            result.Create(result_rows, result_cols,
                        Emgu.CV.CvEnum.DepthType.Cv32F, 1);

            CvInvoke.MatchTemplate(img, template, result,
                    Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            double[] minVal, maxVal;
            Point[] minLoc, maxLoc;

            result.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

            if (maxVal[0] > 0.85)
            {
                return new HaxCoords(maxLoc[0].X, maxLoc[0].Y,
                                    template.Width, template.Height);
            }
            else
            {
                return null;
            }

        }

        public void click(Point p)
        {
            Point oldPos = Cursor.Position;
            Cursor.Position = p;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP,
                        (uint)p.X, (uint)p.Y, 0, 0);
            Cursor.Position = oldPos;
        }
        public void clickRelative(Point p)
        {
            Point screenPoint = new Point(this.Location.X + p.X,
                                        this.Location.Y + p.Y);
            click(screenPoint);
        }
        /*
         * For AutoJoin to uncheck the button from different thread.
         */
        public void unCheckAutoJoinButton()
        {
            autoJoinButton.Checked = false;
        }

        private void autoJoinButton_CheckedChanged(object sender, EventArgs e)
        {
            if (autoJoinButton.Checked)
            {
                if (!isRoomUrl(browser.Url.ToString()))
                {
                    MessageBox.Show(this,
                                    "You must point the browser to a room link " +
                                    "before you can use the AutoJoin feature.",
                                    "Error");
                    autoJoinButton.Checked = false;
                    return;
                }
                if (!autoJoin.started)
                {
                    autoJoin.startJoining();
                }
                else
                {
                    MessageBox.Show(this,
                                    "Previous AutoJoin is still running. " +
                                    "Wait a moment.",
                                    "Error");
                    autoJoinButton.Checked = false;
                    return;
                }
            }
            else
            {
                if (autoJoin.started)
                    autoJoin.stopJoining();
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            navigate(urlTextBox.Text);
        }

        private void alwaysOnTopButton_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopButton.Checked ? true : false;
        }

        private void urlTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if enter key pressed
            if (e.KeyChar == (char)13)
            {
                navigate(urlTextBox.Text);
            }
        }

        private void urlTextBox_DoubleClick(object sender, EventArgs e)
        {
            urlTextBox.SelectAll();
        }

        private void chatOverlayButton_CheckedChanged(object sender, EventArgs e)
        {
            chatOverlayPanel.Visible = chatOverlayButton.Checked ? true : false;
        }
    }
    /*
     * x and y point to the top left corner of the object 
     */
    public class HaxCoords
    {
        public int x, y, width, height;

        public HaxCoords(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        public Point ToPoint()
        {
            return new Point(x, y);
        }

        public Point ToCenterPoint()
        {
            return new Point(x + (width / 2), y + (height / 2));
        }

    }
}
