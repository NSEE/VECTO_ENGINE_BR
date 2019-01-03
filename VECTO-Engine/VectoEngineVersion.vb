

Public Module VectoEngineVersion

#If RELEASE_CANDIDATE Then
    Private Const Suffix As String = "-RC"
#Else
    Private Const Suffix As String = ""
#End If

	Public ReadOnly Property VersionNumber as String
        get
            return "1.4.4.1463" + Suffix
        end get
    end Property

    Public ReadOnly Property FullVersion as String
        get
            return String.Format("VECTO-Engine {0}", VersionNumber)
        end get
    end Property
End Module
