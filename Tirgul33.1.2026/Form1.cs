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
using System.Web;
namespace Tirgul33._1._2026
{
    public partial class Form1 : Form
    {
        ManagementEventWatcher watcher;

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
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            watcher = new ManagementEventWatcher
                (
                new WqlEventQuery("SELECT * FROM win32_ProcessStartTrace")
                );

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


                TabList.Invoke((Action)(() =>
                    {
                        TabList.Items.Insert(0, masssage);
                    }));

            };
            watcher.Start();
            btnStart.Text = "Running...";
            btnStart.Enabled = false;   
        }
    }
}
