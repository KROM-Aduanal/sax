
Imports gsol
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Nucleo.Recursos

Namespace Syn.Operaciones

    <Serializable()>
    Public Class OperacionesNodos

#Region "Attributes"


#End Region

#Region "Properties"

        Public Property EspacioTrabajo As IEspacioTrabajo

#End Region

#Region "Builders"
        Public Sub New()

        End Sub

        Public Sub New(ByVal espacioTrabajo_ As IEspacioTrabajo)

            _EspacioTrabajo = espacioTrabajo_

        End Sub

#End Region

#Region "Methods"
        Public Sub AgregaRegistroMovimientos(ByRef nodos_ As List(Of Nodo))

            For Each nodo_ As Nodo In nodos_

                Dim listaActualizacionesHijo_ As New List(Of String)

                Dim nodoAux_ = Nothing

                Select Case nodo_.TipoNodo

                    Case TiposNodo.Campo

                        Dim campo_ = CType(nodo_, DecoradorCampo)

                        Dim esVacio_ As Boolean = False

                        If IsNumeric(campo_.Valor) Then

                            esVacio_ = False

                        Else

                            If campo_.Valor = "" Then

                                esVacio_ = True

                            End If

                        End If

                        If Not esVacio_ Then

                            If Not IsNothing(campo_.Valor) Then

                                'campo_.RegistroMovimientos.Add(New RegistroMovimiento With {
                                '                               .Evento = EventosGenericos.Insercion,
                                '                               .DescripcionEvento = GetEnumDescription(DirectCast(Convert.ToInt32(EventosGenericos.Insercion), EventosGenericos)),
                                '                               .FechaEvento = Date.Now,
                                '                               .Texto = "El valor era ->" & campo_.Valor & "<-"})

                                '' campo_.RegistroMovimientos.Add(New RegistroMovimiento With {
                                ''.Evento = EventosGenericos.Insercion,
                                ''.DescripcionEvento = GetEnumDescription(DirectCast(Convert.ToInt32(EventosGenericos.Insercion), EventosGenericos)),
                                ''.IDUsuario = EspacioTrabajo.MisCredenciales.ClaveUsuario,
                                ''.NombreUsuario = EspacioTrabajo.MisCredenciales.NombreAutenticacion,
                                ''.FechaEvento = Date.Now,
                                ''.Texto = "El valor era ->" & campo_.Valor & "<-"})

                            End If

                        End If

                End Select

                If Not IsNothing(nodo_.Nodos) Then

                    AgregaRegistroMovimientos(nodos_:=nodo_.Nodos)

                End If

            Next

        End Sub

#End Region

#Region "Functions"

        Public Function ObtenerRutaCampo(ByVal documentoElectronico_ As DocumentoElectronico, IdUnicoSeccion As Integer, IdUnicoCampo As Integer) As String

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In documentoElectronico_.EstructuraDocumento.Parts

                Dim session_ = ObtenerNodo(parDatos_.Value, IdUnicoSeccion, TiposNodo.Seccion)

                Dim rutaSeccion_ = BuscarRutaNodo(parDatos_.Value, IdUnicoSeccion, Nodo.TiposNodo.Seccion)

                If session_ IsNot Nothing Then

                    Dim rutaNodo_ = BuscarRutaNodo(session_.Nodos, IdUnicoCampo, Nodo.TiposNodo.Campo)
                    'Dim rutaNodo_ = BuscarRutaNodo(parDatos_.Value, IdUnicoCampo, Nodo.TiposNodo.Campo)
                    If rutaNodo_ IsNot Nothing Then

                        Return parDatos_.Key & rutaSeccion_ & rutaNodo_
                        'Return parDatos_.Key & rutaNodo_

                    End If

                End If

            Next

            Return Nothing

        End Function

        Public Function BuscarRutaNodo(ByVal nodos_ As List(Of Nodo),
                                    ByVal IdUnico_ As Integer,
                                    ByVal tipoNodo_ As TiposNodo,
                                    Optional ByVal rutaActual_ As String = Nothing) As String

            If Not IsNothing(nodos_) Then

                If nodos_.Count = 0 Then

                    Return ".Nodos"

                End If

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = tipoNodo_ Then

                        Select Case nodoContexto_.TipoNodo

                            Case TiposNodo.Campo

                                Dim campo_ = CType(nodoContexto_, DecoradorCampo)

                                If campo_.IDUnico = IdUnico_ Then

                                    Return "" 'nodoContexto_.TipoNodo.ToString

                                End If

                            Case TiposNodo.Seccion

                                Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                                If seccion_.IDUnico = IdUnico_ Then

                                    Return ".Nodos" 'nodoContexto_.TipoNodo.ToString

                                End If

                            Case TiposNodo.Partida

                                Dim partida_ = CType(nodoContexto_, DecoradorPartida)

                                If partida_.NumeroSecuencia = IdUnico_ Then

                                    Return "" 'nodoContexto_.TipoNodo.ToString

                                End If

                        End Select


                    End If

                    If Not IsNothing(nodoContexto_.Nodos) Then

                        If nodoContexto_.Nodos.Count = 0 Then

                            Return ".Nodos"

                        End If

                        Dim rutaAux_ = BuscarRutaNodo(nodos_:=nodoContexto_.Nodos,
                                                        IdUnico_:=IdUnico_,
                                                        tipoNodo_:=tipoNodo_,
                                                        rutaActual_:=rutaActual_)

                        If Not IsNothing(rutaAux_) Then

                            'Return nodoContexto_.TipoNodo.ToString & "." & rutaAux_
                            Return ".Nodos" & rutaAux_

                        End If

                    End If

                Next

            End If

            Return Nothing

        End Function

        Public Sub CrearPartidasDocumentoElectronico(ByRef documentoElectronico_ As DocumentoElectronico)

            For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In documentoElectronico_.EstructuraDocumento.Parts

                CrearPartidasSeccioDocumentoElectronico(documentoElectronico_:=documentoElectronico_, parDatos_.Value)

            Next

        End Sub

        Private Sub CrearPartidasSeccioDocumentoElectronico(ByVal documentoElectronico_ As DocumentoElectronico, ByVal nodos_ As List(Of Nodo))

            If Not IsNothing(nodos_) Then

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = TiposNodo.Seccion Then


                        Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                        If seccion_.Nodos Is Nothing Then

                            CrearNodoPartida(documentoElectronico_, seccion_)

                        End If


                    End If

                    If Not IsNothing(nodoContexto_.Nodos) Then

                        CrearPartidasSeccioDocumentoElectronico(documentoElectronico_:=documentoElectronico_, nodos_:=nodoContexto_.Nodos)

                    End If

                Next

            End If

        End Sub
        Public Function ObtenerNodo(ByVal nodos_ As List(Of Nodo),
                                    ByVal IdUnico_ As Integer,
                                    ByVal tipoNodo_ As TiposNodo) As Nodo

            If Not IsNothing(nodos_) Then

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = tipoNodo_ Then

                        Select Case nodoContexto_.TipoNodo

                            Case TiposNodo.Campo

                                Dim campo_ = CType(nodoContexto_, DecoradorCampo)

                                If campo_.IDUnico = IdUnico_ Then

                                    Return campo_

                                End If

                            Case TiposNodo.Seccion

                                Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                                If seccion_.IDUnico = IdUnico_ Then

                                    Return seccion_

                                End If

                            Case TiposNodo.Partida

                                Dim partida_ = CType(nodoContexto_, DecoradorPartida)

                                If partida_.NumeroSecuencia = IdUnico_ Then

                                    Return partida_

                                End If

                        End Select

                    End If

                    If Not IsNothing(nodoContexto_.Nodos) Then

                        Dim nodoHijo_ = ObtenerNodo(nodos_:=nodoContexto_.Nodos,
                                                        IdUnico_:=IdUnico_,
                                                        tipoNodo_:=tipoNodo_)

                        If Not IsNothing(nodoHijo_) Then

                            Return nodoHijo_

                        End If

                    End If

                Next

            End If

            Return Nothing

        End Function

        Public Function ObtenerSeccionPorCampo(ByVal nodos_ As List(Of Nodo),
                                                    ByVal IdUnico_ As Integer,
                                                    ByVal tipoNodo_ As TiposNodo,
                                                    Optional ByVal seccionPadre_ As DecoradorSeccion = Nothing) As Nodo

            If Not IsNothing(nodos_) Then

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = TiposNodo.Seccion Then

                        seccionPadre_ = CType(nodoContexto_, DecoradorSeccion)

                    End If

                    If nodoContexto_.TipoNodo = tipoNodo_ Then

                        Select Case nodoContexto_.TipoNodo

                            Case TiposNodo.Campo

                                Dim campo_ = CType(nodoContexto_, DecoradorCampo)

                                If campo_.IDUnico = IdUnico_ Then

                                    Return seccionPadre_

                                End If

                                'Case TiposNodo.Seccion

                                '    Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                                '    If seccion_.IDUnico = IdUnico_ Then

                                '        Return seccionPadre_

                                '    End If

                        End Select

                    End If

                    If Not IsNothing(nodoContexto_.Nodos) Then

                        Dim nodoHijo_ = ObtenerSeccionPorCampo(nodos_:=nodoContexto_.Nodos,
                                                        IdUnico_:=IdUnico_,
                                                        tipoNodo_:=tipoNodo_,
                                                        seccionPadre_:=seccionPadre_)

                        If Not IsNothing(nodoHijo_) Then

                            Return nodoHijo_

                        End If

                    End If

                Next

            End If

            Return Nothing

        End Function

        Public Function ObtenerNodoPartida(ByVal nodos_ As List(Of Nodo),
                                           ByVal consecutivo_ As Integer,
                                           ByVal tipoNodo_ As TiposNodo) As Nodo

            If Not IsNothing(nodos_) Then

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = tipoNodo_ Then

                        Dim partida_ = CType(nodoContexto_, DecoradorPartida)

                        If partida_.NumeroSecuencia = consecutivo_ Then

                            Return partida_

                        End If

                    End If

                Next

            End If

            Return Nothing

        End Function

        Public Function ActualizarNodo(ByRef nodos_ As List(Of Nodo),
                                       ByVal IdUnico_ As Integer,
                                       ByVal tipoNodo_ As TiposNodo,
                                       ByVal nodoNuevo_ As Nodo) As Boolean


            If Not IsNothing(nodos_) Then

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = tipoNodo_ Then

                        Select Case nodoContexto_.TipoNodo

                            Case TiposNodo.Campo

                                Dim campo_ = CType(nodoContexto_, DecoradorCampo)

                                If campo_.IDUnico = IdUnico_ Then

                                    campo_ = nodoNuevo_

                                    Return True

                                End If

                            Case TiposNodo.Seccion

                                Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                                If seccion_.IDUnico = IdUnico_ Then

                                    seccion_ = nodoNuevo_

                                    Return True

                                End If

                            Case TiposNodo.Partida

                                Dim partida_ = CType(nodoContexto_, DecoradorPartida)

                                If partida_.NumeroSecuencia = IdUnico_ Then

                                    partida_ = nodoNuevo_

                                    Return True

                                End If

                        End Select

                    End If

                    If Not IsNothing(nodoContexto_.Nodos) Then

                        Dim nodoHijo_ = ObtenerNodo(nodos_:=nodoContexto_.Nodos,
                                                    IdUnico_:=IdUnico_,
                                                    tipoNodo_:=tipoNodo_)

                        If Not IsNothing(nodoHijo_) Then

                            nodoHijo_ = nodoNuevo_

                            Return True

                        End If

                    End If

                Next

            End If

            Return False

        End Function

        Public Function ContarNodosPartidas(ByVal nodos_ As List(Of Nodo)) As Integer

            If Not IsNothing(nodos_) Then

                Dim cantidadPartidas_ = 0

                For Each nodoContexto_ As Nodo In nodos_

                    If nodoContexto_.TipoNodo = TiposNodo.Partida Then

                        cantidadPartidas_ += 1

                    End If

                Next

                Return cantidadPartidas_

            End If

            Return 0

        End Function

        Public Sub AjustarConteoNodosPartidas(ByRef nodos_ As List(Of Nodo))

            If Not IsNothing(nodos_) Then

                Dim cantidadPartidas_ = 0

                For Each nodoContexto_ As PartidaGenerica In nodos_

                    If nodoContexto_.TipoNodo = TiposNodo.Partida And nodoContexto_.estado = 1 Then

                        cantidadPartidas_ += 1

                        nodoContexto_.NumeroSecuencia = cantidadPartidas_

                    End If

                Next

            End If

        End Sub

        Public Function CrearNodoPartida(ByRef documento_ As Object, ByRef seccion_ As DecoradorSeccion) As Partida

            If IsNothing(seccion_.Nodos) Then

                seccion_.Nodos = New List(Of Nodo)

            End If

            Dim partida2_ = New PartidaGenerica()

            With partida2_

                .Nodos = documento_.ObtenerCamposSeccion(seccion_.IDUnico)

            End With

            If Not IsNothing(partida2_) Then

                partida2_.TipoNodo = TiposNodo.Partida

                partida2_.NumeroSecuencia = seccion_.CantidadPartidas + 1

                seccion_.Nodos.Add(partida2_)

                Dim a = seccion_.Nodos.GetType()
                Dim b = a.ToString

            End If

            Return partida2_

        End Function
        Public Function EsVacioNodoCampoValor(ByVal campo_ As Campo,
                                              Optional esValorPresentacion_ As Boolean = False) As Boolean

            If Not IsNothing(campo_) Then

                Dim esVacio_ As Boolean = False

                If Not esValorPresentacion_ Then

                    If Not IsNothing(campo_.Valor) Then

                        Select Case campo_.TipoDato

                            Case Componentes.Campo.TiposDato.Entero, Componentes.Campo.TiposDato.Real

                                If IsNumeric(campo_.Valor) Then

                                    esVacio_ = False

                                Else

                                    esVacio_ = True

                                End If

                            Case Componentes.Campo.TiposDato.IdObject

                                If campo_.Valor Is Nothing Then

                                    esVacio_ = True

                                End If


                            Case Componentes.Campo.TiposDato.Texto

                                If campo_.Valor Is Nothing Then

                                    esVacio_ = True

                                Else

                                    If campo_.Valor = "" Then

                                        esVacio_ = True

                                    Else

                                        esVacio_ = False

                                    End If

                                End If

                        End Select

                        If Not esVacio_ Then

                            Return False

                        Else

                            Return True

                        End If

                    Else

                        Return True

                    End If

                Else

                    If Not IsNothing(campo_.ValorPresentacion) Then

                        If campo_.ValorPresentacion Is Nothing Then

                            esVacio_ = True

                        Else

                            If campo_.ValorPresentacion = "" Then

                                esVacio_ = True

                            Else

                                esVacio_ = False

                            End If

                        End If

                        If Not esVacio_ Then

                            Return False

                        Else

                            Return True

                        End If

                    Else

                        Return True

                    End If

                End If

            Else

                Return True

            End If

        End Function

#End Region

    End Class

End Namespace