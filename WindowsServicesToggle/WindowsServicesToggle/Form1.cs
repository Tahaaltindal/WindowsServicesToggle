using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Timers;

namespace Servislerr
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = false;
            GetWindowServices();
            GetWindowServices();


        }


        private void GetWindowServices()
        {
            //throw new NotImplementedException();
            ServiceController[] service;
            service = ServiceController.GetServices();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            for (int i = 0; i < service.Length; i++)
            {
                comboBox1.Items.Add(service[i].ServiceName);
                comboBox2.Items.Add(service[i].ServiceName);
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //kaldığım yer           
            try
            {
                string tarih = DateTime.Now.ToShortDateString();
                string saat = DateTime.Now.ToLongTimeString();
                //
                var singleservice = new ServiceController((string)e.Argument);
               // if ((singleservice.Status.Equals(ServiceControllerStatus.Stopped)) || (singleservice.Status.Equals(ServiceControllerStatus.StopPending)))
                if ((singleservice.Status.Equals(ServiceControllerStatus.Running)) || (singleservice.Status.Equals(ServiceControllerStatus.Paused)))
                {


                    singleservice.Stop();
                    listBox1.Items.Add(comboBox1.Text + " Servis durduruldu." + tarih + " " + saat + " \n ");
                    Thread.Sleep(5000);
                    singleservice.Start();
                    listBox1.Items.Add(comboBox1.Text + " servisi tekrar başlatıldı !." + tarih + " " + saat + "\n");
                }
                if ((singleservice.Status.Equals(ServiceControllerStatus.Stopped)) || (singleservice.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    singleservice.Start();
                    listBox1.Items.Add(comboBox1.Text + " Servis çalıştırıldı." + tarih + " " + saat + " \n ");
                }
                // timer1.Stop();
            }

            catch
            {
                MessageBox.Show("Bir hata oluştu.");
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString(), "Servis Başlatılamadı.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            listBox1.Items.Add("Program Başlatıldı." + "\n");//listbox'a log girildi
            if (checkBox1.Checked == true)
            {
                label3.Text = ((int.Parse(textBox1.Text)) * 60).ToString();
                timer1.Start();
            }

            this.progressBar1.Visible = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string tarih = DateTime.Now.ToShortDateString();
                string saat = DateTime.Now.ToLongTimeString();
                timer1.Stop();
                this.backgroundWorker2.RunWorkerAsync(comboBox2.Text);
                listBox1.Items.Add(comboBox1.Text + " Servis Durduruldu.  " + tarih + " " + saat + "\n");
                progressBar1.Hide();
            }
            catch (Exception)
            {
                MessageBox.Show("Hata.");
               
            }
           
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = comboBox1.Text;
            ServiceController singleservice = new ServiceController(label4.Text);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            /*label3.Text = ((int.Parse(textBox1.Text)) * 60).ToString();
            this.backgroundWorker1.RunWorkerAsync(label4.Text);*/


            label3.Text = ((int.Parse(label3.Text) - 1).ToString());
            if (int.Parse(label3.Text) < 10)
            {
                label3.ForeColor = Color.Red;

            }
            else
            {
                label3.ForeColor = Color.Black;
            }
            if (int.Parse(label3.Text) < 1)
            {
                label3.Text = ((int.Parse(textBox1.Text)) * 60).ToString();
                this.backgroundWorker1.RunWorkerAsync(label4.Text);
                    



            }

        }

        private void timerlog_Tick(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileStream fs = File.Create(path + "\\LOG.txt");
            fs.Close();
            String[] sitesDizi = new String[listBox1.Items.Count];
            listBox1.Items.CopyTo(sitesDizi, 0);
            System.IO.File.WriteAllLines(path + "\\LOG.txt", sitesDizi);
        }

        private void backgroundWorker2_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                string tarih = DateTime.Now.ToShortDateString();
                string saat = DateTime.Now.ToLongTimeString();
                //
                var singleservice = new ServiceController((string)e.Argument);
                // if ((singleservice.Status.Equals(ServiceControllerStatus.Stopped)) || (singleservice.Status.Equals(ServiceControllerStatus.StopPending)))
                if (singleservice.Status.Equals(ServiceControllerStatus.Running))
                {


                    singleservice.Stop();
                    listBox1.Items.Add(comboBox1.Text + " Servis Durduruldu.  " + tarih + " " + saat + "\n");


                }
            }
            catch (Exception)
            {

                //MessageBox.Show("Hata");
            }
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
           
            try
            {
                string tarih = DateTime.Now.ToShortDateString();
                string saat = DateTime.Now.ToLongTimeString();
                //
                var singleservice = new ServiceController((string)e.Argument);
                // if ((singleservice.Status.Equals(ServiceControllerStatus.Stopped)) || (singleservice.Status.Equals(ServiceControllerStatus.StopPending)))
                if (singleservice.Status.Equals(ServiceControllerStatus.Stopped))
                {


                    singleservice.Start();
                    //Thread.Sleep(5000);
                    listBox2.Items.Add(comboBox1.Text + " Servis Çalıştırıldı.  " + tarih + " " + saat + "\n");


                }
            }
            catch (Exception)
            {

               MessageBox.Show("Hata");
            }
            
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString(), "Servis Başlatılamadı.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.button3.Enabled = false;
            this.backgroundWorker3.RunWorkerAsync(comboBox2.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceController singleservice = new ServiceController(comboBox2.Text);
            if ((singleservice.Status.Equals(ServiceControllerStatus.Stopped)) || (singleservice.Status.Equals(ServiceControllerStatus.StopPending)))
            {

              //  button4.Enabled = false;
               // button3.Enabled = true;
            }
                else
                {
               // button4.Enabled = true;
               // button3.Enabled = false;
                }
            
            }

        private void button4_Click(object sender, EventArgs e)
        {
            //this.button3.Enabled = false;
            this.backgroundWorker4.RunWorkerAsync(comboBox2.Text);

        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                string tarih = DateTime.Now.ToShortDateString();
                string saat = DateTime.Now.ToLongTimeString();
                //
                var singleservice = new ServiceController((string)e.Argument);
                // if ((singleservice.Status.Equals(ServiceControllerStatus.Stopped)) || (singleservice.Status.Equals(ServiceControllerStatus.StopPending)))
                if (singleservice.Status.Equals(ServiceControllerStatus.Running))
                {


                    singleservice.Stop();
                    listBox2.Items.Add(comboBox1.Text + " Servis Durduruldu.  " + tarih + " " + saat + "\n");


                }
            }
            catch (Exception)
            {

                MessageBox.Show("Hata");
            }
            
         
        }



        public int i { get; set; }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString(), "Servis Başlatılamadı.");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


   
        }
    }

    