Imports System.Runtime.CompilerServices

Module Extension

    <Extension()>
    Public Function ToTextBoxText(value As Double?, Optional format As String = Nothing) As String
        If value.HasValue Then
            Return If(format <> Nothing, value.Value.ToString(format), value.Value)
        End If
        Return String.Empty
    End Function

    <Extension()>
    Public Function ToTextBoxText(value As Integer?, Optional format As String = Nothing) As String
        If value.HasValue Then
            Return If(format <> Nothing, value.Value.ToString(format), value.Value)
        End If
        Return String.Empty
    End Function

    <Extension()>
    public Function TextToDouble(value as String) As Double?
        If value <> String.Empty
            Dim resultValue As Double
            If Double.TryParse(value, resultValue) Then
                Return resultValue
            end if
        End If
        Return Nothing
    End Function

    <Extension()>
    public Function TextToInteger(value as String) As Integer?
        If value <> String.Empty
            Dim resultValue As Integer
            If Integer.TryParse(value, resultValue) Then
                Return resultValue
            end if
        End If
        Return Nothing
    End Function

End Module
