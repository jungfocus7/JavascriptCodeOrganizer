Imports System
Imports System.IO
Imports System.Xml



Public NotInheritable Class ConfigData
#Region "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 1) 싱글턴 정적 영역"
    ''' <summary>
    ''' Xml Data Template
    ''' </summary>
    Private Shared ReadOnly m_xdt As String = "
<Root>
</Root>
    ".Trim()


    ''' <summary>
    ''' Config Data File Path
    ''' </summary>
    Private Shared m_cdfp As String
    Public Shared ReadOnly Property ConfigDataFilePath As String
        Get
            If String.IsNullOrWhiteSpace(m_cdfp) Then
                Dim cdp As String = Environment.GetCommandLineArgs()(0)
                cdp = Path.GetDirectoryName(cdp)
                m_cdfp = Path.Combine(cdp, "ConfigData.xml")
            End If
            Return m_cdfp
        End Get
    End Property


    ''' <summary>
    ''' Xml Document
    ''' </summary>
    Private Shared m_xdoc As XmlDocument


    ''' <summary>
    ''' 데이터 로드
    ''' </summary>
    Private Shared Sub _LoadConfigData()
        If m_xdoc Is Nothing Then
            If Not File.Exists(ConfigDataFilePath) Then
                File.WriteAllText(ConfigDataFilePath, m_xdt)
            End If
            m_xdoc = New XmlDocument()
            m_xdoc.Load(ConfigDataFilePath)
        End If
    End Sub


    ''' <summary>
    ''' 싱글턴 인스턴스 참조
    ''' </summary>
    Private Shared m_ccd As ConfigData


    ''' <summary>
    ''' 싱글턴 인스턴스 생성
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function Create() As ConfigData
        If m_ccd Is Nothing Then
            m_ccd = New ConfigData()
        End If
        Return m_ccd
    End Function
#End Region



    ''' <summary>
    ''' 생성자
    ''' </summary>
    Private Sub New()
        _LoadConfigData()
    End Sub

End Class
