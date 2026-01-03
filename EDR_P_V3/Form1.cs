using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics.Tracing;
namespace EDR_P_V3
{

        public partial class Form1 : Form
        {


 
        ManagementEventWatcher watcher;//The watcher


        //List of strange Processes
            string[] badProcesses =
            {
            "mimikatz",
            "powershell",
            "cmd",
            "psexec",
            "procdump",
            "netcat",
            "nc",
            "wmic",
            "certutil",
            "mshta"
        };


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Form1()
            {
                InitializeComponent();
            }

            private void btnStart_Click(object sender, EventArgs e)
            {
                watcher = new ManagementEventWatcher//create a watcher
                    (
                    new WqlEventQuery("SELECT * FROM win32_ProcessStartTrace")// give him a protocol
                    );


            //what should he do when there a new peocess
                watcher.EventArrived += (s, ev) =>
                {
                    string name = ev.NewEvent["ProcessName"].ToString().ToLower();
                    string pid = ev.NewEvent["ProcessID"].ToString();
                    string time = DateTime.Now.ToString("HH,mm,ss");

                    bool isBad = false;

                    foreach (string bad in badProcesses)
                    {
                        if (name.Contains(bad))
                        {
                            isBad = true;
                            break;
                        }
                    }

                    string masssage;
                    if (isBad)
                    {

                        masssage = $"[{time}]  YouFucked ⚠️ ALERT: {name} (PID: {pid})";
                    }
                    else
                    {
                        masssage = $"[{time}]  {name} (PID: {pid})";

                    }


                    ProcessList.Invoke((Action)(() =>
                    {
                        ProcessList.Items.Insert(0, masssage);
                    }));

                };
            //start the program
                watcher.Start();
                btnStart.Text = "Running...";
                btnStart.Enabled = false;
            }
        }
    }
