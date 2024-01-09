Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Text.RegularExpressions




Public Module EntryPoint
    Private m_config As ConfigData = ConfigData.Create()


    ''' <summary>
    ''' 메인 엔트리 포인트
    ''' </summary>
    ''' <param name="args"></param>
    Public Sub Main(args As String())
        'args = {"D:\042#DOTNET#Work\WV2UICOM\HtmlRoot\hjs\hfscrw#bu1_.js"}
        If (Not args Is Nothing) AndAlso (args.Length > 0) Then
            _WorkStart(args(0))
        Else
            _Log("입력 파라미터가 잘못되었습니다.")
        End If
    End Sub


    ''' <summary>
    ''' 로그 출력
    ''' </summary>
    ''' <param name="msg"></param>
    Private Sub _Log(msg As String)
        Console.WriteLine(msg)
    End Sub




    ''' <summary>
    ''' InputFilePath
    ''' </summary>
    Private m_ifp As String

    ''' <summary>
    ''' OutputFilePath
    ''' </summary>
    Private m_ofp As String

    ''' <summary>
    ''' StreamReader
    ''' </summary>
    Private m_srd As StreamReader

    ''' <summary>
    ''' StreamWriter
    ''' </summary>
    Private m_swt As StreamWriter

    ''' <summary>
    ''' m_bcm
    ''' </summary>
    Private m_bcm As Boolean = False

    ''' <summary>
    ''' OutputFilePath
    ''' </summary>
    Private m_wt As Stopwatch


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ifp"></param>
    Private Sub _WorkStart(ifp As String)
        m_ifp = ifp
        m_ofp = FileSystemHelper.GetOutputFilePath(ifp)
        '_Log(String.Format("m_ifp: {0}, m_ofp: {1}", m_ifp, m_ofp))

        m_wt = Stopwatch.StartNew()

        Try
            m_srd = New StreamReader(m_ifp)
            m_swt = New StreamWriter(m_ofp)

            While Not m_srd.EndOfStream
                Try
                    Dim ls As String = m_srd.ReadLine()
                    Dim ws As String = ls.Trim()
                    If (String.IsNullOrWhiteSpace(ws)) Then
                        Continue While
                    End If

                    ws = _ClearCommentsOneLine(ws)
                    If (String.IsNullOrWhiteSpace(ws)) Then
                        Continue While
                    End If

                    ws = _ClearCommentsMultiLineAll(ws)
                    If (String.IsNullOrWhiteSpace(ws)) Then
                        Continue While
                    End If

                    If Not m_bcm Then
                        ws = _ClearCommentsMultiLineBegin(ws)
                    Else
                        ws = _ClearCommentsMultiLineEnd(ws)
                    End If

                    If Not String.IsNullOrWhiteSpace(ws) Then
                        m_swt.WriteLine(ws)
                        '_Log(ws)
                    End If
                Catch ex As Exception
                    _Log(ex.ToString())
                    Throw ex
                End Try
            End While
        Catch ex As Exception
            _Log(ex.ToString())
        Finally
            m_srd.Clear()
            m_swt.Clear()
        End Try

        m_wt.Stop()
        _Log(m_wt.Elapsed.ToString())
    End Sub


    ''' <summary>
    ''' 한줄 주석 제거
    ''' </summary>
    ''' <param name="ws"></param>
    ''' <returns></returns>
    Private Function _ClearCommentsOneLine(ws As String) As String
        Const rex As String = "^[ \t]*\/\/[^\r\n]*$"
        If Regex.IsMatch(ws, rex) Then
            Return String.Empty
        Else
            Return ws
        End If
    End Function


    ''' <summary>
    ''' 두줄 주석 제거 All
    ''' </summary>
    ''' <param name="ws"></param>
    ''' <returns></returns>
    Private Function _ClearCommentsMultiLineAll(ws As String) As String
        Const rex As String = "\/\*[\S\s]*?\*\/"
        Dim rv As String = Regex.Replace(ws, rex, String.Empty)
        Return rv
    End Function


    ''' <summary>
    ''' 두줄 주석 제거 Begin
    ''' </summary>
    ''' <param name="ws"></param>
    ''' <returns></returns>
    Private Function _ClearCommentsMultiLineBegin(ws As String) As String
        Const rex As String = "\/\*.*?$"
        Dim mt As Match = Regex.Match(ws, rex)
        If mt.Success Then
            m_bcm = True
            Dim rv As String = ws.Replace(mt.Value, String.Empty)
            Return rv
        Else
            Return ws
        End If
    End Function


    ''' <summary>
    ''' 두줄 주석 제거 End
    ''' </summary>
    ''' <param name="ws"></param>
    ''' <returns></returns>
    Private Function _ClearCommentsMultiLineEnd(ws As String) As String
        Const rex As String = "^.*?\*\/"
        Dim mt As Match = Regex.Match(ws, rex)
        If mt.Success Then
            m_bcm = False
            Dim rv As String = ws.Replace(mt.Value, String.Empty)
            Return rv
        Else
            Return String.Empty
        End If
    End Function

End Module
