'importaciones de espacios de nombre
'Imports System.Data 'ADO Net 
Imports System.Data.SqlClient

Public Class clsConectaBD
    Private cn As SqlConnection

    Sub New()
        cn = New SqlConnection
        'BDLocal - Autenticaciòn windows
        'cn.ConnectionString = "data source=(localdb)\MSSQLLocalDB;Initial catalog=vidrieria;integrated security=SSPI;language=spanish"
        cn.ConnectionString = "data source=DESKTOP-BPJRUBO;Initial catalog=vidrieria;integrated security=SSPI;language=spanish"

        'BDLocal - Autenticaciòn SQL Server

        'cn.ConnectionString = "data source=(local);Initial catalog=BDSistema;user id=sa;password='USAT2024'"
        'cn.ConnectionString = "data source=PCL108PROF;Initial catalog=BDDistribuidos_A;integrated security=SSPI;language=spanish"

        'BD en la nube somee.com
        'cn.ConnectionString = "workstation id=BDPersonal2024.mssql.somee.com;packet size=4096;user id=cdelcastillo_SQLLogin_1;pwd=wptf98uw6j;data source=BDPersonal2024.mssql.somee.com;persist security info=False;initial catalog=BDPersonal2024;language=spanish"
    End Sub

    Public Sub conectar()
        Try
            If cn.State = Data.ConnectionState.Closed Then
                cn.Open()
            End If
        Catch ex As Exception
            Throw New Exception("Error al conectar a BD")
        End Try
    End Sub

    Public Sub desconectar()
        Try
            If cn.State <> Data.ConnectionState.Closed Then
                cn.Close()
            End If
        Catch ex As Exception
            Throw New Exception("Error al desconectar a BD")
        End Try
    End Sub

    Public ReadOnly Property estadoCN() As String
        Get
            If cn.State = Data.ConnectionState.Open Then
                Return "BD está abierta."

            Else
                Return "BD está cerrada."
            End If
        End Get
    End Property

    Public ReadOnly Property miConexion() As SqlConnection
        Get
            Return cn
        End Get
    End Property

    Public ReadOnly Property Servidor() As String
        Get
            Return cn.DataSource.ToString
        End Get
    End Property

    Public Sub abrirconexion()
        Try
            'transaccion = False
            If cn.State <> Data.ConnectionState.Open Then ' SI EL ESTADO DE  LA CONEXION ES DIFERENTE DE ABIERTO ENTONCES ABRE LA CONEXION
                'cn.ConnectionString = "data source=(localdb)\MSSQLLocalDB;Initial catalog=vidrieria;integrated security=SSPI;language=spanish"
                cn.ConnectionString = "data source=DESKTOP-BPJRUBO;Initial catalog=vidrieria;integrated security=SSPI;language=spanish"
                cn.Open()
            End If
        Catch Ex As Exception
            Err.Raise(Err.Number, Err.Source, Err.Description)
        End Try
    End Sub

    Public Sub cerrarconexion()
        Try
            'transaccion = False
            If cn.State = Data.ConnectionState.Open Then
                cn.Close()
                cn.Dispose()
            End If
        Catch Ex As Exception
            Err.Raise(Err.Number, Err.Source, Err.Description)
        End Try
    End Sub

    Public Sub abrirconexionTrans()
        'Try
        '    If transaccion <> True Then
        '        abrirconexion()
        '        tsql = cn.BeginTransaction()
        '        transaccion = True
        '    End If
        'Catch ex As Exception
        '    Err.Raise(Err.Number, Err.Source, Err.Description)
        'End Try

    End Sub

    Public Sub cerrarconexionTrans()
        'Try
        '    If transaccion = True Then
        '        tsql.Commit()
        '        cerrarconexion()
        '        transaccion = False
        '    End If
        'Catch ex As Exception
        '    Err.Raise(Err.Number, Err.Source, Err.Description)
        'End Try
    End Sub

    Public Sub cancelarconexionTrans()
        'Try
        '    If transaccion = True Then
        '        tsql.Rollback()
        '        cerrarconexion()
        '        transaccion = False
        '    End If
        'Catch ex As Exception
        '    Err.Raise(Err.Number, Err.Source, Err.Description)
        'End Try
    End Sub
End Class
