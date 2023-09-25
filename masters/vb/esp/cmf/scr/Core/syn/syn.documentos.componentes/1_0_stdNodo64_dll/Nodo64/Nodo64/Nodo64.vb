Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Bson.Serialization.Options
Imports MongoDB.Bson.Serialization.Serializers
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones



'<BsonKnownTypes(GetType(DecoradorSeccion))>
'<Serializable()>
'<BsonKnownTypes(GetType(Campo), GetType(Seccion), GetType(Partida), GetType(DecoradorNodo))>
Namespace Syn.Documento.Componentes

    <BsonDiscriminator(RootClass:=True)>
    <BsonKnownTypes(GetType(Campo), GetType(Seccion), GetType(Partida), GetType(DecoradorSeccion), GetType(DecoradorNodo))>
    <Serializable()>
    Public MustInherit Class Nodo

        Private _operacionesNodo As OperacionesNodos

        Private _tipoNodo As TiposNodo

#Region "Enum"
        Enum TiposNodo
            SinDefinir = 0
            Nodo = 1
            Campo = 2
            Seccion = 3
            Partida = 4
        End Enum

#End Region

        Public Sub New()

            _operacionesNodo = New OperacionesNodos()

            _estado = 1

            _archivado = False

        End Sub

#Region "Properties"

        <BsonIgnoreIfDefault>
        Public Property idPermiso As Integer

        <BsonElement("TipoNodo")>
        Public Property TipoNodo As TiposNodo

            Get

                Return _tipoNodo

            End Get

            Set(value As TiposNodo)

                _tipoNodo = value

                _DescripcionTipoNodo = GetEnumDescription(DirectCast(Convert.ToInt32(value), TiposNodo))

            End Set

        End Property

        <BsonElement("DescripcionTipoNodo")>
        Public Property DescripcionTipoNodo As String

        '<BsonRepresentation(BsonType.Array)>
        <BsonElement("Nodos")>
        <BsonIgnoreIfNull>
        Public Property Nodos As List(Of Nodo)
        Public Property Campo(ByVal claveCampo_ As Integer) As Campo

            Get

                Return _operacionesNodo.ObtenerNodo(nodos_:=Me.Nodos,
                                                    IdUnico_:=claveCampo_,
                                                    tipoNodo_:=TiposNodo.Campo)

            End Get

            Set(value As Campo)

                'Falta

            End Set

        End Property
        Public Property Attribute(ByVal claveCampo_ As Integer) As Campo

            Get

                Return _operacionesNodo.ObtenerNodo(nodos_:=Me.Nodos,
                                                    IdUnico_:=claveCampo_,
                                                    tipoNodo_:=TiposNodo.Campo)


            End Get

            Set(value As Campo)

                'falta

            End Set

        End Property
        Public Property Seccion(ByVal claveSeccion_ As Integer) As Seccion

            Get

                Return _operacionesNodo.ObtenerNodo(nodos_:=Me.Nodos,
                                                    IdUnico_:=claveSeccion_,
                                                    tipoNodo_:=TiposNodo.Seccion)

            End Get

            Set(value As Seccion)

                'Falta

            End Set

        End Property

        <BsonIgnoreIfDefault>
        Public Property TipoDocumentoElectronico As TiposDocumentoElectronico

        <BsonIgnore>
        Public Property Partida(ByVal documento_ As Object) As Partida

            Get

                Return _operacionesNodo.CrearNodoPartida(documento_, CType(Me, DecoradorSeccion))

            End Get

            Set(value As Partida)



            End Set

        End Property

        'Public Property Partida() As Partida

        '    Get

        '        Return _operacionesNodo.CrearNodoPartida(CType(Me, DecoradorSeccion))

        '    End Get

        '    Set(value As Partida)



        '    End Set

        'End Property
        Public Property Partida(ByVal numeroSecuencia_ As Integer) As Partida

            Get

                Return _operacionesNodo.ObtenerNodoPartida(nodos_:=CType(Me, DecoradorSeccion).Nodos,
                                                    consecutivo_:=numeroSecuencia_,
                                                    tipoNodo_:=TiposNodo.Partida)

            End Get

            Set(value As Partida)

            End Set

        End Property

        <BsonIgnore>
        Public ReadOnly Property OperacionesNodo As OperacionesNodos

            Get

                Return _operacionesNodo

            End Get

        End Property

        <BsonElement("estado")>
        Public Property estado As Integer

        <BsonElement("archivado")>
        <BsonIgnoreIfNull>
        Public Property archivado As Boolean

#End Region

    End Class


End Namespace