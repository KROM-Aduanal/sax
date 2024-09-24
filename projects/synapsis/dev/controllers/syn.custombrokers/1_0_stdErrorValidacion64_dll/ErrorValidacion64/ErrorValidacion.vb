Imports Wma.Exceptions

Public Class ErrorValidacion : Implements ICloneable

#Region "Enums"

    Enum TiposCriterios

        SinDefinir = 0
        Sintactito = 1
        Catalogico = 2
        Estructural = 3
        Normativo = 4
        DatoInexacto = 5
        UsoCostumbreAA = 6
        UsoCostumbreCliente = 7

    End Enum

    Enum Acciones
        'Este tema no se analizo a profundidad o al menos no tengo nada por ahora.
        SinDefinir = 0
        Desplegar = 1 'Mostrar lo que puedes realizar
        Ejecutar = 2 'Corregir lo que se supone esta mal o que se puede resolver

    End Enum

    Enum TiposMensajes

        SinDefinir = 0
        Advertencia = 1
        ErrorEncontrado = 2 'quise colocar la palabra error y me marca que es una palabra reservada
        Informe = 3
        Incidencia = 4

    End Enum

#End Region

#Region "Propiedades"

    Property tipo As String

    Property numero As Integer

    Property descripcion As String

    Property tipocriterio As TiposCriterios

    Property accion As Acciones

    Property tipomensaje As TiposMensajes

    Property mensaje As String

#End Region

#Region "Metodos"

    Public Sub Add()

    End Sub

    Public Sub Delete()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class