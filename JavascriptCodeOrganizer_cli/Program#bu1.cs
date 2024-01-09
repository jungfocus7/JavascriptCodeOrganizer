using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;




namespace ConsoleApp14
{
    class Program
    {
        static void Main(string[] args)
        {
            string txt = @"
AAAAAAAAAA
BBBBBBBBBB
CCCCCC        CCCC
            ".Trim();

            //using (StringReader usr = new StringReader(txt))
            //{
            //    char[] buf = new char[5];
            //    int p = usr.Read(buf, 0, buf.Length);
            //}


            ////StreamReader usr = new StreamReader(".\\Data.txt");
            //StreamReader usr = new StreamReader(@"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1_.js");
            //while (!usr.EndOfStream)
            //{
            //    try
            //    {
            //        string ls = usr.ReadLine().Trim();
            //        _WorkLineStr(ls);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }
            //}

            //_WorkEnd();


            //StreamReader usr = new StreamReader(@"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1_.js");


            _ifp = @"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1_.js";
            _ofp = @"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1-min.js";

            using (_usr = new StreamReader(_ifp))
            {
                while (!_usr.EndOfStream)
                {
                    try
                    {
                        string ls = _usr.ReadLine().Trim();
                        _WorkLineStr(ls);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }


        private static string _ifp;
        private static string _ofp;
        private static StreamReader _usr;
        private static StreamWriter _usw;
        private static StringBuilder _osb = new StringBuilder();
        private static bool _bcm = false;



        private static string _ClearCommentsOneLine(string ws)
        {
            const string rex = @"^[ \t]*\/\/[^\r\n]*$";
            if (Regex.IsMatch(ws, rex))
                return string.Empty;
            else
                return ws;
        }


        private static string _ClearCommentsMultiLineAll(string ws)
        {
            const string rex = @"\/\*[\S\s]*?\*\/";
            string rv = Regex.Replace(ws, rex, string.Empty);
            return rv;
        }


        private static string _ClearCommentsMultiLineBegin(string ws)
        {
            const string rex = @"\/\*.*?$";
            Match mt = Regex.Match(ws, rex);
            if (mt.Success)
            {
                _bcm = true;
                string rv = ws.Replace(mt.Value, string.Empty);
                return rv;
            }
            else
                return ws;
        }


        private static string _ClearCommentsMultiLineEnd(string ws)
        {
            const string rex = @"^.*?\*\/";
            Match mt = Regex.Match(ws, rex);
            if (mt.Success)
            {
                _bcm = false;
                string rv = ws.Replace(mt.Value, string.Empty);
                return rv;
            }
            else
                return string.Empty;
        }



        private static void _WorkLineStr(string ls)
        {
            string ws = ls.Trim();
            if (string.IsNullOrWhiteSpace(ws) == false)
            {
                ws = _ClearCommentsOneLine(ws);
                if (string.IsNullOrWhiteSpace(ws)) return;

                ws = _ClearCommentsMultiLineAll(ws);
                if (string.IsNullOrWhiteSpace(ws)) return;

                if (_bcm == false)
                    ws = _ClearCommentsMultiLineBegin(ws);
                else
                    ws = _ClearCommentsMultiLineEnd(ws);

                if (string.IsNullOrWhiteSpace(ws) == false)
                {
                    //_osb.AppendLine(ws);
                    Console.WriteLine(ws);
                }
            }
        }


        private static void _WorkEnd()
        {
            if (_osb.Length > 0)
            {
                Console.WriteLine(_osb.ToString());
            }
        }

    }
}
