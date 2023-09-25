Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones

Namespace Gsol.BaseDatos

    Public Class PoliticasBaseDatos
        Implements IPoliticasBaseDatos

#Region "Enum"

#End Region

#Region "Atributos"

        Private _iespacioTrabajo As IEspacioTrabajo

        Private _permiso As Integer

        Private _politica As Integer

        Private _tagWatcher As TagWatcher

        Private _nombrePolitica As String

        Private _numeroPolitica As String

        Private _ioperacionesPolitica As IOperacionesCatalogo

        Private _sistema As Organismo

        Private _consulta As String

        Private _parametros As String

        Private _sql As String

        Private _ObjetoRepositorio As Object

        Private _tipoProcedimiento As IPoliticasBaseDatos.TiposProcedimientos

        Private _informacionFaltante As List(Of String)

        Private _verificaCampos As IPoliticasBaseDatos.VerificarCampos

#End Region

#Region "Propiedades"

        Public ReadOnly Property GetParametrosPolitica As String Implements IPoliticasBaseDatos.GetParametrosPolitica
            Get

                If _parametros Is Nothing Then

                    _parametros = ""

                End If

                Return _parametros

            End Get
        End Property

        Public ReadOnly Property GetNombrePolitica As String _
            Implements IPoliticasBaseDatos.GetNombrePolitica

            Get
                If _nombrePolitica Is Nothing Then

                    _nombrePolitica = ""

                End If

                Return _nombrePolitica

            End Get

        End Property

        Public ReadOnly Property GetNumeroPolitica As String Implements IPoliticasBaseDatos.GetNumeroPolitica

            Get

                If _nombrePolitica Is Nothing Then

                    _numeroPolitica = ""

                End If

                Return _numeroPolitica

            End Get

        End Property

        Public ReadOnly Property GetPermiso As Integer Implements IPoliticasBaseDatos.GetPermiso

            Get

                Return _permiso

            End Get

        End Property

        Public ReadOnly Property GetTagWatcher As Wma.Exceptions.TagWatcher Implements IPoliticasBaseDatos.GetTagWatcher

            Get

                Return _tagWatcher

            End Get

        End Property

        Public Property IOperacionesCatalogo As IOperacionesCatalogo Implements IPoliticasBaseDatos.IOperacionesCatalogo

            Get

                Return _ioperacionesPolitica

            End Get

            Set(ByVal value As IOperacionesCatalogo)

                _ioperacionesPolitica = value

            End Set

        End Property

        Public WriteOnly Property SetPermiso As Integer _
            Implements IPoliticasBaseDatos.SetPermiso

            Set(ByVal value As Integer)

                _permiso = value

            End Set

        End Property

        Public ReadOnly Property ObjetoRepositorio As String Implements IPoliticasBaseDatos.ObjetoRepositorio

            Get

                Return _ObjetoRepositorio

            End Get

        End Property

        Public Property TipoProcedimiento As IPoliticasBaseDatos.TiposProcedimientos Implements IPoliticasBaseDatos.TipoProcedimiento

            Get

                Return _tipoProcedimiento

            End Get

            Set(ByVal value As IPoliticasBaseDatos.TiposProcedimientos)

                _tipoProcedimiento = value

            End Set

        End Property

        Public ReadOnly Property InformacionFaltante As List(Of String)

            Get

                Return _informacionFaltante

            End Get

        End Property

        Public Property VerificaCampos As IPoliticasBaseDatos.VerificarCampos Implements IPoliticasBaseDatos.VerificaCampos

            Get

                Return _verificaCampos

            End Get

            Set(ByVal value As IPoliticasBaseDatos.VerificarCampos)

                _verificaCampos = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New(Optional ByVal tipoProcedimiento_ As IPoliticasBaseDatos.TiposProcedimientos = IPoliticasBaseDatos.TiposProcedimientos.Funcion)

            _informacionFaltante = New List(Of String)

            _tagWatcher = New TagWatcher

            _iespacioTrabajo = New EspacioTrabajo

            _tipoProcedimiento = tipoProcedimiento_

            _verificaCampos = IPoliticasBaseDatos.VerificarCampos.No

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo,
                ByVal politica_ As Integer)

            _informacionFaltante = New List(Of String)

            _tipoProcedimiento = IPoliticasBaseDatos.TiposProcedimientos.Funcion

            _verificaCampos = IPoliticasBaseDatos.VerificarCampos.No

            _politica = politica_

            _ioperacionesPolitica = ioperaciones_

            _sistema = New Organismo

            _tagWatcher = New TagWatcher

            _ObjetoRepositorio = New Object

            RealizarConIOperaciones(ioperaciones_)

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo,
                ByVal politica_ As Integer,
                ByVal TipoProcedimiento_ As IPoliticasBaseDatos.TiposProcedimientos,
                Optional ByVal verificaCampos_ As IPoliticasBaseDatos.VerificarCampos = IPoliticasBaseDatos.VerificarCampos.No)

            _informacionFaltante = New List(Of String)

            _tipoProcedimiento = TipoProcedimiento_

            _verificaCampos = verificaCampos_

            _politica = politica_

            _ioperacionesPolitica = ioperaciones_

            _sistema = New Organismo

            _tagWatcher = New TagWatcher

            _ObjetoRepositorio = New Object

            RealizarConIOperaciones(ioperaciones_)

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo,
                ByVal politica_ As Integer,
                ByVal caracteristicas_ As List(Of CaracteristicaCatalogo))

            _informacionFaltante = New List(Of String)

            _politica = politica_

            _ioperacionesPolitica = ioperaciones_

            _sistema = New Organismo

            _tagWatcher = New TagWatcher

            _ObjetoRepositorio = New Object

            _tipoProcedimiento = IPoliticasBaseDatos.TiposProcedimientos.Funcion

            _verificaCampos = IPoliticasBaseDatos.VerificarCampos.No

            If caracteristicas_.Count > 1 Or Not caracteristicas_ Is Nothing Then

                RealizarConCaracteristicas(ioperaciones_, caracteristicas_)

            Else

                RealizarConIOperaciones(ioperaciones_)

            End If

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo,
               ByVal politica_ As Integer,
               ByVal caracteristicas_ As List(Of CaracteristicaCatalogo),
               ByVal TipoProcedimiento_ As IPoliticasBaseDatos.TiposProcedimientos,
               Optional ByVal verificaCampos_ As IPoliticasBaseDatos.VerificarCampos = IPoliticasBaseDatos.VerificarCampos.No)

            _informacionFaltante = New List(Of String)

            _tipoProcedimiento = TipoProcedimiento_

            _verificaCampos = verificaCampos_

            _politica = politica_

            _ioperacionesPolitica = ioperaciones_

            _sistema = New Organismo

            _tagWatcher = New TagWatcher

            _ObjetoRepositorio = New Object

            If caracteristicas_.Count > 1 Or Not caracteristicas_ Is Nothing Then

                RealizarConCaracteristicas(ioperaciones_, caracteristicas_)

            Else

                RealizarConIOperaciones(ioperaciones_)

            End If

        End Sub

#End Region

#Region "Metodos"

        Private Sub RealizarConIOperaciones(ByVal ioperaciones_ As OperacionesCatalogo)

            _tagWatcher.Status = TagWatcher.TypeStatus.Ok

            _ioperacionesPolitica = _sistema.ConsultaModulo(_ioperacionesPolitica.EspacioTrabajo,
                                                             "PoliticasCorporativas",
                                                             " and i_cve_politica = " & _politica)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''VERIFICAMOS QUE EXISTA ESA POLITICA''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Not _ioperacionesPolitica Is Nothing Then

                If _sistema.TieneResultados(_ioperacionesPolitica) Then

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''OBTENEMOS LA CANTIDAD DE POLITICAS''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''Y ASIGNAMOS LOS VALORES DE LA POLITICA''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Dim cantidadColumnas_ = ioperaciones_.Caracteristicas.Count

                    _consulta = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Nombre Consulta").ToString

                    _parametros = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Parámetros").ToString

                    _numeroPolitica = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Número Política").ToString

                    _nombrePolitica = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Nombre Política").ToString

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''PARTIMOS LOS PARAMETROS DE LA POLITICA''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''PARA ARMAR EL QUERY Y VEMOS QUE EXISTAN TODOS LOS PARAMETROS'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Dim parametrosArray_ As String() = _parametros.Split(",")

                    Dim encontrados_ = 0

                    For counter2 As Integer = 0 To cantidadColumnas_ - 1

                        For counter3 As Integer = 0 To parametrosArray_.Length - 1

                            Dim aux = parametrosArray_(counter3).ToString

                            Dim aux2 = ioperaciones_.Caracteristicas(counter2).Nombre

                            If ioperaciones_.Caracteristicas(counter2).Nombre = parametrosArray_(counter3).ToString Then

                                encontrados_ = encontrados_ + 1

                            End If

                        Next

                    Next

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''VERIFICAMOS QUE LOS ENCONTRADOS CORRESPONDAN A TODOS LOS PARAMETROS'''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If encontrados_ = parametrosArray_.Length Then

                        If _tipoProcedimiento = IPoliticasBaseDatos.TiposProcedimientos.Funcion Then

                            _sql = "select *  from  " & _consulta & " ("

                            If parametrosArray_.Length = 1 Then

                                _sql = _sql & "'" & ioperaciones_.CampoPorNombre(parametrosArray_(0)) & "'"

                            Else

                                For counter2 As Integer = 0 To parametrosArray_.Length - 1

                                    If counter2 = parametrosArray_.Length - 1 Then

                                        _sql = _sql & "'" & ioperaciones_.CampoPorNombre(parametrosArray_(counter2)) & "'"

                                    Else

                                        _sql = _sql & "'" & ioperaciones_.CampoPorNombre(parametrosArray_(counter2)) & "',"

                                    End If

                                Next


                            End If

                            _sql = _sql & ")"

                        ElseIf _tipoProcedimiento = IPoliticasBaseDatos.TiposProcedimientos.ProcedimientoAlmacenado Then

                            _sql = " exec " & _consulta & " "

                            If parametrosArray_.Length = 1 Then

                                _sql = _sql & "'" & ioperaciones_.CampoPorNombre(parametrosArray_(0)) & "'"

                            Else

                                For counter2 As Integer = 0 To parametrosArray_.Length - 1

                                    If counter2 = parametrosArray_.Length - 1 Then

                                        _sql = _sql & "'" & ioperaciones_.CampoPorNombre(parametrosArray_(counter2)) & "'"

                                    Else

                                        _sql = _sql & "'" & ioperaciones_.CampoPorNombre(parametrosArray_(counter2)) & "',"

                                    End If

                                Next


                            End If

                        End If


                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ''''''''''''''''''''''''''''''''''''''''''''''Ejecuta consulta y verifica que tenga valores'''''''''''''''''''''''''''''''''''''''''''''''''
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        _tagWatcher = _sistema.ComandosSingletonSQL(_sql)

                        If _tagWatcher.Status = TagWatcher.TypeStatus.Ok Then

                            Dim cursorDataTable_ As New DataTable

                            cursorDataTable_ = DirectCast(_tagWatcher.ObjectReturned, DataTable)

                            _ObjetoRepositorio = _tagWatcher.ObjectReturned

                            If _verificaCampos = IPoliticasBaseDatos.VerificarCampos.Si Then


                                For Each col_ As DataColumn In cursorDataTable_.Columns

                                    If col_.ColumnName <> "i_Error" Then

                                        If IsDBNull(cursorDataTable_.Rows(0)(col_.ColumnName)) Then

                                            _informacionFaltante.Add(col_.ColumnName)

                                        End If

                                    End If

                                Next

                                If _informacionFaltante.Count > 0 Then

                                    _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                                    _tagWatcher.SetError(TagWatcher.ErrorTypes.C6_010_7018)

                                End If

                                Exit Sub

                            End If

                            If cursorDataTable_.Rows.Count = 0 Then

                                _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                                _tagWatcher.SetError(TagWatcher.ErrorTypes.C6_010_7007)

                            Else

                                If Not VerificarDBNull(cursorDataTable_(0)(0)) Is Nothing Then

                                    _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                                    _tagWatcher.SetError(cursorDataTable_(0)(0))

                                End If

                            End If

                        End If

                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ''''''''''''''''''''''''''''''''''''''''''''''EN CASO DE QUE EXISTAN PARAMETROS INCORRECTOS''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Else

                        _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                        _tagWatcher.Errors = TagWatcher.ErrorTypes.C6_010_7003

                    End If

                Else
                    _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                    _tagWatcher.Errors = TagWatcher.ErrorTypes.C6_010_7004

                End If

            Else

                _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                _tagWatcher.Errors = TagWatcher.ErrorTypes.C6_010_7004

            End If

        End Sub

        Private Sub RealizarConCaracteristicas(ByVal ioperaciones_ As OperacionesCatalogo,
                                       ByVal caracteristicas_ As List(Of CaracteristicaCatalogo))

            _tagWatcher.Status = TagWatcher.TypeStatus.Ok

            _ioperacionesPolitica = _sistema.ConsultaModulo(_ioperacionesPolitica.EspacioTrabajo,
                                                            "PoliticasCorporativas",
                                                            " and i_cve_politica = " & _politica)

            ' _ioperacionesPolitica = CType(Me._sistema.ConsultaModulo(_ioperacionesPolitica.EspacioTrabajo, "PoliticasCorporativas", " and i_cve_permiso = " & permiso_).Clone(), IOperacionesCatalogo)


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''VERIFICAMOS QUE EXISTAN POLITICAS ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Not _ioperacionesPolitica Is Nothing Then

                If _sistema.TieneResultados(_ioperacionesPolitica) Then

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''OBTENEMOS LA CANTIDAD DE POLITICAS''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''Y ASIGNAMOS LOS VALORES DE LA POLITICA''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Dim cantidadColumnas_ = caracteristicas_.Count

                    _consulta = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Nombre Consulta").ToString

                    _parametros = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Parámetros").ToString

                    _numeroPolitica = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Número Política").ToString

                    _nombrePolitica = _ioperacionesPolitica.Vista.Tables(0).Rows(0)("Nombre Política").ToString

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''PARTIMOS LOS PARAMETROS DE LA POLITICA''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''PARA ARMAR EL QUERY Y VEMOS QUE EXISTAN TODOS LOS PARAMETROS'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Dim parametrosArray_ As String() = _parametros.Split(",")

                    Dim encontrados_ = 0

                    For counter2 As Integer = 0 To cantidadColumnas_ - 1

                        For counter3 As Integer = 0 To parametrosArray_.Length - 1

                            Dim aux = parametrosArray_(counter3).ToString

                            Dim aux2 = caracteristicas_(counter2).Nombre

                            If caracteristicas_(counter2).Nombre = parametrosArray_(counter3).ToString Then

                                encontrados_ = encontrados_ + 1

                            End If

                        Next

                    Next

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''VERIFICAMOS QUE LOS ENCONTRADOS CORRESPONDAN A TODOS LOS PARAMETROS'''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If encontrados_ = parametrosArray_.Length Then

                        If _tipoProcedimiento = IPoliticasBaseDatos.TiposProcedimientos.Funcion Then

                            _sql = "select *  from  " & _consulta & " ("

                            If parametrosArray_.Length = 1 Then

                                _sql = _sql & "'" & (caracteristicas_(0).ValorAsignado) & "'"

                            Else

                                For counter2 As Integer = 0 To parametrosArray_.Length - 1

                                    Dim valor_ = Nothing

                                    For i As Integer = 0 To parametrosArray_.Length - 1

                                        If parametrosArray_(counter2) = caracteristicas_(i).Nombre Then

                                            valor_ = caracteristicas_(i).ValorAsignado

                                        End If

                                    Next

                                    If counter2 = parametrosArray_.Length - 1 Then

                                        _sql = _sql & "'" & valor_ & "'"

                                    Else

                                        _sql = _sql & "'" & valor_ & "',"

                                    End If

                                Next


                            End If

                            _sql = _sql & ")"


                        ElseIf _tipoProcedimiento = IPoliticasBaseDatos.TiposProcedimientos.ProcedimientoAlmacenado Then

                            _sql = " exec " & _consulta & " "

                            If parametrosArray_.Length = 1 Then

                                _sql = _sql & "'" & (caracteristicas_(0).ValorAsignado) & "'"

                            Else

                                For counter2 As Integer = 0 To parametrosArray_.Length - 1

                                    Dim valor_ = Nothing

                                    For i As Integer = 0 To parametrosArray_.Length - 1

                                        If parametrosArray_(counter2) = caracteristicas_(i).Nombre Then

                                            valor_ = caracteristicas_(i).ValorAsignado

                                        End If

                                    Next

                                    If counter2 = parametrosArray_.Length - 1 Then

                                        _sql = _sql & "'" & valor_ & "'"

                                    Else

                                        _sql = _sql & "'" & valor_ & "',"

                                    End If

                                Next

                            End If

                        End If


                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ''''''''''''''''''''''''''''''''''''''''''''''Ejecuta consulta y verifica que tenga valores'''''''''''''''''''''''''''''''''''''''''''''''''
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        _tagWatcher = _sistema.ComandosSingletonSQL(_sql)

                        If _tagWatcher.Status = TagWatcher.TypeStatus.Ok Then

                            Dim cursorDataTable_ As New DataTable

                            cursorDataTable_ = DirectCast(_tagWatcher.ObjectReturned, DataTable)

                            _ObjetoRepositorio = _tagWatcher.ObjectReturned

                            If Not IsNothing(cursorDataTable_) Then

                                If _verificaCampos = IPoliticasBaseDatos.VerificarCampos.Si Then

                                    For Each col_ As DataColumn In cursorDataTable_.Columns

                                        If col_.ColumnName <> "i_Error" Then

                                            If IsDBNull(cursorDataTable_.Rows(0)(col_.ColumnName)) Then

                                                _informacionFaltante.Add(col_.ColumnName)

                                            End If

                                        End If

                                    Next

                                    If _informacionFaltante.Count > 0 Then

                                        _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                                        _tagWatcher.SetError(TagWatcher.ErrorTypes.C6_010_7018)

                                    End If

                                    Exit Sub

                                End If

                                If cursorDataTable_.Rows.Count = 0 Then

                                    _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                                    _tagWatcher.SetError(TagWatcher.ErrorTypes.C6_010_7007)

                                Else

                                    If Not VerificarDBNull(cursorDataTable_(0)(0)) Is Nothing Then

                                        _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                                        _tagWatcher.SetError(cursorDataTable_(0)(0))

                                    End If

                                End If

                            End If

                        End If

                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ''''''''''''''''''''''''''''''''''''''''''''''EN CASO DE QUE EXISTAN PARAMETROS INCORRECTOS''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Else

                        _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                        _tagWatcher.Errors = TagWatcher.ErrorTypes.C6_010_7003

                    End If


                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''EN CASO DE QUE NO TENGA POLITICAS'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Else
                    _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                    _tagWatcher.Errors = TagWatcher.ErrorTypes.C6_010_7004

                End If

            Else

                _tagWatcher.Status = TagWatcher.TypeStatus.Errors

                _tagWatcher.Errors = TagWatcher.ErrorTypes.C6_010_7004

            End If

        End Sub

        Private Function VerificarDBNull(ByVal campo_ As Object) As String

            If campo_ Is DBNull.Value Then

                Return Nothing

            End If

            Return campo_

        End Function

#End Region

    End Class

End Namespace
