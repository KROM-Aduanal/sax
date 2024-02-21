
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.Recursos

Namespace Syn.Documento.Componentes

    <BsonKnownTypes(GetType(DecoradorCampo))>
    <Serializable()>
    Public MustInherit Class Campo
        Inherits Nodo

#Region "Attributes"

        Private _valor As Object

#End Region

#Region "Enums"
        Public Enum TiposDato
            SinDefinir = 0
            Entero = 1
            Real = 2
            Texto = 3
            Booleano = 4
            Fecha = 5
            IdObject = 6
            Documento = 7
        End Enum

        Public Enum TiposRedondeos
            SinDefinir = 0
            HaciaArriba = 1
            HaciaAbajo = 2
        End Enum

#End Region

#Region "Builders"

        Public Sub New()

            TipoNodo = Nodo.TiposNodo.Campo

            UseAsMetadata = False

        End Sub

#End Region

#Region "Properties"

        <BsonElement("IDUnico")>
        Public Property IDUnico As Integer

        '<BsonElement("NombrePresentacion")>
        '<BsonIgnoreIfDefault>
        '<BsonIgnoreIfNull>
        'Public Property NombrePresentacion As String

        <BsonElement("Valor")>
        <BsonIgnoreIfDefault>
        <BsonIgnoreIfNull>
        Public Overridable Property Valor As Object

            Get

                Return _valor

            End Get

            Set(value_ As Object)

                _valor = ValidaTipoDato(value_)

                'If IsNothing(_RegistroMovimientos) Then

                '    _RegistroMovimientos = New List(Of RegistroMovimiento)

                'Else

                '    _RegistroMovimientos.Add(New RegistroMovimiento)

                'End If

            End Set

        End Property

        <BsonElement("ValorPresentacion")>
        <BsonIgnoreIfDefault>
        <BsonIgnoreIfNull>
        Public Property ValorPresentacion As String

        <BsonIgnore>
        Public Property ValorFirma As String

        <BsonElement("Nombre")>
        Public Property Nombre As String

        <BsonElement("TipoDato")>
        Public Property TipoDato As TiposDato

        <BsonElement("Longitud")>
        <BsonIgnoreIfDefault>
        <BsonIgnoreIfNull>
        Public Property Longitud As Integer

        <BsonElement("CantidadEnteros")>
        <BsonIgnoreIfDefault>
        <BsonIgnoreIfNull>
        Public Property CantidadEnteros As Integer

        <BsonElement("CantidadDecimales")>
        <BsonIgnoreIfDefault>
        <BsonIgnoreIfNull>
        Public Property CantidadDecimales As Integer

        <BsonElement("TipoRedondeo")>
        <BsonIgnoreIfDefault>
        <BsonIgnoreIfNull>
        Public Property TipoRedondeo As TiposRedondeos

        '<BsonElement("RegistroMovimiento")>
        '<BsonIgnoreIfDefault>
        '<BsonIgnoreIfNull>
        'Public Property RegistroMovimientos As List(Of RegistroMovimiento)

        '<BsonElement("UseAsMetadata")>
        '<BsonIgnoreIfDefault>
        '<BsonIgnoreIfNull>
        <BsonIgnore>
        Public Property UseAsMetadata As Boolean

#End Region

#Region "Methods"

        Public MustOverride Sub Display()

#End Region

#Region "Funciones"

        Private Function ValidaTipoDato(ByVal valor_ As Object)

            If _TipoDato = TiposDato.SinDefinir Then
                Return valor_
            End If

            Dim valorValidado_ = Nothing

            Select Case _TipoDato

                Case TiposDato.Booleano

                    'valorValidado_ = If(valor_ <> "", Convert.ToBoolean(valor_), "")
                    valorValidado_ = If(IsNumeric(valor_), Convert.ToBoolean(valor_), Nothing)

                Case TiposDato.Entero

                    'valorValidado_ = If(valor_ <> "", Integer.Parse(valor_), "")
                    valorValidado_ = If(IsNumeric(valor_), Integer.Parse(valor_), Nothing)

                Case TiposDato.Fecha

                    'If valor_ = "" Then

                    '    valorValidado_ = ""

                    'Else

                    '    Dim dateTime_ As DateTime = DateTime.ParseExact(valor_, "ddMMyyyy", Nothing)

                    '    valorValidado_ = dateTime_.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture)

                    'End If

                    Return valor_

                Case TiposDato.Real

                    'valorValidado_ = If(valor_ <> "", Convert.ToDecimal(valor_), "")
                    valorValidado_ = If(IsNumeric(valor_), Convert.ToDecimal(valor_), Nothing)

                Case TiposDato.Texto

                    valorValidado_ = CType(valor_, String)

                Case TiposDato.IdObject

                    valorValidado_ = CType(valor_, ObjectId)

                Case Else

                    valorValidado_ = CType(valor_, String)

            End Select

            Return valorValidado_

        End Function

#End Region

    End Class

End Namespace
