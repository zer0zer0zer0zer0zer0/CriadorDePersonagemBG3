Module Module1
    Public diretorio As String
    Public db As New ADODB.Connection
    Public rs As New ADODB.Recordset
    Public sql, resp As String
    Public cont As Integer

    Sub Carregar_dados()
        Try
            sql = $"select * from tb_personagem order by nome asc"
            rs = db.Execute(sql)

            With Form1.dgv_dados
                cont = 0
                .Rows.Clear()

                Do While rs.EOF = False
                    cont = cont + 1
                    ' Utiliza os índices numéricos dos campos no lugar do nome entre aspas
                    .Rows.Add(cont, rs.Fields(1).Value, rs.Fields(2).Value, rs.Fields(3).Value, rs.Fields(4).Value, rs.Fields(5).Value, rs.Fields(6).Value, "Editar", "Excluir")
                    rs.MoveNext()
                Loop
            End With
        Catch ex As Exception
            MsgBox("Erro ao carregar dados: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ATENÇÃO")
        End Try
    End Sub

    Sub Limpar_cadastro()
        Try
            With Form1
                .txt_nome.Clear()
                .cmb_race.SelectedIndex = -1
                .cmb_origem.SelectedIndex = -1
                .cmb_classe.SelectedIndex = -1
                .txt_lvl.Clear()
                .txt_hp.Clear()
                .img_foto.Load(Application.StartupPath & "\fotos\nova_foto.png")
                .txt_nome.Focus()
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Sub Conecta_banco()
        Try
            db = CreateObject("ADODB.Connection")
            ' Altere Data Source caso esteja a rodar em uma instância com nome específico (ex: .\SQLEXPRESS)
            db.Open("Provider=SQLOLEDB;Data Source=.\SQLEXPRESS;Initial Catalog=bd_rpg;trusted_connection=yes;")
            MsgBox("Conexão estabelecida com sucesso!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "AVISO")
        Catch ex As Exception
            MsgBox("Erro ao conectar ao banco de dados: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ATENÇÃO")
        End Try
    End Sub

End Module