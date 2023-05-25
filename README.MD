### 參數模板
* fun
  * CopyFiles 文件複製到...
  * MoveFiles 文件移動到...

#### CopyFiles\MoveFiles 參數
* split 當多路徑時採用的分隔符(默認,)
* paths 源文件 d:\test.txt 或 "d:\test1.txt","d:\test2.txt"(分隔符取決於上一項設置)
* tpath 目標路徑

### 其他參數
* debug 啟用調試模式,傳任何值則會開啟，調用時自動輸出參數並且窗口不會自動關閉

### 示例

```
WinProxy.exe fun=CopyFiles&paths=D:\zixuan_file\use\other\file1.xls&tpath=Z:\YZX
```

```
WinProxy.exe fun=CopyFiles&paths="D:\zixuan_file\use\other\file1.xls","D:\zixuan_file\use\other\file2.xls"&tpath=Z:\YZX&debug=1
```