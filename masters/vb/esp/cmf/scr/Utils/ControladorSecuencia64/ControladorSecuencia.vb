Imports System.Text.RegularExpressions
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports Wma.Exceptions

Public Class ControladorSecuencia
    Implements IControladorSecuencia, IDisposable

#Region "Propiedades privadas"

    Private _espacioTrabajo As IEspacioTrabajo

    Private disposedValue As Boolean

    Private _limite As Int32

#End Region

#Region "Propiedades públicas"
    Public Property _Enviroment As Int32 _
        Implements IControladorSecuencia._Enviroment

    Public Property _TipoSecuencia As Int32 _
        Implements IControladorSecuencia._TipoSecuencia

    Public Property _SubtipoSecuencia As Int32 _
        Implements IControladorSecuencia._SubtipoSecuencia
    Public Property _Secuencia As ISecuencia _
        Implements IControladorSecuencia._Secuencia

    Public Property _Estado As TagWatcher _
        Implements IControladorSecuencia._Estado
#End Region

    Sub New()

        _Secuencia = New Secuencia
        _Estado = New TagWatcher
        _limite = 1

    End Sub
    Sub New(ByVal limite_ As Int32)

        _Secuencia = New Secuencia
        _Estado = New TagWatcher
        _limite = limite_

    End Sub

#Region "Métodos privados"

    Private Sub Inicializa(ByVal nombre_ As String,
                           ByVal tipoSecuencia_ As Int32,
                           ByVal compania_ As Int32,
                           ByVal area_ As Int32,
                           Optional ByVal enviroment_ As Int32 = 0,
                           Optional ByVal anio_ As Int32 = 0,
                           Optional ByVal mes_ As Int32 = 0,
                           Optional ByVal subTipoSecuencia_ As Int32 = 0,
                           Optional ByVal prefijo_ As String = Nothing,
                           Optional ByVal sufijo_ As String = Nothing)

        With _Secuencia
            .nombre = nombre_
            .compania = compania_
            .area = area_
            .environment = enviroment_
            .anio = anio_
            .mes = mes_
            .tiposecuencia = tipoSecuencia_
            .subtiposecuencia = subTipoSecuencia_
            .prefijo = prefijo_
            .sufijo = sufijo_
            .estado = 1
            .archivado = True
        End With

    End Sub

    Private Function GenerarSecuencia(ByVal secuencia_ As Secuencia,
                                      Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                      As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Secuencia)("Reg000Secuencias")

            With secuencia_

                Dim filter_ = Builders(Of Secuencia).Filter.Eq(Function(x) x.nombre, .nombre) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.compania, .compania)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.area, .area)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.tiposecuencia, .tiposecuencia)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.subtiposecuencia, .subtiposecuencia)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.prefijo, .prefijo)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.sufijo, .sufijo)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.environment, .environment)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.anio, .anio)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.mes, .mes)) _
                And (Builders(Of Secuencia).Filter.Eq(Function(x) x.estado, .estado))

                Dim updateStatements_ = Builders(Of Secuencia).Update.
                                        Inc(Of Int32)(Function(x) x.sec, _limite).
                                        Set(Function(x) x.nombre, .nombre).
                                        Set(Function(x) x.compania, .compania).
                                        Set(Function(x) x.area, .area).
                                        Set(Function(x) x.subtiposecuencia, .subtiposecuencia).
                                        Set(Function(x) x.prefijo, .prefijo).
                                        Set(Function(x) x.sufijo, .sufijo).
                                        Set(Function(x) x.tiposecuencia, .tiposecuencia).
                                        Set(Function(x) x.environment, .environment).
                                        Set(Function(x) x.anio, .anio).
                                        Set(Function(x) x.mes, .mes).
                                        Set(Function(x) x.estado, .estado)

                Dim opciones_ = New FindOneAndUpdateOptions(Of Secuencia)()

                With opciones_

                    .ReturnDocument = ReturnDocument.After

                    .Projection = New ProjectionDefinitionBuilder(Of Secuencia)().Include(Function(n) n.sec)

                End With

                Dim result_ As Secuencia

                If session_ IsNot Nothing Then

                    result_ = operationsDB_.FindOneAndUpdate(session_, filter_, updateStatements_, opciones_)

                Else

                    result_ = operationsDB_.FindOneAndUpdate(filter_, updateStatements_, opciones_)

                End If

                With _Estado

                    If result_ IsNot Nothing Then

                        result_.secuenciaAnterior = result_.sec - _limite

                        .SetOK()

                        .ObjectReturned = result_

                    Else

                        With opciones_

                            .IsUpsert = True

                        End With

                        result_ = operationsDB_.FindOneAndUpdate(filter_, updateStatements_, opciones_)

                        If result_ IsNot Nothing Then

                            result_.secuenciaAnterior = result_.sec - _limite

                            .SetOK()

                            .ObjectReturned = result_

                        End If

                    End If

                End With

            End With

        End Using

        Return _Estado

    End Function

#End Region

#Region "Métodos públicos"
    Public Function Generar(nombre_ As String,
                            tipoSecuencia_ As Integer,
                            compania_ As Integer,
                            area_ As Integer,
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
                            Implements IControladorSecuencia.Generar

        '''
        ''' Genera una secuencia de documento
        ''' # COMBINACIÓN 1
        ''' nombre_ = Empresas
        ''' tipoSecuencia_ = 1 (Nacional) , 2 (Internacional)
        ''' compania_ = 1 (Krom aduanal), 2 (Krom logistica)
        ''' area_ = 1 (Tráfico aduanal), 2 (Tráfico logística)
        '''
        '''

        With _Estado

            If nombre_ IsNot Nothing Then

                Inicializa(nombre_,
                            tipoSecuencia_,
                            compania_,
                            area_)

                GenerarSecuencia(_Secuencia, session_)

            Else

                .SetError(Me, "Valores no pueden ser nulos")

            End If

        End With

        Return _Estado

    End Function

    Public Function Generar(nombre_ As String,
                            tipoSecuencia_ As Integer,
                            compania_ As Integer,
                            area_ As Integer,
                            subtipoSecuencia_ As Integer,
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
                            Implements IControladorSecuencia.Generar

        '''
        ''' Genera una secuencia de documento
        ''' # COMBINACIÓN 2
        ''' nombre_ = Pedimentos
        ''' tipoSecuencia_ = 1 (PedimentoNormal) , 2 (PedimentoRectificación), 3 (Pedimento complementario), ...
        ''' compania_ = 1 (Krom aduanal), 2 (Krom logistica), ...
        ''' area_ = 1 (Tráfico aduanal), 2 (Tráfico logística), ...
        ''' subtipoSecuencia_ = 430 (Veracruz), 160 (Manzanillo), 510 (Lázaro Cárdenas), ...
        '''

        With _Estado

            If nombre_ IsNot Nothing Then

                Inicializa(nombre_,
                            tipoSecuencia_,
                            compania_,
                            area_,
                            subtipoSecuencia_)

                GenerarSecuencia(_Secuencia, session_)

            Else

                .SetError(Me, "Valores no pueden ser nulos")

            End If

        End With

        Return _Estado

    End Function

    Public Function Generar(nombre_ As String,
                            tipoSecuencia_ As Integer,
                            compania_ As Integer,
                            area_ As Integer,
                            subtipoSecuencia_ As Integer,
                            enviroment_ As Integer,
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
                            Implements IControladorSecuencia.Generar

        '''
        ''' Genera una secuencia de documento
        ''' # COMBINACIÓN 3
        ''' nombre_ = Pedimentos
        ''' tipoSecuencia_ = 1 (PedimentoNormal) , 2 (PedimentoRectificación), 3 (Pedimento complementario), ...
        ''' compania_ = 1 (Krom aduanal), 2 (Krom logistica), ...
        ''' area_ = 1 (Tráfico aduanal), 2 (Tráfico logística), ...
        ''' subtipoSecuencia_ = 430 (Veracruz), 160 (Manzanillo), 510 (Lázaro Cárdenas), ...
        ''' enviroment_ = 1 (Veracruz), 3 (CDMX), 4 (Virtual), 8 (Manzanillo), 6 (Altamira), ...
        '''

        With _Estado

            If nombre_ IsNot Nothing Then

                Inicializa(nombre_,
                            tipoSecuencia_,
                            compania_,
                            area_,
                            subtipoSecuencia_,
                            enviroment_)

                GenerarSecuencia(_Secuencia, session_)

            Else

                .SetError(Me, "Valores no pueden ser nulos")

            End If

        End With

        Return _Estado

    End Function

    Public Function Generar(nombre_ As String,
                            tipoSecuencia_ As Integer,
                            compania_ As Integer,
                            area_ As Integer,
                            subtipoSecuencia_ As Integer,
                            prefijo_ As String,
                            sufijo_ As String,
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
                            Implements IControladorSecuencia.Generar


        '''
        ''' Genera una secuencia de documento
        ''' # COMBINACIÓN 4
        ''' nombre_ = Pedimentos
        ''' tipoSecuencia_ = 1 (PedimentoNormal) , 2 (PedimentoRectificación), 3 (Pedimento complementario), ...
        ''' compania_ = 1 (Krom aduanal), 2 (Krom logistica), ...
        ''' area_ = 1 (Tráfico aduanal), 2 (Tráfico logística), ...
        ''' subtipoSecuencia_ = 430 (Veracruz), 160 (Manzanillo), 510 (Lázaro Cárdenas), ...
        ''' prefijo_ = RKU, ...
        ''' sufijo_ =  ...
        '''

        With _Estado

            If nombre_ IsNot Nothing Then

                Inicializa(nombre_,
                            tipoSecuencia_,
                            compania_,
                            area_,
                            subtipoSecuencia_,
                            prefijo_,
                            sufijo_)

                GenerarSecuencia(_Secuencia, session_)

            Else

                .SetError(Me, "Valores no pueden ser nulos")

            End If

        End With

        Return _Estado

    End Function

    Public Function Generar(secuencia_ As ISecuencia,
                            Optional session_ As IClientSessionHandle = Nothing) _
                            As TagWatcher _
                            Implements IControladorSecuencia.Generar

        '''
        ''' Genera una secuencia de documento
        ''' # COMBINACIÓN 5
        ''' secuencia_ = Estructura de secuencia
        '''


        With _Estado

            If secuencia_ IsNot Nothing Then

                GenerarSecuencia(secuencia_, session_)

            Else

                .SetError(Me, "Estructura secuencia no existe")

            End If

        End With

        Return _Estado

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)

                _espacioTrabajo = Nothing

                '_Secuencia = Nothing

                '_Estado = Nothing

                disposedValue = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True
        End If
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose

        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)

    End Sub

#End Region
End Class
