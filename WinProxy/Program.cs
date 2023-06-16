using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinProxy
{
    class Program
    {
        static List<ParamInfo> ParamInfoList;
        static bool Is_Debug = false;
        static StringBuilder OutPutMsg = new StringBuilder(); 

        static void Main(string[] args)
        {
             //args = new string[] { "fun=CopyFiles&paths=\"D:\\zixuan_file\\use\\other\\file1.xls\",\"D:\\zixuan_file\\use\\other\\file2.xls\"&tpath=Z:\\YZX&debug=1" };

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
                var paths = SplitPaths(GetParamValue("paths"));
                var tarPath = GetParamValue("tpath");
                var updateTime = GetParamValue("uptime", "N");

                if(updateTime.ToUpper() == "Y")
                {
                    if (paths != null && paths.Count() > 0)
                    {
                        foreach(var path in paths)
                        {
                            var file = new System.IO.FileInfo(path);
                            file.CreationTime = DateTime.Now;
                            file.LastWriteTime = DateTime.Now;
                            file.LastAccessTime = DateTime.Now;
                        }
                    }
                }

                string msg = string.Empty;
                if (fun == "CopyFiles")
                    FileOperateProxy.CopyFiles(paths, tarPath, true, true, false, ref msg);
                else if (fun == "MoveFiles")
                    FileOperateProxy.MoveFiles(paths, tarPath, true, true, false, ref msg);
            }

            if (Is_Debug && OutPutMsg.Length > 0)
                MessageBox.Show(OutPutMsg.ToString(), 
                    Application.ProductName + " debug info", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                
        }

        static string[] SplitPaths(string paths)
        {
            if (string.IsNullOrEmpty(paths))
                return null;

            List<string> resultPaths = new List<string>();

            var split = GetParamValue("split", ",");
            var inSplit = "|";

            paths = paths.Trim('"').Replace("\"" + split + "\"", inSplit);

            if (paths.IndexOf(inSplit) != -1)
            {
                resultPaths.AddRange(paths
                    .Split(new string[] { inSplit }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray());
            }
            else
            {
                resultPaths.Add(paths);
            }

            return resultPaths.ToArray();
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
                OutPutMsg.AppendLine($"key:{key},value:{result}");

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
