using System.Collections.Generic;

namespace ExcelReaderStandardLibrary
{
    public interface ITableReader
    {
        List<Dictionary<string, string>> Read(string filename, int worksheetIndex, int headerRowIndex);
    }
}