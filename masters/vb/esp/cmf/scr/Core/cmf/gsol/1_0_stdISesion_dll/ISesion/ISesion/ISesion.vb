Imports Wma.WebServices
Imports Wma.Exceptions

Namespace gsol

    Public Interface ISesion

#Region "Atributos"
        Enum Idiomas
            Espaniol = 1
            Ingles = 2
        End Enum

        Enum Notificaciones
            SinNotificacion = 0
            CredencialesIncorrectas = 1
            FalloConexion = 2
            SinEspacioAsignado = 3
        End Enum
#End Region

#Region "Propiedades"

        Property IdentificadorUsuario As String
        Property ContraseniaUsuario As String
        Property GrupoEmpresarial As Integer
        Property DivisionEmpresarial As Integer
        Property Aplicacion As Integer
        Property Idioma As Idiomas
        Property MininimoCaracteresUsuario As Integer
        Property MinimoNumerosUsuario As Integer
        Property MinimoCaracteresContrasena As Integer
        Property MinimoMayusculasContrasena As Integer
        Property MinimoMinusculasContrasena As Integer
        Property MinimoNumerosContrasena As Integer
        ReadOnly Property StatusArgumentos As Boolean
        ReadOnly Property Notificacion As Notificaciones
        ReadOnly Property NombreAutenticacion As String
        ReadOnly Property GruposEmpresariales As DataSet
        ReadOnly Property EspacioTrabajo As IEspacioTrabajo
        ReadOnly Property ClaveUsuario As Int32

        Function GetProfile(ByVal IDRequiredUser As String, _
                 ByVal WebServiceUserID As String, _
                 ByVal WebServicePasswordID As String, _
                 Optional ByVal IdRequiredApplication As Integer = 4, _
                 Optional ByVal CorporateNumber As Integer = 1, _
                 Optional ByVal CompanyId As Integer = 0, _
                 Optional ByVal FullAuthentication As Boolean = False) As UserProfile


        Function GetProfileWorkSpace(ByRef EspacioTrabajo As IEspacioTrabajo, _
                                    ByVal IDRequiredUser As String, _
                                     ByVal WebServiceUserID As String, _
                                     ByVal WebServicePasswordID As String, _
                                     Optional ByVal IdRequiredApplication As Integer = 4, _
                                     Optional ByVal CorporateNumber As Integer = 1, _
                                     Optional ByVal CompanyId As Integer = 0, _
                                     Optional ByVal FullAuthentication As Boolean = False) As UserProfile

        Function SetURLUserPicture(ByVal MobileUserID As String, _
                          ByVal WebServiceUserID As String, _
                          ByVal WebServicePasswordID As String, _
                          ByVal URLPicture_ As String) As TagWatcher



#End Region

#Region "Metodos"

        Sub iniciar()


#End Region

    End Interface

End Namespace