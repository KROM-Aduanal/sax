
Imports gsol
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Nucleo.Recursos
Imports Syn.Operaciones.DocumentoElectronicoObjetoActualizador

Namespace Syn.Operaciones

    <Serializable()>
    Public Class OperacionesNodosNoSQL
        Inherits OperacionesNodos

#Region "Attributes"


#End Region

#Region "Properties"


#End Region

#Region "Builders"

        Public Sub New()

        End Sub

        Public Sub New(ByVal espacioTrabajo_ As IEspacioTrabajo)

            EspacioTrabajo = espacioTrabajo_

        End Sub

#End Region

#Region "Methods"


#End Region

#Region "Functions"


        Public Function PreparaMetaDatos(ByVal nodos_ As List(Of Nodo)) As List(Of CampoGenerico)

            Dim listaCampos_ As New List(Of CampoGenerico)

            For Each nodo_ As Nodo In nodos_

                Dim nodoAux_ = Nothing

                Select Case nodo_.TipoNodo

                    Case TiposNodo.Campo

                        Dim campoNuevo_ = CType(nodo_, CampoGenerico)

                        If campoNuevo_.UseAsMetadata = True Then

                            listaCampos_.Add(campoNuevo_)

                        End If

                    Case Else

                        If nodo_.Nodos IsNot Nothing Then

                            nodoAux_ = PreparaMetaDatos(nodos_:=nodo_.Nodos)

                            If nodoAux_ IsNot Nothing Then

                                For Each subnodo_ As Object In nodoAux_

                                    listaCampos_.Add(subnodo_)

                                Next

                            End If

                        End If

                End Select

            Next

            If Not IsNothing(listaCampos_) Then

                If listaCampos_.Count > 0 Then

                    Return listaCampos_

                End If

            End If

            Return Nothing

        End Function

        Public Function AnalizaDiferenciasNodos(ByVal nodosNuevo_ As List(Of Nodo),
                                                ByVal nodosOriginal_ As List(Of Nodo)) As List(Of DocumentoElectronicoObjetoActualizador)

            Dim listaActualizaciones_ As New List(Of DocumentoElectronicoObjetoActualizador)

            Dim contadorNodo_ As Int32 = 0

            Dim cortarActualizacion_ As Boolean = False

            For Each nodo_ As Nodo In nodosNuevo_

                Dim listaActualizacionesHijo_ = New List(Of DocumentoElectronicoObjetoActualizador)

                'Dim listaActualizacionesHijo_ As New List(Of String)

                Dim rutaActualizacion_ = contadorNodo_.ToString

                Dim nodoAux_ = Nothing

                Select Case nodo_.TipoNodo

                    Case TiposNodo.Nodo

                        nodoAux_ = nodosOriginal_

                    Case TiposNodo.Seccion

                        Dim seccionNueva_ = CType(nodo_, SeccionGenerica)

                        nodoAux_ = ObtenerNodo(nodos_:=nodosOriginal_,
                                               IdUnico_:=seccionNueva_.IDUnico,
                                               tipoNodo_:=TiposNodo.Seccion)

                        If Not IsNothing(nodoAux_) Then

                            nodoAux_ = nodoAux_.Nodos

                        Else

                            listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                            .RutaActualizacion = rutaActualizacion_,
                                            .Valor = seccionNueva_,
                                            .TipoDato = TiposDato.Documento
                                            })

                            cortarActualizacion_ = True

                        End If


                    Case TiposNodo.Partida

                        Dim partidaNueva_ = CType(nodo_, PartidaGenerica)

                        nodoAux_ = ObtenerNodoPartida(nodos_:=nodosOriginal_,
                                                      consecutivo_:=partidaNueva_.NumeroSecuencia,
                                                      tipoNodo_:=TiposNodo.Partida)

                        If Not IsNothing(nodoAux_) Then

                            Dim partidaOriginal_ = CType(nodoAux_, PartidaGenerica)

                            'If Not partidaNueva_.NumeroSecuencia = partidaOriginal_.NumeroSecuencia Then

                            '    listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                            '            .RutaActualizacion = rutaActualizacion_,
                            '            .PropiedadActualizar = "NumeroSecuencia",
                            '            .Valor = partidaNueva_,
                            '            .TipoDato = TiposDato.Entero
                            '    })

                            'End If

                            If Not partidaNueva_.estado = partidaOriginal_.estado Then

                                listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                        .RutaActualizacion = rutaActualizacion_,
                                        .PropiedadActualizar = "estado",
                                        .Valor = partidaNueva_.estado,
                                        .TipoDato = TiposDato.Entero
                                })

                            End If
                            'ARCHIVADO EN EL COMPARADOR
                            If Not partidaNueva_.archivado = partidaOriginal_.archivado Then

                                listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                        .RutaActualizacion = rutaActualizacion_,
                                        .PropiedadActualizar = "archivado",
                                        .Valor = partidaNueva_.archivado,
                                        .TipoDato = TiposDato.Booleano
                                })

                            End If

                            nodoAux_ = nodoAux_.Nodos

                        Else

                            listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                            .RutaActualizacion = rutaActualizacion_,
                                            .Valor = partidaNueva_,
                                            .TipoDato = TiposDato.Documento,
                                            .TipoNodo = TiposNodo.Partida
                                            })

                            cortarActualizacion_ = True


                        End If

                    Case TiposNodo.Campo

                        Dim campoNuevo_ = CType(nodo_, CampoGenerico)

                        nodoAux_ = ObtenerNodo(nodos_:=nodosOriginal_,
                                               IdUnico_:=campoNuevo_.IDUnico,
                                               tipoNodo_:=TiposNodo.Campo)


                        Dim campoOriginal_ = CType(nodoAux_, CampoGenerico)


                        If Not EsVacioNodoCampoValor(campoOriginal_) Then

                            If Not EsVacioNodoCampoValor(campoNuevo_) Then

                                If campoOriginal_.TipoDato = TiposDato.Fecha Then

                                    campoOriginal_.Valor = Convert.ToDateTime(campoOriginal_.Valor).Date.ToString("yyyy-MM-dd")

                                    campoNuevo_.Valor = Convert.ToDateTime(campoNuevo_.Valor).Date.ToString("yyyy-MM-dd")

                                End If

                                If Not campoNuevo_.Valor = campoOriginal_.Valor Then

                                    listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                            .RutaActualizacion = rutaActualizacion_,
                                            .PropiedadActualizar = "Valor",
                                            .Valor = campoNuevo_.Valor,
                                            .TipoDato = campoNuevo_.TipoDato
                                    })

                                End If

                                If Not EsVacioNodoCampoValor(campoNuevo_, True) Then

                                    If Not campoNuevo_.ValorPresentacion = campoOriginal_.ValorPresentacion Then

                                        listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                        .RutaActualizacion = rutaActualizacion_,
                                        .PropiedadActualizar = "ValorPresentacion",
                                        .Valor = campoNuevo_.ValorPresentacion,
                                        .TipoDato = TiposDato.Texto
                                    })

                                    End If

                                End If

                            End If

                        Else

                            If Not EsVacioNodoCampoValor(campoNuevo_) Then

                                listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                            .RutaActualizacion = rutaActualizacion_,
                                            .PropiedadActualizar = "Valor",
                                            .Valor = campoNuevo_.Valor,
                                            .TipoDato = campoNuevo_.TipoDato
                                    })

                                If Not EsVacioNodoCampoValor(campoNuevo_, True) Then

                                    listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                            .RutaActualizacion = rutaActualizacion_,
                                            .PropiedadActualizar = "ValorPresentacion",
                                            .Valor = campoNuevo_.ValorPresentacion,
                                            .TipoDato = TiposDato.Texto
                                     })

                                End If

                            End If

                        End If

                End Select

                If Not cortarActualizacion_ Then

                    If Not IsNothing(nodo_.Nodos) Then

                        listaActualizacionesHijo_ = AnalizaDiferenciasNodos(nodosNuevo_:=nodo_.Nodos,
                                                                            nodosOriginal_:=nodoAux_)

                        If Not IsNothing(listaActualizacionesHijo_) Then

                            If listaActualizacionesHijo_.Count > 0 Then

                                For Each actualizacion_ As DocumentoElectronicoObjetoActualizador In listaActualizacionesHijo_

                                    listaActualizaciones_.Add(New DocumentoElectronicoObjetoActualizador With {
                                        .RutaActualizacion = rutaActualizacion_ & ".Nodos." & actualizacion_.RutaActualizacion,
                                        .Valor = actualizacion_.Valor,
                                        .TipoDato = actualizacion_.TipoDato,
                                        .PropiedadActualizar = actualizacion_.PropiedadActualizar,
                                        .TipoNodo = actualizacion_.TipoNodo
                                        })

                                Next

                            End If

                        End If

                    End If

                End If

                contadorNodo_ += 1

            Next

            If Not IsNothing(listaActualizaciones_) Then

                If listaActualizaciones_.Count > 0 Then

                    Return listaActualizaciones_

                End If

            End If

            Return Nothing

        End Function

        'Public Function AnalizaRegistroMovimientos(ByVal campoNuevo_ As Campo,
        '                                           ByVal campoOriginal_ As Campo) As List(Of String)

        '    Dim listaActualizaciones_ As New List(Of String)

        '    If campoNuevo_.RegistroMovimientos.Count > campoOriginal_.RegistroMovimientos.Count Then

        '        For contador_ As Integer = campoOriginal_.RegistroMovimientos.Count To campoNuevo_.RegistroMovimientos.Count - 1

        '            listaActualizaciones_.Add("RegistroMovimiento." & contador_ & ".Evento=" & EventosGenericos.Actualizacion & "|Evento->" & EventosGenericos.Actualizacion & "|Entero")

        '            listaActualizaciones_.Add("RegistroMovimiento." & contador_ & ".DescripcionEvento=" & GetEnumDescription(DirectCast(Convert.ToInt32(EventosGenericos.Actualizacion), EventosGenericos)) & "|Evento->" & GetEnumDescription(DirectCast(Convert.ToInt32(EventosGenericos.Actualizacion), EventosGenericos)) & "|Texto")

        '            'listaActualizaciones_.Add("RegistroMovimiento." & contador_ & ".IDUsuario=" & EspacioTrabajo.MisCredenciales.ClaveUsuario & "|Evento->" & EspacioTrabajo.MisCredenciales.ClaveUsuario & "|Entero")

        '            'listaActualizaciones_.Add("RegistroMovimiento." & contador_ & ".NombreUsuario=" & EspacioTrabajo.MisCredenciales.NombreAutenticacion & "|Evento->" & EspacioTrabajo.MisCredenciales.NombreAutenticacion & "|Texto")

        '            listaActualizaciones_.Add("RegistroMovimiento." & contador_ & ".FechaEvento=" & Date.Now & "|Evento->" & Date.Now & "|Fecha")

        '            listaActualizaciones_.Add("RegistroMovimiento." & contador_ & ".Texto= El valor era ->" & campoOriginal_.Valor & "<- y el valor nuevo es ->" & campoNuevo_.Valor & "<-|Evento-> & EventosGenericos.Actualizacion " & "|Texto")

        '        Next contador_

        '        Return listaActualizaciones_

        '    Else

        '        Return Nothing

        '    End If

        'End Function

#End Region

    End Class

    Public Class DocumentoElectronicoObjetoActualizador


        Public TipoNodo As TiposNodo

        Public TipoDato As TiposDato

        Public PropiedadActualizar As String

        Public RutaActualizacion As String

        Public Valor As Object


        Sub New()

            TipoNodo = TiposNodo.SinDefinir

            TipoDato = TiposDato.SinDefinir

        End Sub

    End Class

End Namespace