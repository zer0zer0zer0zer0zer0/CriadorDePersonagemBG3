Public Class Form1

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Conecta_banco()
        Carregar_dados()

        'Aplica o estilo sem borda na ToolStrip
        ToolStrip1.Renderer = New SemBordaRenderer()
        ToolStrip1.BackColor = Color.FromArgb(18, 18, 20)
    End Sub

    Private Sub dgv_dados_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_dados.CellContentClick
        Try
            With dgv_dados

                If .CurrentRow.Cells(7).Selected = True Then
                    Dim nome_pers As String = .CurrentRow.Cells(1).Value

                    sql = $"select * from tb_personagem where nome='{nome_pers}'"
                    rs = db.Execute(sql)

                    txt_nome.Text = rs.Fields(1).Value
                    cmb_race.Text = rs.Fields(2).Value
                    cmb_origem.Text = rs.Fields(3).Value
                    cmb_classe.Text = rs.Fields(4).Value
                    txt_lvl.Text = rs.Fields(5).Value
                    txt_hp.Text = rs.Fields(6).Value

                    If Not IsDBNull(rs.Fields(7).Value) Then
                        diretorio = rs.Fields(7).Value
                        img_foto.Load(diretorio)
                    End If

                ElseIf .CurrentRow.Cells(8).Selected = True Then
                    Dim nome_pers As String = .CurrentRow.Cells(1).Value

                    resp = MsgBox("Deseja mesmo apagar o personagem " & nome_pers & "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "CONFIRMAÇÃO")

                    If resp = MsgBoxResult.Yes Then
                        sql = $"delete from tb_personagem where nome='{nome_pers}'"
                        rs = db.Execute(sql)
                        MsgBox("Personagem excluído!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "AVISO")

                        Carregar_dados()
                        Limpar_cadastro()
                    End If
                End If
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles img_foto.Click
        Try
            Using dlg As New OpenFileDialog()
                dlg.Title = "Selecione uma Foto"
                dlg.InitialDirectory = IO.Path.Combine(Application.StartupPath, "Fotos")
                If dlg.ShowDialog() = DialogResult.OK Then
                    diretorio = dlg.FileName
                    img_foto.Load(diretorio)
                End If
            End Using
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub txt_hp_TextChanged(sender As Object, e As EventArgs) Handles txt_hp.TextChanged
    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)
    End Sub

    Private Sub btn_salvar_Click(sender As Object, e As EventArgs) Handles btn_salvar.Click
        Try
            If String.IsNullOrWhiteSpace(txt_nome.Text) Then
                MsgBox("Por favor, informe o nome do personagem!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "ATENÇÃO")
                txt_nome.Focus()
                Exit Sub
            End If

            sql = $"select * from tb_personagem where nome='{txt_nome.Text}'"
            rs = db.Execute(sql)

            ' NOVO CADASTRO
            If rs.EOF = True Then
                sql = $"insert into tb_personagem (nome, raca, origem, classe, nivel, hp, foto) 
                  values ('{txt_nome.Text}',
                          '{cmb_race.Text}',
                          '{cmb_origem.Text}',
                          '{cmb_classe.Text}',
                          '{txt_lvl.Text}',
                          '{txt_hp.Text}',
                          '{diretorio}')"

                rs = db.Execute(UCase(sql))
                MsgBox("Personagem criado com sucesso!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "AVISO")

                ' ATUALIZAÇÃO
            Else
                sql = $"update tb_personagem set raca='{cmb_race.Text}', 
                                         origem='{cmb_origem.Text}',
                                         classe='{cmb_classe.Text}',
                                         nivel='{txt_lvl.Text}',
                                         hp='{txt_hp.Text}',
                                         foto='{diretorio}'
                                         where nome='{txt_nome.Text}'"

                rs = db.Execute(UCase(sql))
                MsgBox("Personagem atualizado com sucesso!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "AVISO")
            End If

            Limpar_cadastro()
            Carregar_dados()

        Catch ex As Exception
            MsgBox("Erro ao salvar o personagem: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERRO")
        End Try
    End Sub

    Private Sub ToolStripTextBox1_Click(sender As Object, e As EventArgs) Handles txt_pesquisar.Click
    End Sub

    ' Pesquisa em tempo real usando o campo da ToolStrip
    Private Sub txt_pesquisar_TextChanged(sender As Object, e As EventArgs) Handles txt_pesquisar.TextChanged
        Try
            sql = $"select * from tb_personagem where nome like '{txt_pesquisar.Text}%' order by nome asc"
            rs = db.Execute(sql)

            With dgv_dados
                cont = 0
                .Rows.Clear()

                Do While rs.EOF = False
                    cont = cont + 1
                    .Rows.Add(cont, rs.Fields(1).Value, rs.Fields(2).Value, rs.Fields(3).Value, rs.Fields(4).Value, rs.Fields(5).Value, rs.Fields(6).Value, Nothing, Nothing)
                    rs.MoveNext()
                Loop
            End With
        Catch ex As Exception
            MsgBox("Erro ao pesquisar: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERRO")
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim resposta As MsgBoxResult
        resposta = MsgBox("Tem certeza que deseja sair do Criador de Personagens?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Criador de Personagens")

        If resposta = MsgBoxResult.No Then
            e.Cancel = True
        End If
    End Sub

End Class

' Classe responsável por remover a borda inferior da ToolStrip
Public Class SemBordaRenderer
    Inherits ToolStripProfessionalRenderer
    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        ' Mantido em branco para não desenhar borda
    End Sub


End Class