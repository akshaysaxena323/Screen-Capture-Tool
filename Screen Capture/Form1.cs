// ReSharper disable InconsistentNaming

namespace VisioForge
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisioForge.Controls.UI.WinForms;
    using VisioForge.Types;
    using VisioForge.Types.OutputFormat;
    using VisioForge.Types.Sources;
    using VisioForge.Types.VideoEffects;

    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private bool IsWindows7OrNewer()
        {
            var version = Environment.OSVersion.Version;
            if (version.Major > 6)
            {
                return true;
            }

            if (version.Major == 6 && version.Minor >= 1)
            {
                return true;
            }

            return false;
        }

        public Form1()
        {
            InitializeComponent();
        }

      
     

        private void btSelectOutput_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                edOutput.Text = saveFileDialog1.FileName;
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
           VideoCapture1.Video_Sample_Grabber_Enabled = true;

            // from screen
            VideoCapture1.Screen_Capture_Source = new ScreenCaptureSourceSettings();
            VideoCapture1.Screen_Capture_Source.FullScreen = true;
         
            VideoCapture1.Screen_Capture_Source.Mode = VFScreenCaptureMode.Screen;
            VideoCapture1.Screen_Capture_Source.FrameRate = (float)Convert.ToDouble(edScreenFrameRate.Text);


            VideoCapture1.Audio_RecordAudio = false;
            VideoCapture1.Audio_PlayAudio = false;
          
            // apply capture params
            VideoCapture1.Video_Effects_Enabled = true;
            VideoCapture1.Video_Effects_Clear();

            VideoCapture1.Mode = VFVideoCaptureMode.ScreenCapture;
            VideoCapture1.Output_Filename = edOutput.Text;
           
               var mp4Output = new VFMP4Output();
            
                        mp4Output.MP4Mode = VFMP4Mode.v11;
                

                    VideoCapture1.Output_Format = mp4Output;
            
            VideoCapture1.Start();
        }
        
        private void btStop_Click(object sender, EventArgs e)
        {
            VideoCapture1.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text += " (SDK v" + VideoCapture1.SDK_Version + ", " + VideoCapture1.SDK_State + ")";

            edOutput.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VisioForge\\" + "output.mp4";
           

            if (VideoCapture.Filter_Supported_EVR())
            {
                VideoCapture1.Video_Renderer.Video_Renderer = VFVideoRenderer.EVR;
            }
            else if (VideoCapture.Filter_Supported_VMR9())
            {
                VideoCapture1.Video_Renderer.Video_Renderer = VFVideoRenderer.VMR9;
            }
            else
            {
                VideoCapture1.Video_Renderer.Video_Renderer = VFVideoRenderer.VideoRenderer;
            }
        }

    }
}

// ReSharper restore InconsistentNaming
