using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinProxy
{
    class Program
    {
        static List<ParamInfo> ParamInfoList;
        static bool Is_Debug = false;

        static void Main(string[] args)
        {
            var paramStr = string.Empty;

            if (args != null && args.Length > 0)
            {
                paramStr = args[0];
                ParamInfoList = InitParamSet(paramStr);
            }

            Is_Debug = string.IsNullOrEmpty(GetParamValue("debug")) ? false : true;

            var fun = GetParamValue("fun");

            if(fun == "CopyFiles" || fun == "MoveFiles")
            {
                var paths = GetParamValue("paths");
                var tarPath = GetParamValue("tpath");

                string msg = string.Empty;
                if (fun == "CopyFiles")
                    FileOperateProxy.CopyFiles(new string[] { paths }, tarPath, true, true, false, ref msg);
                else if (fun == "MoveFiles")
                    FileOperateProxy.MoveFiles(new string[] { paths }, tarPath, true, true, false, ref msg);
            }

            if (Is_Debug)
                Console.ReadKey();
        }

        static string GetParamValue(string key, string dvalue = "")
        {
            var result = dvalue;

            if (ParamInfoList != null && ParamInfoList.Count > 0)
            {
                var p = ParamInfoList.SingleOrDefault(x => x.Key.ToUpper() == key.ToUpper());
                if (p != null)
                    result = p.Value;
            }
            
            if (Is_Debug)
                Console.WriteLine($"key:{key},value:{result}");

            return result;
        }

        static List<ParamInfo> InitParamSet(string paramStr)
        {
            var result = new List<ParamInfo>();
            if (!string.IsNullOrEmpty(paramStr))
            {
                var plist = paramStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                if (plist != null && plist.Length > 0)
                {
                    foreach(var p in plist)
                    {
                        var p2 = p.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        var pinfo = new ParamInfo();
                        pinfo.Key = p2[0];
                        if (p2.Length == 2)
                            pinfo.Value = p2[1];
                        else
                            pinfo.Value = string.Empty;

                        result.Add(pinfo);
                    }
                }
            }

            return result;
        }
    }

    class ParamInfo
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
