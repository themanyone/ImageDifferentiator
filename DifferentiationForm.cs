using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using System.Media;
using System.Numerics;

namespace ImageDifferentiator
{
    public partial class DifferentiationForm : Form
    {
        public DifferentiationForm()
        {
            InitializeComponent();
        }

        private (int max, int min) GetMaxMIn(Bitmap bitMap)
        {
            //make a pass to find min and max
            int max = 0;
            int min = int.MaxValue;

            for (int y = 0; y < bitMap.Height; y++)
            {
                for (int x = 0; x < bitMap.Width; x++)
                {
                    var pixel = bitMap.GetPixel(x, y);
                    max = pixel.R > max ? pixel.R : max;
                    max = pixel.G > max ? pixel.G : max;
                    max = pixel.B > max ? pixel.B : max;
                    min = pixel.R < min ? pixel.R : min;
                    min = pixel.G < min ? pixel.G : min;
                    min = pixel.B < min ? pixel.B : min;
                }
            }
            return (max, min);
        }

        private Bitmap Normalize(Image image)
        {
            Bitmap bitMap = new Bitmap(image);

            (var max, var min) = GetMaxMIn(bitMap);

            //subtract the min from all, then calculate a factor that would raise the max to 255
            double factor = 255 / (double)(max - min);
            if (factor == 1 || factor == double.PositiveInfinity)
            {
                return bitMap;
            }

            for (int y = 0; y < bitMap.Height; y++)
            {
                for (int x = 0; x < bitMap.Width; x++)
                {
                    var pixel = bitMap.GetPixel(x, y);
                    var newPixel = Color.FromArgb(
                        (int)((pixel.R - min) * factor),
                        (int)((pixel.G - min) * factor),
                        (int)((pixel.B - min) * factor));
                    bitMap.SetPixel(x, y, newPixel);
                }
            }

            return bitMap;
        }

        private void btImage1_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog1.FileName);
                if (image != null)
                {
                    if (cbNorm.Checked)
                    {
                        image = Normalize(image);
                    }
                    pictureBox1.Image = image;
                }
            }

        }

        private void btImage2_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog1.FileName);
                if (image != null)
                {
                    if (cbNorm.Checked)
                    {
                        image = Normalize(image);
                    }
                    pictureBox2.Image = image;
                }
            }
        }

        void SizeAndPlacePicture3()
        {
            pictureBox3.Size = pictureBox2.Size;

            pictureBox3.Location = new Point(splitContainer2.SplitterDistance, pictureBox3.Location.Y);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            SizeAndPlacePicture3();
        }

        private void btCalc_Click(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SizeAndPlacePicture3();
        }

        private bool _decoding = false;
        DateTime _lastChecked = DateTime.Now;

        MjpegDecoder mjpeg = null;
        int _detections = 0;

        private void btStream_Click(object sender, EventArgs e)
        {
            if (_decoding == true)
            {
                mjpeg.StopStream();
                _decoding = false;
                btStream.BackColor = Color.LightYellow;
                return;
            }

            _lastChecked = DateTime.Now;
            mjpeg = new MjpegDecoder();
            mjpeg.FrameReady += mjpeg_FrameReady;
            mjpeg.Error += mjpeg_Error;
            mjpeg.ParseStream(new Uri(tbURL.Text));
            _decoding = true;
            btStream.BackColor = Color.LightGreen;
            _detections = 0;
            _restarts = 0;
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            if (DateTime.Now > _lastChecked.AddMilliseconds((double)nudChechEvery.Value))
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(e.FrameBuffer);
                stream.Position = 0;
                _imageQueue.Enqueue(Image.FromStream(stream));
                _lastChecked = DateTime.Now;
            }
        }

        void mjpeg_Error(object sender, ErrorEventArgs e)
        {
            //MessageBox.Show(e.Message);
            lbInfo.Text = e.Message;

            //this can't be good
            //let's try to restart it
            //seems like the slowness of processing the queue messes up the restart
            //lets let it play out
            if (_imageQueue.Count() == 0)
            {
                RestartStream();
            }
        }

        private int _restarts = 0;

        private void RestartStream()
        {
            mjpeg.StopStream();
            _decoding = false;
            btStream.BackColor = Color.Pink;
            Thread.Sleep(500);
            mjpeg = new MjpegDecoder();
            mjpeg.FrameReady += mjpeg_FrameReady;
            mjpeg.Error += mjpeg_Error;
            mjpeg.ParseStream(new Uri(tbURL.Text));
            btStream.BackColor = Color.LightGreen;
            _decoding = true;
            _restarts++;
            lbRestarts.Text = _restarts.ToString();
        }

        Queue<Image> _imageQueue = new Queue<Image>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_imageQueue.Count > 0)
            {
                pictureBox2.Image = pictureBox1.Image;
                pictureBox1.Image = cbNorm.Checked ? Normalize(_imageQueue.Dequeue()) : _imageQueue.Dequeue();
                lbQueueSize.Text = _imageQueue.Count.ToString();
                if (cbDoCalc.Checked)
                {
                    CalculateDifference();
                }
            }
            //lets see if the stream has stopped
            if (_decoding)
            {
                if (DateTime.Now.Subtract(_lastChecked).TotalSeconds > 3)
                {
                    // seems like the slowness of processing the queue messes up the restart
                    //lets let it play out
                    if (_imageQueue.Count() == 0)
                    {
                        RestartStream();
                    }
                }
            }
        }

        SoundPlayer simpleSound = new SoundPlayer(@"clank.wav");

        private void CalculateDifference()
        {
            if (pictureBox1.Image == null || pictureBox2.Image == null
                || pictureBox1.Image.Width != pictureBox2.Image.Width)
                return;

            Bitmap bitMap1 = new Bitmap(pictureBox1.Image);
            Bitmap bitMap2 = new Bitmap(pictureBox2.Image);

            Bitmap diff = new Bitmap(bitMap1.Width, bitMap1.Height);

            for (int y = 0; y < bitMap1.Height; y++)
            {
                for (int x = 0; x < bitMap1.Width; x++)
                {
                    var pixel1 = bitMap1.GetPixel(x, y);
                    var pixel2 = bitMap2.GetPixel(x, y);
                    var color = Color.FromArgb(
                        //Math.Abs(pixel1.A - pixel2.A),
                        Math.Abs(pixel1.R - pixel2.R),
                        Math.Abs(pixel1.G - pixel2.G),
                        Math.Abs(pixel1.B - pixel2.B)
                        );
                    diff.SetPixel(x, y, color);
                }
            }

            if (cbNormalizeDifference.Checked)
            {
                diff = Normalize((Image)diff);
            }
            pictureBox3.Image = diff;

            int overThresh = 0;
            for (int y = 0; y < diff.Height; y++)
            {
                for (int x = 0; x < diff.Width; x++)
                {
                    var pixel = diff.GetPixel(x, y);
                    bool whiteOut = false;
                    if (pixel.R > nudThresh.Value)
                    {
                        overThresh++;
                        whiteOut = true;
                    }
                    if (pixel.G > nudThresh.Value)
                    {
                        overThresh++;
                        whiteOut = true;
                    }
                    if (pixel.B > nudThresh.Value)
                    {
                        overThresh++;
                        whiteOut = true;
                    }
                    if (cbWhiteOut.Checked)
                    {
                        if (whiteOut)
                        {
                            diff.SetPixel(x, y, Color.White);
                        }
                        else
                        {
                            diff.SetPixel(x, y, Color.Black);
                        }
                    }
                }
            }
            tbOverThresh.Text = overThresh.ToString();
            double total = diff.Height * diff.Width * 3;
            var percentMotion = (overThresh / (decimal)total);
            tbPct.Text = percentMotion.ToString();

            lbInfo.Text = $"total={total.ToString()} width={diff.Width} height={diff.Height}";

            if (percentMotion > nudPctThresh.Value)
            {
                tbDetected.Text = "Motion";
                tbDetected.BackColor = Color.LightGreen;
                _detections++;
                lbDetCnt.Text = _detections.ToString();

                if (cbPlayTone.Checked)
                {
                    simpleSound.Play();
                }

                if (cbSave.Checked)
                {
                    if (Directory.Exists(folderBrowserDialog1.SelectedPath))
                    {
                        var now = DateTime.Now;
                        var fileNameRoot = $"{now.Year}-{now.Month.ToString("00")}-{now.Day.ToString("00")}-" +
                            $"{now.Hour.ToString("00")}-{now.Minute.ToString("00")}-{now.Second.ToString("00")}-{now.Millisecond.ToString("000")}";
                        var fileName = Path.Combine(folderBrowserDialog1.SelectedPath, fileNameRoot + ".jpg");
                        using (var outFile = File.OpenWrite(fileName))
                        {
                            pictureBox1.Image.Save(outFile, ImageFormat.Jpeg);
                        }
                        if (Directory.Exists(folderBrowserDialog1.SelectedPath + "\\diff"))
                        {
                            fileName = Path.Combine(folderBrowserDialog1.SelectedPath + "\\diff", fileNameRoot + ".jpg");
                            using (var outFile = File.OpenWrite(fileName))
                            {
                                pictureBox3.Image.Save(outFile, ImageFormat.Jpeg);
                            }
                        }
                    }
                }
            }
            else
            {
                tbDetected.Text = "No Motion";
                tbDetected.BackColor = Color.LightYellow;
            }
        }

        private void btFolder_Click(object sender, EventArgs e)
        {
            if (_decoding)
            {
                MessageBox.Show("Be quick! Streaming is enabled.");
            }
            folderBrowserDialog1.ShowDialog();
        }
    }
}
