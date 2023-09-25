Imports System.Collections.Generic
Imports System.Data
Imports gsol.krom.controladores

Namespace gsol.krom

    Public Interface IControladorPeticiones

#Region "Metodos"

        Function RealizarPeticion(ByVal peticion_ As Peticiones, controladorWeb_ As ControladorWeb) As DataTable

        'Function RealizarPeticion(ByVal peticion_ As List(Of Peticiones)) As DataTable

#End Region

    End Interface

End Namespace
