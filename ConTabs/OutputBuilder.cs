using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConTabs
{
    public partial class Table<T>
    {
        private sealed class OutputBuilder<T2> where T2 : class
        {
            private readonly StringBuilder sb;
            private readonly Table<T2> table;
            private readonly Style style;

            internal static string BuildOutput(Table<T2> t, Style s)
            {
                var instance = new OutputBuilder<T2>(t, s);
                return instance.sb.ToString();
            }

            private OutputBuilder(Table<T2> t, Style s)
            {
                table = t;
                style = s;
                sb = new StringBuilder();
                HLine(TopMidBot.Top); NewLine();
                Headers(); NewLine();
                HLine(TopMidBot.Mid); NewLine();
                if (table.Data == null || table.Data.Count() == 0)
                {
                    NoDataLine(); NewLine();
                }
                else
                {
                    for (int i = 0; i < table.Data.Count(); i++)
                    {
                        DataRow(i);
                    }
                }
                HLine(TopMidBot.Bot);
            }

            private void NewLine()
            {
                sb.Append(Environment.NewLine);
            }

            private void HLine(TopMidBot v)
            {
                sb.Append(GetCorner(v, LeftCentreRight.Left));
                for (int i = 0; i < table._colsShown.Count; i++)
                {
                    sb.Append(new string(style.Floor, table._colsShown[i].MaxWidth + (2 * table.Padding)));
                    if (i < table._colsShown.Count - 1) sb.Append(GetCorner(v, LeftCentreRight.Centre));
                }
                sb.Append(GetCorner(v, LeftCentreRight.Right));
            }

            private void NoDataLine()
            {
                var noDataText = "no data";
                int colWidths = table._colsShown.Sum(c => c.MaxWidth);
                int innerWidth = colWidths + (2 * table._colsShown.Count * table.Padding) + (table._colsShown.Count + 1) - 2;
                int leftPad = (innerWidth - noDataText.Length) / 2;
                int rightPad = innerWidth - (leftPad + noDataText.Length);
                sb.Append(style.Wall + new String(' ', leftPad) + noDataText + new string(' ', rightPad) + style.Wall);
            }

            private void Headers()
            {
                sb.Append(style.Wall);
                foreach (var col in table._colsShown)
                {
                    sb.Append(GetPaddingString(table.Padding) + table.HeaderAlignment.ProcessString(col.ColumnName, col.MaxWidth) + GetPaddingString(table.Padding) + style.Wall);
                }
            }

            private void DataRow(int i)
            {
                var cols = table._colsShown.Select(c => new CellParts(c.StringValForCol(c.Values[i]), c.MaxWidth, c.Alignment)).ToList();

                var maxLines = cols.Max(c => c.LineCount);

                for (int j = 0; j < maxLines; j++)
                {
                    DataLine(cols, j); NewLine();
                }
            }

            private void DataLine(List<CellParts> parts, int line)
            {
                sb.Append(style.Wall);
                foreach (var part in parts)
                {
                    string val = part.GetLine(line);
                    sb.Append(GetPaddingString(table.Padding) + part.Alignment.ProcessString(val, part.ColMaxWidth) + GetPaddingString(table.Padding) + style.Wall);
                }
            }

            private enum TopMidBot
            {
                Top,
                Mid,
                Bot
            }

            private enum LeftCentreRight
            {
                Left,
                Centre,
                Right
            }

            private sealed class CellParts
            {
                internal CellParts(string value, int width, Alignment alignment)
                {
                    _value = value;
                    ColMaxWidth = width;
                    Alignment = alignment;
                }

                internal int ColMaxWidth { get; private set; }
                internal Alignment Alignment { get; private set; }
                internal int LineCount => _lines.Length;

                internal string GetLine(int i)
                {
                    if (_lines.Length > i) return _lines[i];
                    return String.Empty;
                }

                private string _value { get; set; }
                private string[] _lines => _value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }

            private char GetCorner(TopMidBot v, LeftCentreRight h)
            {
                return style.Corners[(int)h, (int)v];
            }

            private string GetPaddingString(int length)
            {
                return new String(' ', length);
            }
        }
    }
}
