Imports MongoDB.Bson.Serialization.Attributes
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions
Imports MongoDB.Driver
Imports MongoDB.Bson
Imports gsol.krom

Public Class Secuencia

    <BsonId>
    Property _id As ObjectId
    Property sec As Int32
    Property environment As Int32
    Property anio As Int32
    Property mes As Int32
    Property estado As Boolean = True
    Property nombre As String
    Property tiposecuencia As Int32
    Property subtiposecuencia As Int32
    Property prefijo As String = Nothing
    Property sufijo As String = Nothing


    Sub New()

    End Sub

    Public Function GeneraSecuencia(ByVal nombre_ As String,
                                       Optional ByVal enviroment_ As Int16 = 0,
                                       Optional ByVal anio_ As Int16 = 0,
                                       Optional ByVal mes_ As Int16 = 0,
                                       Optional ByVal tipoSecuencia_ As Integer = 0,
                                       Optional ByVal subTipoSecuencia_ As Integer = 0,
                                       Optional ByVal prefijo As String = Nothing
                                       ) As Int32

        ''* ** * ** Generador de secuencias referencias ** * ** *
        Dim secuencia_ As New Secuencia _
                  With {.nombre = nombre_,
                      .environment = enviroment_,
                      .anio = anio_,
                      .mes = mes_,
                      .tiposecuencia = tipoSecuencia_,
                      .subtiposecuencia = subTipoSecuencia_,
                      .prefijo = prefijo
                      }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select
        ''* ** * ** Generador de seciencias ** * ** *
        Return sec_

    End Function

    Public Async Function Generar() As Task(Of TagWatcher)

        Dim respuesta_ As New TagWatcher

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Secuencia)("Reg000Secuencias")

        Dim filter_ = Builders(Of Secuencia).Filter.Eq(Function(x) x.nombre, Me.nombre) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.tiposecuencia, Me.tiposecuencia)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.subtiposecuencia, Me.subtiposecuencia)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.prefijo, Me.prefijo)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.sufijo, Me.sufijo)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.environment, Me.environment)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.anio, Me.anio)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.mes, Me.mes)) _
                 And (Builders(Of Secuencia).Filter.Eq(Function(x) x.estado, True))

        Dim updateStatements_ = Builders(Of Secuencia).Update.
                                         Inc(Of Int32)(Function(x) x.sec, 1).
                                         Set(Function(x) x.nombre, Me.nombre).
                                         Set(Function(x) x.subtiposecuencia, Me.subtiposecuencia).
                                         Set(Function(x) x.prefijo, Me.prefijo).
                                         Set(Function(x) x.sufijo, Me.sufijo).
                                         Set(Function(x) x.tiposecuencia, Me.tiposecuencia).
                                         Set(Function(x) x.environment, Me.environment).
                                         Set(Function(x) x.anio, Me.anio).
                                         Set(Function(x) x.mes, Me.mes).
                                         Set(Function(x) x.estado, Me.estado)

        Dim opciones_ = New FindOneAndUpdateOptions(Of Secuencia)()

        With opciones_

            .ReturnDocument = ReturnDocument.After

            .Projection = New ProjectionDefinitionBuilder(Of Secuencia)().Include(Function(n) n.sec)

        End With

        Dim result_ = operationsDB_.FindOneAndUpdate(filter_, updateStatements_, opciones_)

        With respuesta_

            If result_ IsNot Nothing Then

                .SetOK()

                .ObjectReturned = result_

            Else

                With opciones_

                    .IsUpsert = True

                End With

                result_ = operationsDB_.FindOneAndUpdate(filter_, updateStatements_, opciones_)

                If result_ IsNot Nothing Then

                    .SetOK()

                    .ObjectReturned = result_

                End If

            End If

        End With

        Return respuesta_

    End Function

End Class