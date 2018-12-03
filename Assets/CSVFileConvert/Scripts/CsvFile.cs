using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

/// <summary>
/// Csv文件处理类，包括读取和保存csv格式文件
/// </summary>
public class CsvFile
{
    public const string DEFAULT_EXTENSION = ".csv";
    public static Encoding DEFAULT_ENCODING = Encoding.GetEncoding("GB2312");//.GetEncoding("GB2312");//因CSV文件默认是该格式，但GB2312在移动端不支持
    public delegate void LoadCallback(CsvFile sender, bool success);

    #region 静态

    public static CsvFile Create(string fileName)
    {
        if (fileName == null)
            throw new ArgumentNullException("fileName");

        FileStream fs = File.Create(fileName);
        var r = new CsvFile();

        r._name = fileName;
        r._baseStream = fs;

        return r;
    }

    #endregion

    #region 子类

    public class Entry
    {
        public List<string> rowValues;
    }

    #endregion

    bool _disposed = false;
    List<Entry> _list = new List<Entry>();

    /// <summary>
    /// 当从文件读取时，该值才会有值，为文件名
    /// </summary>
    string _name;
    Stream _baseStream;
    Encoding _encoding;

    public CsvFile(FileStream stream, int skipRowCount = 0) : this(stream, DEFAULT_ENCODING, skipRowCount) { }

    public CsvFile(FileStream stream, Encoding encoding, int skipRowCount = 0)
    {
        _baseStream = stream;
        _name = stream.Name;
        _encoding = encoding;
        Read(skipRowCount);
    }

    public CsvFile(Stream stream, int skipRowCount = 0) : this(stream, DEFAULT_ENCODING, skipRowCount) { }

    public CsvFile(Stream stream, Encoding encoding, int skipRowCount = 0)
    {
        _baseStream = stream;
        _encoding = encoding;
        Read(skipRowCount);
    }

    public CsvFile(string path, int skipRowCount = 0) : this(path, DEFAULT_ENCODING, skipRowCount) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="encoding"></param>
    /// <param name="skipRowCount"></param>
    /// <exception cref="ArgumentException">path不能是url形式</exception>
    public CsvFile(string fileName, Encoding encoding, int skipRowCount = 0)
    {
        if (IsUrl(fileName))
            throw new ArgumentException("不能是url形式", "path");

        _baseStream = File.OpenRead(fileName);
        _name = fileName;
        _encoding = encoding;
        Read(skipRowCount);
    }

    bool IsUrl(string path)
    {
        if (path == null)
            throw new ArgumentNullException("path");

        if (path.Contains("://"))
            return true;

        return false;
    }

    private CsvFile()
    {
        _encoding = DEFAULT_ENCODING;
    }

    public string Name { get { return _name; } }
    public List<Entry> Lines { get { return _list; } }
    public int Count { get { return _list.Count; } }

    public void Add(Entry row)
    {
        _list.Add(row);
    }

    public void Add(List<string> row)
    {
        _list.Add(new Entry() { rowValues = row });
    }

    public void Add(List<List<string>> rows)
    {
        foreach (List<string> row in rows)
        {
            Add(row);
        }
    }

    public void Save()
    {
        //using (Stream stream = File.OpenWrite(_name))
        //{
        Stream stream = _baseStream;
        if (!stream.CanWrite)
        {
            stream = File.OpenWrite(_name);
        }
        using (StreamWriter sw = new StreamWriter(stream, _encoding))
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                List<string> row = _list[i].rowValues;
                for (int j = 0; j < row.Count; j++)
                {
                    string data = row[j];
                    // 替换英文冒号, 英文冒号需要换成两个冒号
                    data = data.Replace("\"", "\"\"");
                    if (data.Contains(',') || data.Contains('"') || data.Contains('\r') || data.Contains('\n'))
                    {
                        // 含逗号 冒号 换行符的需要放到引号中
                        data = string.Format("\"{0}\"", data);
                    }
                    sb.Append(data);
                    if (j < row.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine();
            }
            sw.Write(sb.ToString());
            sw.Flush();
        }
        //}
    }

    public void Dispose()
    {
        Dispose(true);
    }

    void Read(int skipRowCount = 0)
    {
        // 对象使用完后就回收
        using (StreamReader sr = new StreamReader(_baseStream, _encoding))
        {
            for (int i = 0; i < skipRowCount; i++)
            {
                //跳过这几行
                sr.ReadLine();
            }
            List<string> row = null;

            while (true)
            {
                row = ReadRow(sr);

                if (row == null)
                    break;

                _list.Add(new Entry() { rowValues = row });

            }
        }

    }

    List<string> ReadRow(StreamReader stream)
    {
        List<string> result = new List<string>();
        string line = "";
        bool insideQuotes = false;
        int wordStart = 0;
        string s;

        while ((s = stream.ReadLine()) != null)
        {
            if (insideQuotes)
            {
                s = s.Replace("\\n", "\n");
                line += "\n" + s;
            }
            else
            {
                line = s;
                line = line.Replace("\\n", "\n");
                wordStart = 0;
            }

            for (int i = wordStart, imax = line.Length; i < imax; ++i)
            {
                char ch = line[i];

                if (ch == ',')
                {
                    if (!insideQuotes)
                    {
                        result.Add(line.Substring(wordStart, i - wordStart));
                        wordStart = i + 1;
                        if (wordStart >= imax)
                        {
                            // 如果最后一列的内容为null，该行是由,结尾的，需要判定下
                            result.Add(string.Empty);
                            return result;
                        }
                    }
                }
                else if (ch == '"')
                {
                    if (insideQuotes)
                    {
                        if (i + 1 >= imax)
                        {
                            result.Add(line.Substring(wordStart, i - wordStart).Replace("\"\"", "\""));
                            return result;
                        }

                        if (line[i + 1] != '"')
                        {
                            result.Add(line.Substring(wordStart, i - wordStart).Replace("\"\"", ""));
                            insideQuotes = false;

                            if (line[i + 1] == ',')
                            {
                                ++i;
                                wordStart = i + 1;
                            }
                        }
                        else
                        {
                            ++i;
                        }
                    }
                    else
                    {
                        wordStart = i + 1;
                        insideQuotes = true;
                    }
                }
            }

            if (wordStart < line.Length)
            {
                if (insideQuotes) continue;
                result.Add(line.Substring(wordStart, line.Length - wordStart));
            }
            return result;
        }
        return null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            //托管资源的回收清理
            _list = null;

            if (_baseStream != null)
            {
                _baseStream.Close();
                _baseStream.Dispose();
                _baseStream = null;
            }
        }
        //非托管资源的回收清理
        _disposed = true;
    }

}
