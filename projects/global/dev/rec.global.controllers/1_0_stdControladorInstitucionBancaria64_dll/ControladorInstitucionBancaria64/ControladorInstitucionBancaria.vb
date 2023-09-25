Imports gsol.krom
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Rec.Globals.Controllers.IControladorInstitucionBancaria.Modalidades
Imports MongoDB.Bson
Imports System.Net.Mime.MediaTypeNames
Imports System.Text.RegularExpressions

Public Class ControladorInstitucionBancaria
    Implements IControladorInstitucionBancaria, ICloneable, IDisposable


#Region "Atributos"
    Dim _ConservarBancos As Boolean
#End Region

#Region "Propiedades"
    Property InstitucionesBancarias As List(Of InstitucionBancaria) Implements IControladorInstitucionBancaria.InstitucionesBancarias

    Property Estado As TagWatcher Implements IControladorInstitucionBancaria.Estado

    Property ModalidadTrabajo As IControladorInstitucionBancaria.Modalidades Implements IControladorInstitucionBancaria.ModalidadTrabajo

#End Region

#Region "Constructores"
    Sub New()
        _Estado = New TagWatcher
        ModalidadTrabajo = Interno
        _ConservarBancos = True
        _InstitucionesBancarias = New List(Of InstitucionBancaria)
    End Sub
#End Region

#Region "Métodos"

    Public Function NuevoBanco(banco_ As InstitucionBancaria,
                               Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                               Implements IControladorInstitucionBancaria.NuevoBanco
        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).GetType.Name)

                Dim result_ = operationsDB_.InsertOneAsync(operationsDB_.Database.Client.StartSession, banco_)

                If result_.IsCanceled Then

                    .SetError()

                    .ObjectReturned = Nothing

                Else

                    .SetOK()

                    .ObjectReturned = banco_

                End If

            End Using

        End With

        Return _Estado

    End Function

    Public Function ActualizaBanco(banco_ As InstitucionBancaria, Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
        Implements IControladorInstitucionBancaria.ActualizaBanco

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).GetType.Name)

                Dim definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.idinstitucionbancaria, banco_.idinstitucionbancaria)

                definicion_ = definicion_.Set(Function(e) e.otrosaliasinstitucion, banco_.otrosaliasinstitucion)

                definicion_ = definicion_.Set(Function(e) e.razonsocialespaniol, banco_.razonsocialespaniol)

                definicion_ = definicion_.Set(Function(e) e.identificadorbanco, banco_.identificadorbanco)

                definicion_ = definicion_.Set(Function(e) e.domiciliofiscal, banco_.domiciliofiscal)

                definicion_ = definicion_.Set(Function(e) e.tipobanco, banco_.tipobanco)

                definicion_ = definicion_.Set(Function(e) e.archivado, banco_.archivado)

                definicion_ = definicion_.Set(Function(e) e.estado, banco_.estado)

                Dim result_ = operationsDB_.UpdateOne(Function(e) e._id = banco_._id, definicion_)

                If result_.ModifiedCount Then

                    .SetOK()

                    .ObjectReturned = banco_

                Else

                    .SetError()

                    .ObjectReturned = Nothing

                End If

            End Using

        End With

        Return _Estado

    End Function

    Public Function ActualizaBanco(idBanco As ObjectId,
                                   ByVal tokens_ As Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object),
                                   Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
        Implements IControladorInstitucionBancaria.ActualizaBanco

        Dim bandera_ As Boolean = False
        Dim alias_ As String = ""
        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).GetType.Name)

                Dim definicion_ As UpdateDefinition(Of InstitucionBancaria) = Nothing


                For Each token_ As KeyValuePair(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object) In tokens_

                        Select Case token_.Key

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDS

                                definicion_ = Nothing

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDBANCARIO

                                If definicion_ Is Nothing Then

                                    definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.idinstitucionbancaria, token_.Value)

                                Else

                                    definicion_ = definicion_.Set(Function(e) e.idinstitucionbancaria, token_.Value)

                                End If

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.NUMIDBANCARIA

                                If definicion_ Is Nothing Then

                                    definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.identificadorbanco, token_.Value)

                                Else

                                    definicion_ = definicion_.Set(Function(e) e.identificadorbanco, token_.Value)

                                End If

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIAL
                            bandera_ = True
                            alias_ = token_.Value.TipoAlias
                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.AddToSet(Of AliasBancos)(Function(e) e.otrosaliasinstitucion, token_.Value)

                            Else
                                'Dim Algo As Predicate(Of AliasBancos) = Function(z) z.TipoAlias = alias_
                                'definicion_ = definicion_.Set(Function(e) e.obtenervalor, token_.Value)
                                definicion_ = definicion_.AddToSet(Of AliasBancos)(Function(e) e.otrosaliasinstitucion, token_.Value)

                            End If

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.RAZONSOCIAL

                                If definicion_ Is Nothing Then

                                    definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.razonsocialespaniol, token_.Value)

                                Else

                                    definicion_ = definicion_.Set(Function(e) e.razonsocialespaniol, token_.Value)

                                End If

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.ARCHIVADO

                                If definicion_ Is Nothing Then

                                    definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.archivado, token_.Value)

                                Else

                                    definicion_ = definicion_.Set(Function(e) e.archivado, token_.Value)

                                End If

                            Case IControladorInstitucionBancaria.CamposBusquedaSimple.ESTADO

                                If definicion_ Is Nothing Then

                                    definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.estado, token_.Value)

                                Else

                                    definicion_ = definicion_.Set(Function(e) e.estado, token_.Value)

                                End If

                        End Select

                    Next
                Dim result_ As UpdateResult
                If bandera_ Then
                    result_ = operationsDB_.UpdateOne(Function(e) e._id = idBanco, definicion_)
                Else
                    result_ = operationsDB_.UpdateOne(Function(e) e._id = idBanco, definicion_)
                End If


                If result_.ModifiedCount Then

                    .SetOK()

                    .ObjectReturned = True

                Else

                    .SetError()

                    .ObjectReturned = Nothing

                End If

            End Using

        End With

        Return _Estado

    End Function


    Function devuelve4(otrainstitucion As List(Of AliasBancos)) As Integer
        Return 4
    End Function
    Private Function BuscarBancos(ByVal tokens_ As Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object),
                          modalidad_ As IControladorInstitucionBancaria.Modalidades) As List(Of InstitucionBancaria) Implements IControladorInstitucionBancaria.BuscarBancos

        Dim definicion_ As FilterDefinition(Of InstitucionBancaria) = Nothing

        If modalidad_ = Interno Then

        Else

            InstitucionesBancarias = New List(Of InstitucionBancaria)

            For Each token_ As KeyValuePair(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object) In tokens_

                Select Case token_.Key

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDS

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e._id, token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e._id, token_.Value)

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDBANCARIO

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.idinstitucionbancaria, token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.idinstitucionbancaria, token_.Value)

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.NUMIDBANCARIA

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.identificadorbanco, token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.identificadorbanco, token_.Value)

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIAL

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.ElemMatch(Function(e) e.otrosaliasinstitucion, Function(alias_) alias_.Valor.Equals(token_.Value.ToString.ToUpper))

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.ElemMatch(Function(e) e.otrosaliasinstitucion, Function(alias_) alias_.Valor.Equals(token_.Value.ToString.ToUpper))

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.RAZONSOCIAL

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.Regex(Function(e) e.razonsocialespaniol, New BsonRegularExpression(token_.Value, "i"))

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.Regex(Function(e) e.razonsocialespaniol, New BsonRegularExpression(token_.Value, "i"))

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.ARCHIVADO

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.archivado, token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.archivado, token_.Value)

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.ESTADO

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.estado, token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e.estado, token_.Value)

                        End If

                End Select

            Next

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).GetType.Name)

                operationsDB_.Aggregate.Match(definicion_).ToList.ForEach(Sub(resultado_)

                                                                              InstitucionesBancarias.Add(resultado_)
                                                                              'MsgBox(resultado_.razonsocialespaniol)

                                                                          End Sub)
            End Using


        End If

        'Enum CamposBusquedaSimple
        '    SinDefinir = 0
        '    IDS = 1
        '    IDBANCARIO = 2
        '    NUMIDBANCARIA = 3
        '    NOMBRECOMERCIAL = 4
        '    RAZONSOCIAL = 5
        '    DOMICILIOFISCAL = 6
        '    ARCHIVADO = 7
        '    ESTADO = 8
        'End Enum

        Return InstitucionesBancarias

    End Function


    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub


#End Region

End Class
