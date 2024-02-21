Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Namespace gsol.seguridad

    Public Class Cifrado256
        Implements ICifrado


#Region "Atributos"

        Private _cadena As String

        Private _llavecifrado As String

        Private _metodocifrado As SymmetricAlgorithm

#End Region

#Region "Constructores"

        Sub New()

            _cadena = Nothing

            _llavecifrado = Nothing

            _metodocifrado = Nothing

        End Sub


        Sub New(ByVal cadena_ As String, _
                ByVal metodocifrado_ As SymmetricAlgorithm, _
                ByVal llavecifrado_ As String)

            _cadena = cadena_

            _metodocifrado = metodocifrado_

            _llavecifrado = llavecifrado_

        End Sub

#End Region

#Region "Propiedades"

        Public Property Cadena As String _
    Implements ICifrado.Cadena

            Get

                Return _cadena

            End Get

            Set(value As String)

                _cadena = value

            End Set

        End Property

#End Region

#Region "Metodos"


        Public Function LlaveAccesoAutomatica(ByVal tipoComporamientoCodigo_ As ICifrado.TipoComportamientoCodigo) As String _
            Implements ICifrado.LlaveAccesoAutomatica

            Select Case tipoComporamientoCodigo_
                Case ICifrado.TipoComportamientoCodigo.MelekDay

                    Dim primerCaracter_ As String =
                        Now.DayOfWeek  ' día de la semana, numero natural sin ceros a la izquierda = 6
                    Dim segundoCaracter_ As String =
                        UCase(Mid(Now.ToString("dddd"), Now.ToString("dddd").Length - 1, 2))  ' dos últimos ´digitos del día, mayuscula ( lunES)  = DO
                    Dim tercerCaracter_ As String =
                        (Now.DayOfWeek * Now.Day).ToString ' día de la semana multiplicado por día del mes = 6*5 = 30
                    Dim cuartoCaracter_ As String =
                        LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) ' ultima letra minuscula del mes ej. Mayo, = o
                    Dim quintoCaracter_ As String =
                        Now.DayOfWeek ' ultima letra minuscula del mes ej. Mayo, = o
                    Dim sextoCaracter_ As String =
                        LCase(Mid(Now.ToString("dddd"), 1, 1)) & LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) 'primera letra del dia y penultima letra del mes minusculas ej. Sábado y Mayo = so

                    primerCaracter_ = Now.Day.ToString

                    Return primerCaracter_ &
                           segundoCaracter_ &
                           tercerCaracter_ &
                           cuartoCaracter_ &
                           quintoCaracter_ &
                           sextoCaracter_

                Case ICifrado.TipoComportamientoCodigo.MelekDaySolicitudPagoReferencias

                    'Dim primerCaracter_ As String =
                    '    Now.DayOfWeek  ' día de la semana, numero natural sin ceros a la izquierda = 6
                    'Dim segundoCaracter_ As String =
                    '    UCase(Mid(Now.ToString("dddd"), Now.ToString("dddd").Length - 1, 2))  ' dos últimos ´digitos del día, mayuscula ( lunES)  = DO
                    'Dim tercerCaracter_ As String =
                    '    (Now.DayOfWeek * Now.Day).ToString ' día de la semana multiplicado por día del mes = 6*5 = 30
                    'Dim cuartoCaracter_ As String =
                    '    LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) ' ultima letra minuscula del mes ej. Mayo, = o
                    'Dim quintoCaracter_ As String =
                    '    Now.DayOfWeek ' ultima letra minuscula del mes ej. Mayo, = o
                    'Dim sextoCaracter_ As String =
                    '    LCase(Mid(Now.ToString("dddd"), 1, 1)) & LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) 'primera letra del dia y penultima letra del mes minusculas ej. Sábado y Mayo = so

                    'primerCaracter_ = Now.Day.ToString

                    'Return primerCaracter_ &
                    '       segundoCaracter_ &
                    '       tercerCaracter_ &
                    '       cuartoCaracter_ &
                    '       quintoCaracter_ &
                    '       sextoCaracter_

                    Dim primerCaracter_ As String = Now.DayOfWeek & "0"

                    Dim segundoCaracter_ As String = If(Now.Day < 10, "0" & Now.Day, Now.Day)

                    Dim tercerCaracter_ As String = If(Now.Month < 10, "0" & Now.Month, Now.Month)

                    Dim cuartoCaracter_ As String = Now.DayOfYear

                    Dim quintoCaracter_ As String = (Now.Month - Math.Truncate(Now.Month / 2)) * Now.Day

                    Return primerCaracter_ & segundoCaracter_ & tercerCaracter_ & cuartoCaracter_ & quintoCaracter_


                Case ICifrado.TipoComportamientoCodigo.MelekDayFacturacion

                    Dim primerCaracter_ As String = "0" & Now.DayOfWeek

                    Dim segundoCaracter_ As String = If(Now.Day < 10, "0" & Now.Day, Now.Day)

                    Dim tercerCaracter_ As String = If(Now.Month < 10, "0" & Now.Month, Now.Month)

                    Dim cuartoCaracter_ As String = Now.DayOfWeek & "0"

                    Dim quintoCaracter_ As String = (Now.Month - (Now.Month / 2)) * Now.Month

                    Return primerCaracter_ & segundoCaracter_ & tercerCaracter_ & cuartoCaracter_ & quintoCaracter_

                Case ICifrado.TipoComportamientoCodigo.MelekHour
                    Return "notimplemented"

                Case Else
                    Return "notfound"

            End Select

        End Function

        Function LeelaveAccesoAutomatica(ByVal comportamiento_ As ICifrado.TipoComportamientoCodigo) As String 'Implements ICifrado.LlaveAccesoAutomatica


            Dim primerCaracter_ As String = Now.DayOfWeek  ' día de la semana, numero natural sin ceros a la izquierda = 6
            Dim segundoCaracter_ As String = UCase(Mid(Now.ToString("dddd"), Now.ToString("dddd").Length - 1, 2))  ' dos últimos ´digitos del día, mayuscula ( lunES)  = DO
            Dim tercerCaracter_ As String = (Now.DayOfWeek * Now.Day).ToString ' día de la semana multiplicado por día del mes = 6*5 = 30
            Dim cuartoCaracter_ As String = LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) ' ultima letra minuscula del mes ej. Mayo, = o
            Dim quintoCaracter_ As String = Now.DayOfWeek ' ultima letra minuscula del mes ej. Mayo, = o
            Dim sextoCaracter_ As String = LCase(Mid(Now.ToString("dddd"), 1, 1)) & LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) 'primera letra del dia y penultima letra del mes minusculas ej. Sábado y Mayo = so

            'ej. password= 5ES10090oso

            'MelekDay
            'MelekHour
            primerCaracter_ = Now.Day.ToString

            Return primerCaracter_ &
                   segundoCaracter_ &
                   tercerCaracter_ &
                   cuartoCaracter_ &
                   quintoCaracter_ &
                   sextoCaracter_

        End Function

        Function CifraCadena(ByVal cadena_ As String,
                             ByVal metodo_ As ICifrado.Metodos) As String Implements ICifrado.CifraCadena
            Dim strResult_ As String = ""

            Select Case metodo_

                Case ICifrado.Metodos.SHA1

                    Dim sha1Obj_ As New Security.Cryptography.SHA1CryptoServiceProvider

                    Dim bytesToHash_() As Byte = System.Text.Encoding.ASCII.GetBytes(cadena_)

                    bytesToHash_ = sha1Obj_.ComputeHash(bytesToHash_)



                    For Each b As Byte In bytesToHash_

                        strResult_ += b.ToString("x2")

                    Next

                Case ICifrado.Metodos.AES

                    Using myAes As Aes = Aes.Create()

                        Dim encrypted_ As Byte() = Cifrar_Aes(cadena_, myAes.Key, myAes.IV)

                        strResult_ = Convert.ToBase64String(encrypted_)
                        'Dim roundtrip As String = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV)
                        'Dim newBytes As Byte() = Convert.FromBase64String(s)
                    End Using

                Case Else

                    'Sin implementar

            End Select

            Return strResult_

        End Function

        Public Function CifraCadena(cadena_ As String,
                                     metodocifrado_ As SymmetricAlgorithm,
                                     Optional llavecifrado_ As String = Nothing) As String _
                                     Implements ICifrado.CifraCadena

            metodocifrado_.Key = Convert.FromBase64String(llavecifrado_)

            metodocifrado_.Mode = CipherMode.ECB

            Dim encriptado_ As ICryptoTransform = metodocifrado_.CreateEncryptor()

            Dim datos_() As Byte = Encoding.Unicode.GetBytes(cadena_)

            Dim datosEncriptados_() As Byte = encriptado_.TransformFinalBlock(datos_, 0, datos_.Length)

            _cadena = Convert.ToBase64String(datosEncriptados_)

            Return _cadena

        End Function


        Public Function DescifraCadena(cadena_ As String,
                                       metodocifrado_ As SymmetricAlgorithm,
                                       llavecifrado_ As String) As String _
                                   Implements ICifrado.DescifraCadena



            '-----------Bloque para crear archivos encriptados -------------------

            'MsgBox("goooool yaa!")
            ''gruposolium.no -ip.org
            ''S0l1umF0rW
            ''"<config _DireccionIPServidorSQLServerGeneralProduccion=" & Chr(34) & "187.157.188.149" & Chr(34) & ">" & _
            ''     "<config _DireccionIPServidorSQLServerGeneralProduccion=" & Chr(34) & "192.168.1.76" & Chr(34) & ">" & _
            '187.191.84.18
            '             200.79.37.119
            '            usr:sa
            '            S0l1umF0rW
            '            usr:krombase
            '            K1s45gri

            '            gruposolium5.ddns.net()
            '14533:
            'localhost
            '10.66.2.102
            '10.66.100.5
            'sa
            'S0l1umF0rW
            '            "<config _DireccionIPServidorSQLServerGeneralProduccion=" & Chr(34) & "gruposolium5.ddns.net,14533" & Chr(34) & ">" & _
            '            "<config _DireccionIPServidorSQLServerGeneralProduccion=" & Chr(34) & "10.66.100.5,14533" & Chr(34) & ">" & _

            '_BaseDatosSQLServerProduccion

            '"<config _ActivarBitacoraGeneral=" & Chr(34) & "0" & Chr(34) & ">" & _    <---0 es apagada la avanzada...
            '"<config _ControlarBitacoraDesdeLlaveCentral=" & Chr(34) & "0" & Chr(34) & ">" & _ <---0 es apagada la dependencia central

            Dim cadena2_ As String =
                    "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "utf-8" & Chr(34) & "?>" &
                    "<Configuracion>" &
                    "<!-- " &
                    "Configuración Base" &
                    "-->" &
                    "<config _ModalidadServicio=" & Chr(34) & "Puente" & Chr(34) & ">" &
                    "</config>" &
                    "<config _DireccionIPServidorSQLServerGeneralProduccion=" & Chr(34) & "localhost" & Chr(34) & ">" &
                    "</config>" &
                    "<config _BaseDatosSQLServerProduccion=" & Chr(34) & "Solium" & Chr(34) & ">" &
                    "</config>" &
                    "<config _UsuarioSQLServerGeneralProduccion=" & Chr(34) & "sa" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ClaveSQLServerGeneralProduccion=" & Chr(34) & "S0l1umF0rW" & Chr(34) & ">" &
                    "</config>" &
                    "<config _B1DireccionIPSQLServer=" & Chr(34) & "localhost" & Chr(34) & ">" &
                    "</config>" &
                    "<config _B1NombreSQLServer=" & Chr(34) & "KBSBIT" & Chr(34) & ">" &
                    "</config>" &
                    "<config _B1UsuarioSQLServer=" & Chr(34) & "sa" & Chr(34) & ">" &
                    "</config>" &
                    "<config _B1PasswordSQLServer=" & Chr(34) & "S0l1umF0rW" & Chr(34) & ">" &
                    "</config>" &
                    "<config _BMDDireccionIPMongoDB=" & Chr(34) & "10.66.1.16:27017" & Chr(34) & ">" & 'Ip mongodb
                    "</config>" &
                    "<config _BMDNombreMongoDB=" & Chr(34) & "SynapsisC0001" & Chr(34) & ">" & 'Base de datos mongodb
                    "</config>" &
                    "<config _BMDUsuarioMongoDB=" & Chr(34) & "Admin" & Chr(34) & ">" & 'Usuario mongodb
                    "</config>" &
                    "<config _BMDPasswordMongoDB=" & Chr(34) & "12345" & Chr(34) & ">" & 'Password mongodb
                    "</config>" &
                    "<config _BDIDireccionIPSQLServer=" & Chr(34) & "localhost" & Chr(34) & ">" &
                    "</config>" &
                    "<config _BDINombreSQLServer=" & Chr(34) & "KBSDIM" & Chr(34) & ">" &
                    "</config>" &
                    "<config _BDIUsuarioSQLServer=" & Chr(34) & "sa" & Chr(34) & ">" &
                    "</config>" &
                    "<config _BDIPasswordSQLServer=" & Chr(34) & "S0l1umF0rW" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ControlarBitacoraDesdeLlaveCentral=" & Chr(34) & "1" & Chr(34) & ">" &
                    "</config>" &
                    "<config _AplicacionInicial=" & Chr(34) & "4" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ActivarLogGeneral=" & Chr(34) & "0" & Chr(34) & ">" &
                    "</config>" &
                    "<config _RutaLogGeneral=" & Chr(34) & "c:\logs\" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ActivarLogAdministrativo=" & Chr(34) & "0" & Chr(34) & ">" &
                    "</config>" &
                    "<config _RutaLogAdministrativo=" & Chr(34) & "C:\TransaccionesContables\" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ActivarBitacoraBasica=" & Chr(34) & "0" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ActivarBitacoraGeneral=" & Chr(34) & "0" & Chr(34) & ">" &
                    "</config>" &
                    "<config _Oficina=" & Chr(34) & "NA" & Chr(34) & ">" &
                    "</config>" &
                    "<config _DireccionIPServidorMYSQLGeneralProduccion=" & Chr(34) & "localhost" & Chr(34) & ">" &
                    "</config>" &
                    "<config _UsuarioMYSQLGeneralProduccion=" & Chr(34) & "EXTRANET" & Chr(34) & ">" &
                    "</config>" &
                    "<config _ClaveMYSQLGeneralProduccion=" & Chr(34) & "654321" & Chr(34) & ">" &
                    "</config>" &
                    "<config _BaseDatosMYSQLProduccion=" & Chr(34) & "rku_extranet" & Chr(34) & ">" &
                    "</config>" &
                    "<config _DireccionServidorVFPGeneralProduccion=" & Chr(34) & "z:\RECO\SAAI\dbfs\" & Chr(34) & ">" &
                    "</config>" &
                    "<sesion _Usuario=" & Chr(34) & "Aesquivel" & Chr(34) & ">" &
                    "</sesion>" &
                    "<sesion _Contrasena=" & Chr(34) & "AmaiaEsquivel123" & Chr(34) & ">" &
                    "</sesion>" &
                    "<sesion _GrupoEmpresarial=" & Chr(34) & "1" & Chr(34) & ">" &
                    "</sesion>" &
                    "<sesion _DivisionEmpresarial=" & Chr(34) & "1" & Chr(34) & ">" &
                    "</sesion>" &
                    "<sesion _Aplicacion=" & Chr(34) & "3" & Chr(34) & ">" &
                    "</sesion>" &
                    "<webservices _EndPoint=" & Chr(34) & "#" & Chr(34) & ">" &
                    "</webservices>" &
                    "<webservices _ContrasenaWebService=" & Chr(34) & "" & Chr(34) & ">" &
                    "</webservices>" &
                    "<config _LicenciaTagCode=" & Chr(34) & "WS19Fld3+b/JM72J+FuQaYLeB4iCUQDAklIEvRDprQtQTBKQtkQj4Q==" & Chr(34) & "> " &
                    "</config>" &
                    "</Configuracion>"


            '_B1DireccionIPSQLServer
            '_B1NombreSQLServer  'KBSDIM
            '_B1UsuarioSQLServer
            '_B1PasswordSQLServer

            '_ControlarBitacoraDesdeLlaveCentral
            '_AplicacionInicial
            '_ActivarLogGeneral
            '_RutaLogGeneral
            '_ActivarLogAdministrativo
            '_RutaLogAdministrativo
            '_ActivarBitacoraBasica
            '_ActivarBitacoraGeneral


            'Private _i_controlarBitacoraDesdeLlaveCentral As Int32 = 0
            '_ControlarBitacoraDesdeLlaveCentral

            '' Singleton configuración central
            'Private _singletonConfiguracion As Configuracion = Nothing

            'Private Shared _instancia As SingletonBitacoras = Nothing

            'Private _i_Cve_Aplicacion As Int32

            'Private _i_ActivarLogGeneral As Boolean
            'Private _t_RutaLogGeneral As String

            'Private _i_ActivarLogAdministrativo As Boolean
            'Private _t_RutaLogAdministrativo As String

            ''Básica
            'Private _i_ActivarBitacoraBasica As Boolean
            ''Avanzada
            'Private _i_ActivarBitacoraGeneral As Boolean


            Dim ok_ As String = Nothing


            ok_ = CifraCadena(cadena2_, metodocifrado_, llavecifrado_)

            '-----------Bloque para crear archivos encriptados -------------------

            metodocifrado_.Key = Convert.FromBase64String(llavecifrado_)

            metodocifrado_.Mode = CipherMode.ECB

            Dim encriptado_ As ICryptoTransform = metodocifrado_.CreateDecryptor()

            Dim datos_() As Byte = Convert.FromBase64String(cadena_)

            Dim datosDescifrado_() As Byte = encriptado_.TransformFinalBlock(datos_, 0, datos_.Length)

            _cadena = Encoding.Unicode.GetString(datosDescifrado_)



            Return _cadena

        End Function

        Private Function Cifrar_Aes(ByVal cadena_ As String, ByVal llave_() As Byte, ByVal iv_() As Byte) As Byte()

            'If plainText Is Nothing OrElse plainText.Length <= 0 Then

            '    Throw New ArgumentNullException("plainText")

            'End If

            'If Key Is Nothing OrElse Key.Length <= 0 Then

            '    Throw New ArgumentNullException("Key")

            'End If

            'If IV Is Nothing OrElse IV.Length <= 0 Then

            '    Throw New ArgumentNullException("IV")

            'End If

            Dim cifrado_() As Byte

            Using aesAlg_ As Aes = Aes.Create()

                aesAlg_.Key = llave_

                aesAlg_.IV = iv_

                Dim cifrador_ As ICryptoTransform = aesAlg_.CreateEncryptor(aesAlg_.Key, aesAlg_.IV)

                Using msCifrado_ As New MemoryStream()

                    Using csEncrypt As New CryptoStream(msCifrado_, cifrador_, CryptoStreamMode.Write)

                        Using swCifrado_ As New StreamWriter(csEncrypt)

                            swCifrado_.Write(cadena_)

                        End Using

                        cifrado_ = msCifrado_.ToArray()

                    End Using

                End Using

            End Using

            Return cifrado_

        End Function

        Private Function Descifrar_Aes(ByVal textCifrado_() As Byte, ByVal llave_() As Byte, ByVal iv_() As Byte) As String

            'If cipherText Is Nothing OrElse cipherText.Length <= 0 Then

            '    Throw New ArgumentNullException("cipherText")

            'End If

            'If Key Is Nothing OrElse Key.Length <= 0 Then

            '    Throw New ArgumentNullException("Key")

            'End If

            'If IV Is Nothing OrElse IV.Length <= 0 Then

            '    Throw New ArgumentNullException("IV")

            'End If

            Dim cadena_ As String = Nothing

            Using aesAlg_ As Aes = Aes.Create()

                aesAlg_.Key = llave_

                aesAlg_.IV = iv_

                Dim descifrador_ As ICryptoTransform = aesAlg_.CreateDecryptor(aesAlg_.Key, aesAlg_.IV)

                Using msDescifrar_ As New MemoryStream(textCifrado_)

                    Using csDescifrar_ As New CryptoStream(msDescifrar_, descifrador_, CryptoStreamMode.Read)

                        Using srDescifrar_ As New StreamReader(csDescifrar_)

                            cadena_ = srDescifrar_.ReadToEnd()

                        End Using
                    End Using
                End Using
            End Using

            Return cadena_

        End Function

#End Region

    End Class

End Namespace

