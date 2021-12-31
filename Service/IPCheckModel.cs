namespace NetworkCheck.Service
{
    public class IPCheckModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("序列", 38)]
        public int Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnHeaderInfo("名称", 80)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnHeaderInfo("IP", 110)]
        public string IP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnHeaderInfo("端口 默认80", 80)]
        public int Port { get { return _port == null ? Utils.DEFAULT_PORT : _port.Value; } set { _port = value; } }
        private int? _port;

        /// <summary>
        /// 
        /// </summary>
        [ColumnHeaderInfo("超时时间", 60)]
        public int Timeout { get { return _timeout == null ? Utils.DEFAULT_TIMEOUT : _timeout.Value; } set { _timeout = value; } }
        private int? _timeout;

        /// <summary>
        /// 
        /// </summary>
        [ColumnHeaderInfo("Ping", 38, false)]
        public bool IsCkPing { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Ping", 50)]
        public string IsCkPingStr { get { return Utils.GetBoolFlag(IsCkPing); } }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Ping 结果", 68, false)]
        public bool? CkPingSuccess { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Ping 结果", 68)]
        public string CkPingSuccessStr { get { return CkPingSuccess.HasValue ? Utils.GetBoolFlag(CkPingSuccess.Value) : ""; } }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Ping 信息", 80)]
        public string CkPingResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnHeaderInfo("Telnet", 50, false)]
        public bool IsCkTelnet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Telnet", 50)]
        public string IsCkTelnetStr { get { return Utils.GetBoolFlag(IsCkTelnet); } }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Telnet 结果", 70, false)]
        public bool? CkTelnetSuccess { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Telnet 结果", 70)]
        public string CkTelnetSuccessStr { get { return CkTelnetSuccess.HasValue ? Utils.GetBoolFlag(CkTelnetSuccess.Value) : ""; } }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [ColumnHeaderInfo("Telnet 信息", 80)]
        public string CkTelnetResult { get; set; }
    }
}