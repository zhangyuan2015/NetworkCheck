using NetworkCheck.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkCheck
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private const string IPLIST_FILE_PATH = "./Config/IPList.json";
        private const string LISTVIEWITEM_TEXT_NAME = "Index";
        private bool Executing = false;
        private List<IPCheckModel> modelList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private ColumnHeaderInfoAttribute GetColumnHeaderInfoAttribute(PropertyInfo propertyInfo)
        {
            object[] attrs = propertyInfo.GetCustomAttributes(typeof(ColumnHeaderInfoAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                return ((ColumnHeaderInfoAttribute)attrs[0]);
            }
            return null;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            listView.View = View.Details;

            PropertyInfo[] propertyInfos = typeof(IPCheckModel).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var attr = GetColumnHeaderInfoAttribute(propertyInfo);
                if (attr != null && attr.IsShow)
                {
                    listView.Columns.Add(new ColumnHeader() { Text = attr.Text, Width = attr.Width, TextAlign = attr.TextAlign });
                }
            }

            modelList = LoadConfigFile();
            labMsg.Text = $"成功加载配置文件 总条数:{modelList.Count}";

            int index = 1;
            foreach (var model in modelList)
            {
                model.Index = index++;
                ListViewItem lvi = new ListViewItem();

                PropertyInfo[] modelInfos = model.GetType().GetProperties();
                foreach (var propertyInfo in modelInfos)
                {
                    var attr = GetColumnHeaderInfoAttribute(propertyInfo);
                    if (attr != null && attr.IsShow)
                    {
                        string val = propertyInfo.GetValue(model, null)?.ToString() ?? "";
                        if (string.Equals(propertyInfo.Name, LISTVIEWITEM_TEXT_NAME))
                            lvi.Text = val;
                        else
                            lvi.SubItems.Add(val);
                    }
                }
                listView.Items.Add(lvi);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<IPCheckModel> LoadConfigFile()
        {
            if (!File.Exists(IPLIST_FILE_PATH))
            {
                return GetTestModelList();
            }
            else
            {
                var ipListJson = File.ReadAllText(IPLIST_FILE_PATH);
                if (string.IsNullOrWhiteSpace(ipListJson))
                    return GetTestModelList();
                else
                {
                    try
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<List<IPCheckModel>>(ipListJson);
                    }
                    catch (Exception ex)
                    {
                        var msgBoxRes = MessageBox.Show($"配置文件解析失败：{Environment.NewLine}{ex.Message}{Environment.NewLine}是否初始化配置文件？", "异常", MessageBoxButtons.OKCancel);
                        if (msgBoxRes == DialogResult.OK)
                        {
                            File.Move(IPLIST_FILE_PATH, $"{ IPLIST_FILE_PATH}.{DateTime.Now.Ticks}.bak");
                            return GetTestModelList();
                        }
                    }
                }
            }
            return new List<IPCheckModel>();
        }

        private static List<IPCheckModel> GetTestModelList()
        {
            var testModelList = new List<IPCheckModel>
            {
                new IPCheckModel { Name = "百度-测试", IP = "www.baidu.com", IsCkPing = true, IsCkTelnet = true, Port = 80, Timeout = 1000 }
            };
            var ipListJson = Newtonsoft.Json.JsonConvert.SerializeObject(testModelList, Newtonsoft.Json.Formatting.Indented);

            var fileInfo = new FileInfo(IPLIST_FILE_PATH);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            File.WriteAllText(IPLIST_FILE_PATH, ipListJson);
            return testModelList;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (Executing)
            {
                return;
            }
            labMsg.Text = $"总条数:{modelList.Count} 检测中";
            btnCheck.Enabled = false;

            Executing = true;
            List<Task> tasks = new List<Task>();
            foreach (ListViewItem item in listView.Items)
            {
                tasks.Add(Task.Run(() =>
                {
                    var model = modelList.FirstOrDefault(a => a.Index.ToString() == item.Text);
                    if (model == null)
                    {
                        item.SubItems[5].Text = "配置不存在";
                    }
                    else
                    {
                        if (model.IsCkPing)
                        {
                            item.SubItems[6].Text = "...";

                            var res = CheckPing(model.IP, model.Timeout);

                            item.SubItems[6].Text = Utils.GetBoolFlag(res.Success);
                            item.SubItems[7].Text = "";
                        }
                        if (model.IsCkTelnet)
                        {
                            item.SubItems[9].Text = "...";

                            var res = CheckConnect(model.IP, model.Port, model.Timeout);

                            item.SubItems[9].Text = Utils.GetBoolFlag(res.Success);
                            item.SubItems[10].Text = res.Message;
                        }
                    }
                }));
            }
            Task.WhenAll(tasks).ContinueWith(a =>
            {
                Executing = false;
                labMsg.Text = $"总条数:{modelList.Count} 检测完成";
                btnCheck.Enabled = true;
            });
        }

        private Result CheckPing(string ipString, int timeout = 1000)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    var taskRes = ping.Send(ipString, timeout);
                    return new Result { Success = taskRes.Status == IPStatus.Success };
                }
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message };
            }
        }

        /// <summary>
        /// 检查服务器和端口是否可以连接
        /// </summary>
        /// <param name="ipString"></param>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private Result CheckConnect(string ipString, int port, int timeout = 1000)
        {
            try
            {
                using (System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient() { SendTimeout = timeout })
                {
                    tcpClient.Connect(ipString, port);
                    bool connected = tcpClient.Connected;
                    return new Result { Success = connected };
                }
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message };
            }
        }
    }
}