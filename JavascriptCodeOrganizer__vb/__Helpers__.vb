Imports System
Imports System.IO
Imports System.Runtime.CompilerServices




Public NotInheritable Class FileSystemHelper
    Private Sub New()
    End Sub

    Public Shared Function GetOutputFilePath(ifp As String) As String
        If String.IsNullOrWhiteSpace(ifp) OrElse Not File.Exists(ifp) Then
            Return String.Empty
        Else
            Dim cdp As String = Path.GetDirectoryName(ifp)
            Dim ffnm As String = Path.GetFileNameWithoutExtension(ifp)
            Dim ofp As String = Path.Combine(cdp, ffnm & "-mx.js")
            Return ofp
        End If
    End Function

End Class



Public Module ClearObject
    <Extension()>
    Public Sub Clear(obj As IDisposable)
        If Not obj Is Nothing Then
            obj.Dispose()
        End If
    End Sub
End Module
