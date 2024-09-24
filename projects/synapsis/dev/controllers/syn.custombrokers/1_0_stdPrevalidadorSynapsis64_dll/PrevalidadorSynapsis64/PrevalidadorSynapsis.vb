Imports Syn.Documento
Imports Wma.Exceptions

Public Class PrevalidadorSynapsis : Inherits Prevalidador
    Implements IPrevalidadorSynapsis, IPrevalidador

#Region "Atributos"

    Private _pedimentoSynapsis As DocumentoElectronico

    Private _informePrevalidacion As InformePrevalidacion

    Private _estatusSynapsis As TagWatcher

#End Region

#Region "Propiedades"

    Public Property PedimentoSynapsis As Syn.Documento.DocumentoElectronico Implements IPrevalidadorSynapsis.PedimentoSynapsis

        Get

            Return _pedimentoSynapsis

        End Get

        Set(value As Syn.Documento.DocumentoElectronico)

            _pedimentoSynapsis = value

        End Set

    End Property

    Public Property InformePrevalidacion As Controllers.InformePrevalidacion Implements IPrevalidadorSynapsis.InformePrevalidacion

        Get

            Return _informePrevalidacion

        End Get

        Set(value As Controllers.InformePrevalidacion)

            _informePrevalidacion = value

        End Set

    End Property

    Public Property EstatusSynapsis As TagWatcher Implements IPrevalidadorSynapsis.EstatusSynapsis

        Get

            Return _estatusSynapsis

        End Get

        Set(value As TagWatcher)

            _estatusSynapsis = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function CargaDocumentoElectronico(pedimentoSynpasis_ As DocumentoElectronico) As TagWatcher Implements IPrevalidadorSynapsis.CargaDocumentoElectronico

        _estatusSynapsis = New TagWatcher()

        If pedimentoSynpasis_ IsNot Nothing Then

            _pedimentoSynapsis = pedimentoSynpasis_

            _estatusSynapsis.Status = TagWatcher.TypeStatus.Ok

        Else

            _estatusSynapsis.SetError(Me, "Ocurrio un detalle al procesar el documento electrónico, verifique la información.")

        End If

        Return _estatusSynapsis

    End Function

    Public Function GenerarInformePrevalidacion(pedimento_ As DocumentoElectronico) As TagWatcher Implements IPrevalidadorSynapsis.GenerarInformePrevalidacion

        Throw New NotImplementedException()

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

    Public Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

    Public Overloads Sub ObtenerRutaValidacion(ByRef pedimentoSynpasis_ As DocumentoElectronico)

    End Sub
#End Region

End Class