using Modbus.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModbusDeneme
{
    public partial class Form1 : Form
    {
        SerialPort serialPort = new SerialPort();
        ModbusSerialMaster master;
        
       

        public void deactive()
        {
            
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            groupBox7.Enabled = false;
            groupBox8.Enabled = false;
            groupBox9.Enabled = false;

        }
        public void active()
        {
            
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            groupBox7.Enabled = true;
            groupBox8.Enabled = true;
            groupBox9.Enabled = true;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            deactive();
            btnBaglantiKes.Enabled = false;
            if (SerialPort.GetPortNames().Length!=0)
            {
                String[] portlar = SerialPort.GetPortNames();
                cbxPortlar.Text = portlar[0];
                foreach (string port in portlar)
                {
                    cbxPortlar.Items.Add(port);
                }
            }
        }

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.PortName = cbxPortlar.Text; //Port adi
                    serialPort.BaudRate = Convert.ToInt16(txtBaudRade.Text); //Bant genisligi
                    serialPort.DataBits = 8; //data bit
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Open();
                    master = ModbusSerialMaster.CreateRtu(serialPort); //rtu olusturduk
                    active();
                    btnBaglan.Enabled = false;
                    cbxPortlar.Enabled = false;
                    txtBaudRade.Enabled = false;
                    btnBaglantiKes.Enabled = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
             }
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveID = Convert.ToByte(txtSlaveID.Text);
                ushort registerAdres = Convert.ToUInt16(txtRegisterAdresi.Text);
                ushort value = Convert.ToUInt16(txtParametre.Text);
                master.WriteSingleRegister(slaveID, registerAdres, value);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //byte slaveID = Convert.ToByte(txtSlaveID.Text);
            //ushort registerAdres = Convert.ToUInt16(txtRegisterAdresi.Text);
            //ushort value = Convert.ToUInt16(txtParametre.Text);
            // master.WriteSingleRegister(slaveID, registerAdres, value);
            //ushort[] va = new ushort[] { 1 };
            //textBox1.Text = (master.ReadHoldingRegisters(slaveID, registerAdres,1)[0]).ToString();
            //ushort[] data = new ushort[] { 1, 2, 3 }; 
            //master.WriteMultipleRegisters(1, 100,data);// idsi 1 olan 400100 registerina sirayla data dizisini yaz
            //master.WriteSingleCoil(1, 100, true);//idsi 1 olan 101 cikisina 1 yaz
            //var value = master.ReadInputs(1, 100, 1);            
            //var value = master.ReadCoils(1, 100, 2); // idsi 1 olan 100 ve 101 adreslerini oku
            // textBox1.Text = value[0].ToString();
            //var value= master.ReadHoldingRegisters(1, 100, 3);// idsi 1 olan 400100 registerini 100-101-102 oku
            //master.WriteSingleRegister(1, 103, 5);// idsi 1 olan 400103 registerina 2 yaz
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox4.Text);
                ushort startAdres = Convert.ToUInt16(textBox3.Text);
                string[] strArray = textBox2.Text.Split(',');
                ushort[] ushArray = new ushort[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    ushArray[i] = Convert.ToUInt16(strArray[i]);
                }
                master.WriteMultipleRegisters(slaveAdress, startAdres, ushArray);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox7.Text);
                ushort coilAdres = Convert.ToUInt16(textBox6.Text);
                bool value = Convert.ToBoolean(textBox5.Text);
                master.WriteSingleCoil(slaveAdress, coilAdres, value);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox8.Text);
                ushort startAdress = Convert.ToUInt16(textBox9.Text);
                string[] strArray = textBox1.Text.Split(',');
                bool[] data = new bool[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    data[i] = Convert.ToBoolean(strArray[i]);
                }
                master.WriteMultipleCoils(slaveAdress, startAdress, data);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox12.Text);
                ushort startAdress = Convert.ToUInt16(textBox11.Text);
                ushort numberOfPoints = Convert.ToUInt16(textBox10.Text);
                bool[] data = master.ReadCoils(slaveAdress, startAdress, numberOfPoints);
                for (int i = 0; i < data.Length; i++)
                {
                    textBox13.Text += data[i] + " , ";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox17.Text);
                ushort startAdress = Convert.ToUInt16(textBox16.Text);
                ushort numberOfPoints = Convert.ToUInt16(textBox15.Text);
                ushort[] data = master.ReadInputRegisters(slaveAdress, startAdress, numberOfPoints);
                for (int i = 0; i < data.Length; i++)
                {
                    textBox14.Text += data[i] + " , ";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void btnBaglantiKes_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Close();
                deactive();
                btnBaglan.Enabled = true;
                cbxPortlar.Enabled = true;
                txtBaudRade.Enabled = true;
                btnBaglantiKes.Enabled = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox21.Text);
                ushort startAdress = Convert.ToUInt16(textBox20.Text);
                ushort numberOfPoint = Convert.ToUInt16(textBox19.Text);
                ushort[] arr=master.ReadHoldingRegisters(slaveAdress, startAdress, numberOfPoint);
                for (int i = 0; i < arr.Length; i++)
                {
                    textBox18.Text += arr[i] + " ,";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAdress = Convert.ToByte(textBox25.Text);
                ushort startAdress = Convert.ToUInt16(textBox24.Text);
                ushort numberOfPoint = Convert.ToUInt16(textBox23.Text);
               bool[] arr = master.ReadInputs(slaveAdress,startAdress,numberOfPoint);
                for (int i = 0; i < arr.Length; i++)
                {
                    textBox22.Text += arr[i] + " ,";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
