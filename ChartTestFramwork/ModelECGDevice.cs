﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartTestFramwork
{
    internal class ModelECGDevice : IModelECGDevice
    {
        private IModelLocalData modelLocalData;
        private IViewECG viewEKG;
        private IControllerECG controllerEKG;

        IViewECG IModelECGDevice.ViewECG { set => viewEKG=value; }
        IModelLocalData IModelECGDevice.ModelLocaldata { set => modelLocalData=value; }
        IControllerECG IModelECGDevice.ControllerECG { set => controllerEKG=value; }

        private SerialPort serialPort1=new SerialPort();
        private double recievedDouble=0;
        private bool live = false;
        private string message;
    

        public ModelECGDevice()
        {
            serialPort1.BaudRate = 9600;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Handshake = Handshake.None;
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (live == true)
            {


                try
                {
                    string message = serialPort1.ReadLine();
                    recievedDouble = double.Parse(message);
                    viewEKG.NewValue = recievedDouble;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            return;
        }

        void IModelECGDevice.setPort(string portName)
        {
            serialPort1.PortName = portName;
            
        }

        List<ECGValue> IModelECGDevice.getData24h()
        {
            throw new NotImplementedException();
        }

        


        void IModelECGDevice.deleteData24h()
        {
            throw new NotImplementedException();
        }

        void IModelECGDevice.startLiveData()
        {
            live = true;
            
            try
            {
                serialPort1.Open();     
            }
            
            catch (Exception ex)
            { 
                    MessageBox.Show(ex.Message);
            }
        }

        void IModelECGDevice.stopLiveData()
        {
            live=false;
        }

        double IModelECGDevice.getValue()
        {
            return recievedDouble;
        }
    }
}
