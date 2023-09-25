Imports System.Web
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Nodo
Imports MongoDB.Bson.Serialization

Public Class Organismo
    Inherits System.Web.UI.Page

#Region "Enum"
    Public Enum Datos
        SinDefinir = 0
        PaginaReciente = 1
        OficinaReciente = 2
        PerfilUsuario = 3
        SessionID = 4
    End Enum

    Public Enum Cookies
        MiSesion = 1
        MiCache = 2

    End Enum


    Public Enum Modalidad
        Intrinseco = 1
        Extrinseco = 2
    End Enum

#End Region

#Region "Constructores"

    Sub New()

    End Sub

#End Region

#Region "Propiedades"

#End Region

#Region "Métodos"


    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        If eliminar_ Then
            'misdatos_
            Response.Cookies.Item(variableGlobal_.ToString).Value = Nothing
            Response.Cookies.Remove(variableGlobal_.ToString)
            Response.Cookies.Clear()
            Response.Cookies.Add(New HttpCookie(variableGlobal_.ToString, ""))
            Response.Cookies(variableGlobal_.ToString).Expires = DateTime.Now.AddDays(-1)
            Session.Abandon()

            Return Nothing

        Else

            Return Request.Cookies(variableGlobal_.ToString)

        End If

    End Function
    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                          ByVal dato_ As Datos,
                                          ByVal crearSiNoExiste_ As Boolean,
                                          Optional ByVal valorAsignado_ As Object = Nothing) As HttpCookie
        Return PreferenciasUsuario(variableGlobal_, dato_, crearSiNoExiste_, valorAsignado_)


    End Function

    Function PreferenciasUsuario(ByVal variableGlobal_ As Cookies,
                                 ByVal dato_ As Datos,
                                 ByVal crearSiNoExiste_ As Boolean,
                                 Optional ByVal valorAsignado_ As Object = Nothing,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        Dim cookie_ As HttpCookie

        If eliminar_ Then
            'misdatos_
            Response.Cookies.Item(variableGlobal_.ToString).Value = Nothing

            Response.Cookies.Remove(variableGlobal_.ToString)

            Response.Cookies.Clear()

            Response.Cookies.Add(New HttpCookie(variableGlobal_.ToString, ""))

            Response.Cookies(variableGlobal_.ToString).Expires = DateTime.Now.AddDays(-1)

            Session.Abandon()

            Return Nothing

        End If

        cookie_ = Request.Cookies(variableGlobal_.ToString)

        If crearSiNoExiste_ Then

            If cookie_ Is Nothing Then
                'No existe
                cookie_ = New HttpCookie(variableGlobal_.ToString)

                cookie_.Values.Add(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                System.Web.HttpContext.Current.Response.AppendCookie(cookie_)
            Else
                'Existe Request.RawUrl
                cookie_ = Request.Cookies(variableGlobal_.ToString)

                cookie_.Values.Set(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                Response.Cookies.Set(cookie_)

            End If

        End If

        Return cookie_

    End Function

    Public Shared Function ObtenerNombrePagina(ByVal pagina_ As String) As String

        Dim indice_ As Integer, finindice_ As Integer

        Dim nombrePagina_ As String = ""

        indice_ = pagina_.LastIndexOf("/") 'Encontrar el ultimo / para determinar donde inicia el nombre

        indice_ += 1

        finindice_ = pagina_.LastIndexOf("?")

        If (finindice_ = -1) Then 'No hay parametros se toma todo el tamaño de la cadena

            finindice_ = pagina_.Length

        End If

        nombrePagina_ = pagina_.Substring(indice_, (finindice_ - indice_))

        Return nombrePagina_

    End Function

    Public Function SeparacionPalabras(oracion_ As String,
                                       campo_ As String,
                                       anexo_ As String,
                                       valoranexo_ As String,
                                       tipo_ As String) As String

        Dim sentencia_ As String

        Dim arreglopalabras_ = New ArrayList

        Dim arreglopalabras2_ = New ArrayList

        Dim palabras_ As String()

        If tipo_ = "itext" Then

            sentencia_ = "{$text:{$search:" & Chr(34) & Chr(92) & Chr(34) &
                              String.Join(Chr(92) & Chr(34) & " " & Chr(92) & Chr(34), oracion_.Split(" ")) &
                     Chr(92) & Chr(34) & Chr(34) & "}}"
        Else

            oracion_ = oracion_.Replace("a", "[Á,Ä,À,Â,Ã,a,á,à,ä,ã,â,å,ã]").Replace("e", "[É,Ë,È,Ê,ê,e,é,è,ë]").Replace("i", "[Ì,Í,Î,Ï,i,í,ì,ï,î,]").Replace("o", "[Ó,Ô,Ò,Õ,Ö,o,ó,ò,ö,ô,ð,õ]").Replace("u", "[Ú,Û,Ù,Ü,u,ú,ù,ü,û,]").Trim

            palabras_ = oracion_.Split(" ")

            For Each palabra_ In palabras_

                Dim pos_ = arreglopalabras2_.IndexOf(palabra_)

                If pos_ = -1 Then

                    arreglopalabras_.Add(palabra_)

                    arreglopalabras2_.Add(palabra_)

                Else

                    arreglopalabras_(pos_) = arreglopalabras_(pos_) & ".*" & palabra_

                End If

            Next

            sentencia_ = ""

            For Each elemento_ In arreglopalabras_

                If elemento_.IndexOf("*") >= 0 Then

                    sentencia_ = sentencia_ & "{" & campo_ & ":{$regex:'" & elemento_ & "', $options:'si'}},"

                Else

                    If elemento_.length = 3 Then

                        sentencia_ = sentencia_ & "{" & campo_ & ":{$regex:'^" & elemento_ & " ', $options:'i'}},"

                    Else

                        sentencia_ = sentencia_ & "{" & campo_ & ":{$regex:'" & elemento_ & "', $options:'i'}},"

                    End If

                End If

            Next

            If sentencia_ <> "" Then

                If anexo_ <> "" Then

                    sentencia_ = sentencia_ & "{" & anexo_ & ": " & valoranexo_ & "},"

                End If

                sentencia_ = "{$and:[" & sentencia_.Substring(0, sentencia_.Length - 1) & "]}"

            End If

        End If

        Return sentencia_

    End Function




    Public Function ObtenerSelectOption(SelectControl_ As Object,
                                        ListaSelectOption_ As List(Of ValorProvisionalOption)) As List(Of SelectOption)

        Dim ultimoValor_ As ValorProvisionalOption

        Dim temporal_ As New List(Of SelectOption)

        Dim cuenta_ As Int16 = 0

        If SelectControl_.Value = "" Then

            ultimoValor_ = Nothing

        Else

            ultimoValor_ = New ValorProvisionalOption With {.Id = New ObjectId(SelectControl_.Value.ToString),
                                                            .Valor = SelectControl_.Text.ToString.ToUpper}

        End If

        If ultimoValor_ IsNot Nothing Then

            If ListaSelectOption_.Find(Function(e) e.Id = ultimoValor_.Id) Is Nothing Then

                temporal_.Add(New SelectOption With {.Value = ultimoValor_.Id.ToString,
                                                     .Indice = cuenta_,
                                                     .Text = ultimoValor_.Valor.ToUpper})

                cuenta_ += 1

            End If

        End If

        For Each SelectOption_ In ListaSelectOption_

            temporal_.Add(New SelectOption With {.Value = SelectOption_.Id.ToString,
                                                 .Indice = cuenta_,
                                                 .Text = SelectOption_.Valor.ToUpper})

            cuenta_ += 1

        Next

        Return temporal_

    End Function

    Public Function ObtenerRutaCampo(ByVal documentoElectronico_ As DocumentoElectronico,
                                     idUnicoSeccion_ As Integer,
                                     idUnicoCampo_ As Integer) As String

        For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In documentoElectronico_.EstructuraDocumento.Parts

            Dim rutaSeccion_ = BuscarRutaNodo(parDatos_.Value,
                                              idUnicoSeccion_,
                                              idUnicoCampo_,
                                              Nodo.TiposNodo.Nodo)

            If rutaSeccion_ <> "" Then

                Return parDatos_.Key & rutaSeccion_

            End If

        Next

        Return Nothing

    End Function

    Private Function BuscarRutaNodo(ByVal nodos_ As List(Of Nodo),
                                    ByVal idUnico_ As Integer,
                                    ByVal idUnicoCampo_ As Integer,
                                    ByVal tipoNodo_ As TiposNodo,
                                    Optional indice_ As Int32 = 0) As String

        If nodos_ IsNot Nothing Then

            For Each nodoContexto_ As Nodo In nodos_

                Select Case nodoContexto_.TipoNodo

                    Case TiposNodo.Campo

                        Dim campo_ = CType(nodoContexto_, DecoradorCampo)

                        If campo_.IDUnico = idUnicoCampo_ Then

                            Return "(" & indice_ & ")SI"

                        End If

                    Case TiposNodo.Seccion

                        Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                        If seccion_.IDUnico = idUnico_ Then

                            If idUnicoCampo_ = 0 Then

                                Return "(" & indice_ & ")SI"
                            Else

                                Return "(" & indice_ & ").Nodos" & BuscarRutaNodo(nodoContexto_.Nodos,
                                                                                  idUnico_,
                                                                                  idUnicoCampo_,
                                                                                  TiposNodo.Nodo)

                            End If
                            'Else
                            '    If seccion_.Nodos IsNot Nothing Then
                            '        If seccion_.Nodos.Count > 0 Then
                            '            Return "(" & indice_ & ").Nodos" & BuscarRutaNodo(seccion_.Nodos,
                            '                                                          idUnico_,
                            '                                                          idUnicoCampo_,
                            '                                                          TiposNodo.Nodo)

                            '        End If
                            '    End If

                        End If


                    Case TiposNodo.Nodo

                        Dim respuesta_ = BuscarRutaNodo(nodoContexto_.Nodos,
                                                        idUnico_,
                                                        idUnicoCampo_,
                                                        TiposNodo.Nodo)

                        If respuesta_ IsNot Nothing Then

                            If respuesta_.Contains("SI") Then

                                Return "(" & indice_ & ").Nodos" & respuesta_

                            Else

                                Return ""

                            End If

                        End If

                End Select

                indice_ += 1

            Next

        Else

            Return ""

        End If

    End Function

    ' ESTÁ OBSOLETA
    Public Function RemplazaNodo(DocumentoId_ As ObjectId,
                                 ByVal documentoElectronico_ As DocumentoElectronico,
                                 seccion_ As Int32) As String

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
             {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim operationsDB_ As IMongoCollection(Of BsonDocument) =
                _enlaceDatos.GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

            Dim match_ As String = "{'_id':ObjectId('" & DocumentoId_.ToString & "'),'Nodillo.Nodos':{$exists:true}}"

            Dim numcampo_ As Int32 = 1

            Dim ruta_ = ObtenerRutaCampo(documentoElectronico_, seccion_, 0)

            ruta_ = ruta_.Substring(0, ruta_.Length - 2)

            Dim puntos_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                            ruta_.Replace("(", ".").Replace(")", "") & ".Nodos"

            Dim corchete_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                               ruta_.Replace("(", "[").Replace(")", "]") & ".Nodos[0]"

            Dim consulta_ As String = "{NodoAux:" & ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_, seccion_, 0)) & "}"

            Dim fieldValue_ As BsonValue = Nothing

            operationsDB_.Aggregate().Project(BsonDocument.Parse(consulta_)).
                                      Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$type:'$NodoAux.Nodos.0'}}")).
                                      Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$cond:{if:{$eq:['$NN','array']}, then: '$NodoAux.Nodos'," &
                                                                                           " else:['$NodoAux.Nodos.0']}}}")).
                                      Match(BsonDocument.Parse(match_)).
                                      ToList().ForEach(Sub(estatus_)

                                                           fieldValue_ = estatus_.GetValue("NN")

                                                       End Sub)
            If fieldValue_ IsNot Nothing Then

                Dim filter_ As BsonDocument = New BsonDocument("_id", New BsonObjectId(DocumentoId_))

                Dim updateField_ As BsonDocument = New BsonDocument(puntos_, fieldValue_)

                ' Crear el objeto de actualización
                Dim update_ As BsonDocument = New BsonDocument("$set", updateField_)

                ' Realizar la actualización
                operationsDB_.UpdateOne(filter_, update_)

            End If

        End Using

    End Function

    ' ESTÁ OBSOLETA
    Public Function RemplazaNodo(OperacionGenerica_ As OperacionGenerica,
                                 ByVal documentoElectronico_ As DocumentoElectronico,
                                 seccion_ As Int32, modalidad_ As Modalidad) As OperacionGenerica

        If modalidad_ = Modalidad.Extrinseco Then

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
             {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ As IMongoCollection(Of BsonDocument) =
                        _enlaceDatos.GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

                Dim match_ As String = "{'_id':ObjectId('" & OperacionGenerica_.Id.ToString & "')}"

                Dim numcampo_ As Int32 = 1

                Dim ruta_ = ObtenerRutaCampo(documentoElectronico_, seccion_, 0)

                ruta_ = ruta_.Substring(0, ruta_.Length - 2)

                Dim puntos_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                               ruta_.Replace("(", ".").Replace(")", "") & ".Nodos"

                Dim corchete_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                ruta_.Replace("(", "[").Replace(")", "]") & ".Nodos[0]"

                Dim consulta_ As String = "{NodoAux:" & ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_, seccion_, 0)) & "}"

                Dim fieldValue_ As BsonValue

                operationsDB_.Aggregate().Project(BsonDocument.Parse(consulta_)).
                                          Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$type:'$NodoAux.Nodos.0'}}")).
                                          Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$cond:{if:{$eq:['$NN','array']}, " &
                                                                     "then: '$NodoAux.Nodos', else:['$NodoAux.Nodos.0']}}}")).
                                          Match(BsonDocument.Parse(match_)).
                                          ToList().ForEach(Sub(estatus_)

                                                               fieldValue_ = estatus_.GetValue("NN")

                                                           End Sub)

                Dim filter_ As BsonDocument = New BsonDocument("_id", New BsonObjectId(OperacionGenerica_.Id))

                Dim updateField_ As BsonDocument = New BsonDocument(puntos_, fieldValue_)

                ' Crear el objeto de actualización
                Dim update_ As BsonDocument = New BsonDocument("$set", updateField_)

                ' Realizar la actualización

                operationsDB_.UpdateOne(filter_, update_)

            End Using

            Dim Nodo_ = OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
                        EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos(0).Nodos

            OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
             EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos = Nodo_

        Else

            Dim Nodo_ = OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
                        EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos(0).Nodos

            OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
                               EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos = Nodo_

        End If

        Return OperacionGenerica_

    End Function
    Private Function ObtenerRutaMongo(ruta_ As String) As String

        Dim indiceInicial_, indiceFinal_ As Int32

        Dim temporal_ = ruta_

        Dim instruccionMongo_ = "'$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts"

        Dim posiciones_ As New List(Of String)

        While temporal_ <> ""

            indiceInicial_ = temporal_.IndexOf("(")

            indiceFinal_ = temporal_.IndexOf(")")

            If indiceInicial_ < 0 Then

                temporal_ = ""

            Else

                posiciones_.Add(temporal_.Substring(indiceInicial_ + 1, indiceFinal_ - indiceInicial_ - 1))

                instruccionMongo_ &= "." & temporal_.Substring(0, indiceInicial_)

                If indiceFinal_ + 2 > temporal_.Length Then

                    temporal_ = ""

                Else

                    temporal_ = temporal_.Substring(indiceFinal_ + 2, temporal_.Length - indiceFinal_ - 2)

                End If

            End If

        End While

        instruccionMongo_ &= "'"

        For Each posicion_ In posiciones_

            instruccionMongo_ = "{$arrayElemAt:[" & instruccionMongo_ & "," & posicion_ & "]}"

        Next

        Return instruccionMongo_

    End Function
    'dim f1 as Costrucu = Obtee(of ConstructorFacCom)()
    Public Function ObtenerCamposSeccionExterior(listaDocumentoId_ As List(Of ObjectId),
                                                 ByVal documentoElectronico_ As DocumentoElectronico,
                                                 listaSeccion_ As Dictionary(Of [Enum], List(Of [Enum]))) As Dictionary(Of ObjectId, List(Of Nodo))

        'Dim respuesta_ As New TagWatcher

        'respuesta_.SetOK()
        'respuesta_.ObjectReturn = DirectCast(1, T)


        Dim bulkCamposPedidos_ As Dictionary(Of ObjectId, List(Of Nodo))

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim consulta_ As String = ""

            Dim match_ As String = ""

            Dim operationsDB_ As IMongoCollection(Of BsonDocument) = enlaceDatos_.
                                  GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

            Dim numeroCampo_ As Int32 = 1

            For Each seccion_ As KeyValuePair(Of [Enum], List(Of [Enum])) In listaSeccion_

                If seccion_.Value Is Nothing Then

                    Dim seccionAuxiliar_ As Object = seccion_.Key

                    consulta_ = consulta_ & "campo" & numeroCampo_ & ":" &
                                ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                          seccionAuxiliar_, 0)) & ","

                    numeroCampo_ += 1

                Else

                    For Each campo_ In seccion_.Value

                        Dim campoEntero_ As Object = campo_

                        Dim seccionEntero_ As Object = seccion_.Key

                        consulta_ = consulta_ & "campo" & numeroCampo_ & ":" &
                                    ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                                      seccionEntero_,
                                                                      campoEntero_)) & ","

                        numeroCampo_ += 1

                    Next

                End If

            Next

            Dim algo_ = consulta_
            consulta_ = "{" & consulta_.Substring(0, consulta_.Length - 1) & "}"

            bulkCamposPedidos_ = New Dictionary(Of ObjectId, List(Of Nodo))

            For Each documentoId_ In listaDocumentoId_

                match_ &= "ObjectId('" & documentoId_.ToString & "'),"

                bulkCamposPedidos_.Add(documentoId_, New List(Of Nodo))

            Next

            match_ = "{'_id':{$in:[" & match_.Substring(0, match_.Length - 1) & "]}}"

            operationsDB_.Aggregate().
                          Project(BsonDocument.Parse(consulta_)).Match(BsonDocument.Parse(match_)).
                          ToList().ForEach(Sub(estatus_)

                                               For cadena = 1 To estatus_.ElementCount - 1

                                                   bulkCamposPedidos_(New ObjectId(estatus_.GetElement("_id").Value.ToString)).
                                                   Add(BsonSerializer.Deserialize(Of Nodo)(estatus_.GetElement("campo" & cadena).Value.AsBsonDocument))

                                               Next

                                           End Sub)

        End Using

        Return bulkCamposPedidos_

    End Function

    Public Function ObtenerCamposSeccionExterior(listaDocumentoFolio_ As List(Of String),
                                                  ByVal documentoElectronico_ As DocumentoElectronico,
                                                  listaSecciones_ As Dictionary(Of [Enum], List(Of [Enum]))) As Dictionary(Of String, List(Of Nodo))

        Dim bulkCamposPedidos_ As Dictionary(Of String, List(Of Nodo))

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim consulta_ As String = ""

            Dim match_ As String = ""

            Dim operationsDB_ As IMongoCollection(Of BsonDocument) =
                enlaceDatos_.GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

            Dim numeroCampo_ As Int32 = 1

            For Each seccion_ As KeyValuePair(Of [Enum], List(Of [Enum])) In listaSecciones_

                If seccion_.Value Is Nothing Then

                    Dim seccionAuxiliar_ As Object = seccion_.Key

                    consulta_ &= "campo" & numeroCampo_ & ":" &
                                  ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                                    seccionAuxiliar_, 0)) & ","

                    numeroCampo_ += 1

                Else

                    For Each campo_ In seccion_.Value

                        Dim campoAuxiliar_ As Object = campo_

                        Dim seccionAuxiliar_ As Object = seccion_.Key

                        consulta_ = consulta_ & "campo" & numeroCampo_ & ":" &
                                    ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                                      seccionAuxiliar_,
                                                                      campoAuxiliar_)) & ","

                        numeroCampo_ += 1

                    Next

                End If

            Next

            consulta_ = "{" & consulta_.Substring(0, consulta_.Length - 1) &
                        ", FolioDocumento:'$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento'}"

            bulkCamposPedidos_ = New Dictionary(Of String, List(Of Nodo))

            For Each documentoFolio_ In listaDocumentoFolio_

                match_ &= "'" & documentoFolio_ & "',"

                bulkCamposPedidos_.Add(documentoFolio_, New List(Of Nodo))

            Next

            match_ = "{FolioDocumento:{$in:[" & match_.Substring(0, match_.Length - 1) & "]}}"

            operationsDB_.Aggregate().
                          Project(BsonDocument.Parse(consulta_)).Match(BsonDocument.Parse(match_)).
                          ToList().ForEach(Sub(estatus_)

                                               If estatus_.ElementCount = 2 Then


                                                   bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).
                                                       Add(BsonSerializer.Deserialize(Of Nodo) _
                                                       (estatus_.GetElement("campo1").Value.AsBsonDocument))

                                               Else
                                                   For cadena_ = 1 To estatus_.ElementCount - 2

                                                       bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).
                                                       Add(BsonSerializer.Deserialize(Of Nodo) _
                                                       (estatus_.GetElement("campo" & cadena_).Value.AsBsonDocument))

                                                   Next

                                               End If


                                               Dim nodoId_ =
                                               BsonSerializer.Deserialize(Of Nodo)(estatus_.GetElement("campo1").Value.AsBsonDocument)

                                               While nodoId_.DescripcionTipoNodo <> "Campo"

                                                   nodoId_ = nodoId_.Nodos(0)

                                               End While

                                               DirectCast(nodoId_, Campo).Valor = estatus_.GetElement("_id").Value.ToString

                                               DirectCast(nodoId_, Campo).ValorPresentacion = estatus_.GetElement("_id").Value.ToString

                                               DirectCast(nodoId_, Campo).Nombre = "ID"

                                               bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).Add(nodoId_)

                                           End Sub)
        End Using

        Return bulkCamposPedidos_

    End Function

    '    Public Function ActualizaMultiplesCampos(Of T)(builder_ As UpdateDefinitionBuilder(Of T), objeto_ As T) As UpdateDefinition(Of T)

    '        Dim propiedades_ = objeto_.GetType().GetProperties()
    '        Dim definicion_ As UpdateDefinition(Of T) = Nothing
    '        For Each propiedad_ In propiedades_

    '            If definicion_ Is Nothing Then
    '                definicion_ = Builders(Of T).Update.Set(Of T)(propiedad_.Name, propiedad_.GetValue(objeto_))
    '            Else
    '                definicion_ = definicion_.Set(Of T)(propiedad_.Name, propiedad_.GetValue(objeto_))
    '            End If
    '        Next
    '        Return definicion_

    '    End Function
#End Region

End Class

'Public Class SeccionesCamposGenerales
'    Public Property Seccion As Int32
'    Public Property Campos As List(Of Int32)
'    Public Property Valor As List(Of String)
'End Class




Public Class ValorProvisionalOption
    Public Property Id As ObjectId
    Public Property Valor As String
End Class
