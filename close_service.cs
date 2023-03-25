using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using TaskScheduler;

namespace 关闭服务
{
    public partial class close_service : Form
    {
        public close_service()
        {
            InitializeComponent();
        }

        public void CloseService()
        {
            //从注册表中找到wuauserv服务注册地址
            string keyPath = @"SYSTEM\CurrentControlSet\Services\wuauserv";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true);

            int val = -1;
            bool bConverted = Int32.TryParse(key.GetValue("Start").ToString(), out val);
            if (bConverted)
            {
                //启用方式设置为禁用 
                key.SetValue("Start", 4); 
            }

            //var servs = ServiceController.GetServices();
            //找到Windows Update服务
            ServiceController service = new ServiceController("Windows Update");
            if (service.CanStop)
            {
                // 如果权限不够是不能Stop()的。
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
                
            }
            //else// 不能停止，反过来就是可以开启
            //{
            //    service.Start();
            //    service.WaitForStatus(ServiceControllerStatus.Running);
            //}
            
            // 释放对该服务的控制权及释放相应资源。
            service.Close();


            //从注册表中找到WaaSMedicSvc服务注册地址
            keyPath = @"SYSTEM\CurrentControlSet\Services\WaaSMedicSvc";
            
            key = Registry.LocalMachine.OpenSubKey(keyPath, true);

            val = -1;
            bConverted = Int32.TryParse(key.GetValue("Start").ToString(), out val);
            if (bConverted)
            {
                //启用方式设置为禁用
                key.SetValue("Start", 4);
            }

            //找到WaaSMedicSvc服务
            ServiceController service1 = new ServiceController("WaaSMedicSvc");
            if (service1.CanStop)
            {
                // 如果权限不够是不能Stop()的。
                service1.Stop();
                service1.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            // 释放对该服务的控制权及释放相应资源。
            service1.Close();

            //从注册表中找到UsoSvc服务注册地址
            keyPath = @"SYSTEM\CurrentControlSet\Services\UsoSvc";

            key = Registry.LocalMachine.OpenSubKey(keyPath, true);

            val = -1;
            bConverted = Int32.TryParse(key.GetValue("Start").ToString(), out val);
            if (bConverted)
            {
                //启用方式设置为禁用
                key.SetValue("Start", 4);
            }

            //找到UsoSvc服务
            ServiceController service2 = new ServiceController("UsoSvc");
            if (service2.CanStop)
            {
                // 如果权限不够是不能Stop()的。
                service2.Stop();
                service2.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            // 释放对该服务的控制权及释放相应资源。
            service2.Close();

            MessageBox.Show("服务关闭成功！", "成功", MessageBoxButtons.OK);
  

        //taskschd.msc关闭WindowsUpdate的子条目
        //从任务计划程序中找到子条目的地址
        
            //1.连接TaskSchedulerClass 
            TaskSchedulerClass scheduler = new TaskSchedulerClass();
            scheduler.Connect(
                "",//电脑名
                "",//用户名
                "",//域名
                ""//密码
                ) ;

            //2.获取计划任务文件夹
            ITaskFolder folder = scheduler.GetFolder("//Microsoft\\Windows\\WindowsUpdate");

            //3.获取计划任务名称
            IRegisteredTask task = folder.GetTask("Scheduled Start");
            //运行 带参数
            IRunningTask runningtask = task.Run(null);
            //停止 参数为预留参数
            //task.Stop(0);
            //禁用
            task.Enabled = false;
            //启用
            //task.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CloseService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
