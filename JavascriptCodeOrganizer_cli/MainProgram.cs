using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;



namespace JavascriptCodeOrganizer_cli
{
    public static class MainProgram
    {
        public static void Main(string[] args)
        {
            m_ifp = @"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1_.js";
            m_ofp = @"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1_-min.js";

            try
            {
                m_wt = Stopwatch.StartNew();

                m_srd = new StreamReader(m_ifp);
                m_swt = new StreamWriter(m_ofp);

                while (!m_srd.EndOfStream)
                {
                    try
                    {
                        string ls = m_srd.ReadLine();
                        string ws = ls.Trim();
                        if (string.IsNullOrWhiteSpace(ws)) continue;

                        ws = _ClearCommentsOneLine(ws);
                        if (string.IsNullOrWhiteSpace(ws)) continue;

                        ws = _ClearCommentsMultiLineAll(ws);
                        if (string.IsNullOrWhiteSpace(ws)) continue;

                        if (m_bcm == false)
                            ws = _ClearCommentsMultiLineBegin(ws);
                        else
                            ws = _ClearCommentsMultiLineEnd(ws);

                        if (string.IsNullOrWhiteSpace(ws) == false)
                        {
                            m_swt.WriteLine(ws);
                            //_Log(ws);
                        }
                    }
                    catch (Exception ex)
                    {
                        _Log(ex.ToString());
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _Log(ex.ToString());
            }
            finally
            {
                try { m_srd.Dispose(); } catch { }
                try { m_swt.Dispose(); } catch { }
            }

            m_wt.Stop();
            _Log(m_wt.Elapsed.ToString());
        }


        private static void _Log(string msg)
        {
            Console.WriteLine(msg);
        }



        private static string m_ifp;
        private static string m_ofp;

        private static StreamReader m_srd;
        private static StreamWriter m_swt;

        private static bool m_bcm = false;

        private static Stopwatch m_wt;



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
                m_bcm = true;
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
                m_bcm = false;
                string rv = ws.Replace(mt.Value, string.Empty);
                return rv;
            }
            else
                return string.Empty;
        }

    }

}
