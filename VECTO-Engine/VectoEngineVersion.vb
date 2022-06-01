

Public Module VectoEngineVersion

#If CERTIFICATION_RELEASE
    Private Const Suffix As String = ""
#Else
#If RELEASE_CANDIDATE Then
    Private Const Suffix As String = "-RC"
#Else
    Private Const Suffix as String = "-DEV"
#End If
#End If

	Public ReadOnly Property VersionNumber as String
        get
            return "1.5.0.2707" + Suffix
        end get
    end Property

    Public ReadOnly Property FullVersion as String
        get
            return String.Format("VECTO-Engine {0}", VersionNumber)
        end get
    end Property
End Module
